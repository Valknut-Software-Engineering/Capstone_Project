using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace RockVR.Video
{
    /// <summary>
    /// <c>VideoMerger</c> is processed after temp video captured, with or without
    /// temp audio captured. If audio captured, it will merge the video and audio
    /// within same file.
    /// </summary>
    public class VideoMerger
    {
        /// <summary>
        /// The merged video file path.
        /// </summary>
        public string path;
        /// <summary>
        /// The capture video instance.
        /// </summary>
        private VideoCapture videoCapture;
        /// <summary>
        /// The capture audio instance.
        /// </summary>
        private AudioCapture audioCapture;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RockVR.Video.VideoMerger"/> class.
        /// </summary>
        /// <param name="videoCapture">Video capture.</param>
        /// <param name="audioCapture">Audio capture.</param>
        public VideoMerger(VideoCapture videoCapture, AudioCapture audioCapture)
        {
            this.videoCapture = videoCapture;
            this.audioCapture = audioCapture;
        }
        /// <summary>
        /// Video/Audio merge function impl.
        /// Blocking function.
        /// </summary>
        public bool Merge()
        {
            path = PathConfig.saveFolder + StringUtils.GetMp4FileName(StringUtils.GetRandomString(5));
            IntPtr libAPI = LibVideoMergeAPI_Get(
                videoCapture.bitrate,
                path,
                videoCapture.path,
                audioCapture.path,
                PathConfig.ffmpegPath);
            if (libAPI == IntPtr.Zero)
            {
                Debug.LogWarning("[VideoMerger::Merge] Get native LibVideoMergeAPI failed!");
                return false;
            }
            LibVideoMergeAPI_Merge(libAPI);
            // Make sure generated the merge file.
            int waitCount = 0;
            while (!File.Exists(path))
            {
                if (waitCount++ < 100)
                    Thread.Sleep(500);
                else
                {
                    Debug.LogWarning("[VideoMerger::Merge] Merge process failed!");
                    LibVideoMergeAPI_Clean(libAPI);
                    return false;
                }
            }
            LibVideoMergeAPI_Clean(libAPI);
            if (VideoCaptureCtrl.instance.debug)
            {
                Debug.Log("[VideoMerger::Merge] Merge process finish!");
            }
            return true;
        }

        [DllImport("VideoCaptureLib")]
        static extern System.IntPtr LibVideoMergeAPI_Get(int rate, string path, string vpath, string apath, string ffpath);

        [DllImport("VideoCaptureLib")]
        static extern void LibVideoMergeAPI_Merge(IntPtr api);

        [DllImport("VideoCaptureLib")]
        static extern void LibVideoMergeAPI_Clean(IntPtr api);
    }
}