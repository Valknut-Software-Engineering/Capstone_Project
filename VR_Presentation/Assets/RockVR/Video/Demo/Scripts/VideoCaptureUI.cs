using UnityEngine;
using System.Diagnostics;

namespace RockVR.Video.Demo
{
    public class VideoCaptureUI : MonoBehaviour
    {
        private void Awake()
        {
            Application.runInBackground = true;
        }

        private void OnGUI()
        {
            if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    VideoCaptureCtrl.instance.StartCapture();
                }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    VideoCaptureCtrl.instance.StopCapture();
               }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
            {
              //  if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Processing"))
              //  {
                    // Waiting processing end.
              //  }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    // Open video save directory.
                    Process.Start(PathConfig.saveFolder);
                }
            }
        }
    }
}