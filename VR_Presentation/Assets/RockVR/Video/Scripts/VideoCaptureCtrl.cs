using UnityEngine;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using RockVR.Common;

namespace RockVR.Video
{
    /// <summary>
    /// <c>VideoCaptureCtrl</c> component, manage and record gameplay from specific camera.
    /// Work with <c>VideoCapture</c> and <c>AudioCapture</c> component to generate gameplay
    /// videos.
    /// </summary>
    public class VideoCaptureCtrl : Singleton<VideoCaptureCtrl>
    {
        /// <summary>
        ///                   NOT_START
        ///                      |
        ///                      | StartCapture()
        ///                      |
        ///                      v
        ///  ---------------> STARTED
        ///  |                   |
        ///  |                   | StopCapture()
        ///  |                   |
        ///  |                   v
        ///  |                STOPPED
        ///  |                   |
        ///  |                   | Process?
        ///  |                   |
        ///  |  StartCapture()   v
        ///  ----------------- FINISH
        /// </summary>
        public enum StatusType
        {
            NOT_START,
            STARTED,
            STOPPED,
            FINISH,
        }
        /// <summary>
        /// Indicates the error of <c>VideoCaptureCtrl</c> module.
        /// </summary>
        public enum ErrorCodeType
        {
            /// <summary>
            /// No camera or audio was found to perform video or audio
            /// recording. You must specify one or more to start record.
            /// </summary>
            CAMERA_AUDIO_CAPTURE_NOT_FOUND = -1,
            /// <summary>
            /// The ffmpeg executable file is not found, this plugin is
            /// depend on ffmpeg to encode videos.
            /// </summary>
            FFMPEG_NOT_FOUND = -2,
            /// <summary>
            /// The audio/video merge process timeout.
            /// </summary>
            VIDEO_AUDIO_MERGE_TIMEOUT = -3,
        }
        /// <summary>
        /// Get or set the current status.
        /// </summary>
        /// <value>The current status.</value>
        public StatusType status { get; private set; }
        /// <summary>
        /// Enable debug message.
        /// </summary>
        public bool debug = false;
        /// <summary>
        /// Delegate to register event.
        /// </summary>
        public EventDelegate eventDelegate = new EventDelegate();
        /// <summary>
        /// Reference to the <c>VideoCapture</c> components (i.e. cameras)
        /// which will be recorded.
        /// Generally you will want to specify at least one.
        /// </summary>
        [SerializeField]
        private VideoCapture[] videoCaptures;
        /// <summary>
        /// Reference to the <c>AudioCapture</c> component for writing audio files.
        /// This needs to be set when you are recording a video with audio.
        /// </summary>
        [SerializeField]
        private AudioCapture audioCapture;
        /// <summary>
        /// How many capture session is complete currently.
        /// </summary>
        private int videoCaptureFinishCount;
        /// <summary>
        /// The count of camera is enabled for capturing.
        /// </summary>
        private int videoCaptureRequiredCount;
        /// <summary>
        /// The audio/video merge thread.
        /// </summary>
        private Thread videoMergeThread;
        /// <summary>
        /// The garbage collection thread.
        /// </summary>
        private Thread garbageCollectionThread;
        /// <summary>
        /// Whether record audio.
        /// </summary>
        private bool isCaptureAudio;
        /// <summary>
        /// Get or set the <c>VideoCapture</c> components.
        /// </summary>
        /// <value>The <c>VideoCapture</c> components.</value>
        public VideoCapture[] VideoCaptures
        {
            get
            {
                return videoCaptures;
            }
            set
            {
                if (status == StatusType.STARTED)
                {
                    Debug.LogWarning("[VideoCaptureCtrl::VideoCaptures] Cannot " +
                                     "set camera during capture session!");
                    return;
                }
                videoCaptures = value;
            }
        }
        /// <summary>
        /// Get or set the <c>AudioCapture</c> component.
        /// </summary>
        /// <value>The <c>AudioCapture</c> component.</value>
        public AudioCapture AudioCapture
        {
            get
            {
                return audioCapture;
            }
            set
            {
                if (status == StatusType.STARTED)
                {
                    Debug.LogWarning("[VideoCaptureCtrl::AudioCapture] Cannot " +
                                     " set aduio during capture session!");
                    return;
                }
                audioCapture = value;
            }
        }
        /// <summary>
        /// Initialize the attributes of the capture session and start capture.
        /// </summary>
        public void StartCapture()
        {
            if (status != StatusType.NOT_START &&
                status != StatusType.FINISH)
            {
                Debug.LogWarning("[VideoCaptureCtrl::StartCapture] Previous " +
                                 " capture not finish yet!");
                return;
            }
            // Filter out disabled capture component.
            List<VideoCapture> validCaptures = new List<VideoCapture>();
            if (VideoCaptures != null && VideoCaptures.Length > 0)
            {
                foreach (VideoCapture videoCapture in VideoCaptures)
                {
                    if (videoCapture != null && videoCapture.gameObject.activeSelf)
                    {
                        validCaptures.Add(videoCapture);
                    }
                }
            }
            VideoCaptures = validCaptures.ToArray();
            // Cache those value, thread cannot access unity's object.
            isCaptureAudio = false;
            if (AudioCapture != null && AudioCapture.gameObject.activeSelf)
                isCaptureAudio = true;
            // Check if can start a capture session.
            if (!isCaptureAudio && VideoCaptures.Length == 0)
            {
                Debug.LogError("[VideoCaptureCtrl::StartCapture] StartCapture called " +
                    "but no attached VideoRecorder or AudioRecorder were found!"
                );
                return;
            }
            if (!File.Exists(PathConfig.ffmpegPath))
            {
                Debug.LogError(
                    "[VideoCaptureCtrl::StartCapture] FFmpeg not found, please add " +
                    "ffmpeg executable before start capture!"
                );
                return;
            }
            // Loop through each of the video capture component, initialize 
            // and start recording session.
            videoCaptureRequiredCount = 0;
            for (int i = 0; i < VideoCaptures.Length; i++)
            {
                VideoCapture videoCapture = VideoCaptures[i];
                if (videoCapture == null || !videoCapture.gameObject.activeSelf)
                {
                    continue;
                }
                videoCaptureRequiredCount++;
                videoCapture.StartCapture();
                videoCapture.eventDelegate.OnComplete += OnVideoCaptureComplete;
            }
            // Check if record audio.
            if (isCaptureAudio)
            {
                AudioCapture.StartCapture();
                AudioCapture.eventDelegate.OnComplete += OnAudioCaptureComplete;
            }
            // Reset record session count.
            videoCaptureFinishCount = 0;
            // Start garbage collect thread.
            garbageCollectionThread = new Thread(GarbageCollectionThreadFunction);
            garbageCollectionThread.Priority = System.Threading.ThreadPriority.Lowest;
            garbageCollectionThread.IsBackground = true;
            garbageCollectionThread.Start();
            // Update current status.
            status = StatusType.STARTED;
        }
        /// <summary>
        /// Stop capturing and produce the finalized video. Note that the video file
        /// may not be completely written when this method returns. In order to know
        /// when the video file is complete, register <c>OnComplete</c> delegate.
        /// </summary>
        public void StopCapture()
        {
            if (status != StatusType.STARTED)
            {
                Debug.LogWarning("[VideoCaptureCtrl::StopCapture] capture session " +
                                 "not start yet!");
                return;
            }
            foreach (VideoCapture videoCapture in VideoCaptures)
            {
                if (!videoCapture.gameObject.activeSelf)
                {
                    continue;
                }
                videoCapture.StopCapture();
            }

            if (isCaptureAudio)
            {
                AudioCapture.StopCapture();
            }
            status = StatusType.STOPPED;
        }
        /// <summary>
        /// Handle callbacks for the <c>VideoCapture</c> complete.
        /// </summary>
        private void OnVideoCaptureComplete()
        {
            videoCaptureFinishCount++;
            if (
                videoCaptureFinishCount == videoCaptureRequiredCount && // Finish all video capture.
                !isCaptureAudio // No audio capture required.
            )
            {
                status = StatusType.FINISH;
                if (eventDelegate.OnComplete != null)
                    eventDelegate.OnComplete();
            }
        }
        /// <summary>
        /// Handles callbacks for the <c>AudioCapture</c> complete.
        /// </summary>
        private void OnAudioCaptureComplete()
        {
            // Start merging thread when we have videos captured.
            if (isCaptureAudio)
            {
                videoMergeThread = new Thread(VideoMergeThreadFunction);
                videoMergeThread.Priority = System.Threading.ThreadPriority.Lowest;
                videoMergeThread.IsBackground = true;
                videoMergeThread.Start();
            }
        }
        /// <summary>
        /// Media merge the thread function.
        /// </summary>
        private void VideoMergeThreadFunction()
        {
            // Wait for all video record finish.
            while (videoCaptureFinishCount < videoCaptureRequiredCount)
            {
                Thread.Sleep(1000);
            }
            foreach (VideoCapture videoCapture in VideoCaptures)
            {
                // TODO, make audio live streaming work
                if (
                    videoCapture.mode == VideoCapture.ModeType.LIVE_STREAMING ||
                    // Dont merge audio when capture equirectangular, its not sync.
                    videoCapture.format == VideoCapture.FormatType.PANORAMA)
                {
                    continue;
                }
                VideoMerger videoMerger = new VideoMerger(videoCapture, AudioCapture);
                if (!videoMerger.Merge())
                {
                    if (eventDelegate.OnError != null)
                        eventDelegate.OnError((int)ErrorCodeType.VIDEO_AUDIO_MERGE_TIMEOUT);
                }
            }
            Cleanup();
            status = StatusType.FINISH;
            if (eventDelegate.OnComplete != null)
                eventDelegate.OnComplete();
        }
        /// <summary>
        /// Garbage collection thread function.
        /// </summary>
        void GarbageCollectionThreadFunction()
        {
            while (status == StatusType.STARTED)
            {
                // TODO, adjust gc interval dynamic.
                Thread.Sleep(1000);
                System.GC.Collect();
            }
        }
        /// <summary>
        /// Cleanup this instance.
        /// </summary>
        void Cleanup()
        {
            foreach (VideoCapture videoCapture in VideoCaptures)
            {
                // Dont clean panorama video, its not include in merge thread.
                if (videoCapture.format == VideoCapture.FormatType.PANORAMA)
                {
                    continue;
                }
                videoCapture.Cleanup();
            }
            if (isCaptureAudio)
            {
                AudioCapture.Cleanup();
            }
        }
        /// <summary>
        /// Initial instance and init variable.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            // For easy access the CameraCaptures var.
            if (VideoCaptures == null)
                VideoCaptures = new VideoCapture[0];
            // Create default root folder if not created.
            if (!Directory.Exists(PathConfig.saveFolder))
            {
                Directory.CreateDirectory(PathConfig.saveFolder);
            }
            status = StatusType.NOT_START;
        }
        /// <summary>
        /// Check if still processing on application quit.
        /// </summary>
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            // Issue an interrupt if still capturing.
            if (status == StatusType.STARTED)
            {
                StopCapture();
            }
        }
    }
}
