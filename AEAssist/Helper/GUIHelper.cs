﻿using System.Windows;
using System.Windows.Forms;
using AEAssist.Helper;
using MessageBox = System.Windows.MessageBox;

namespace AEAssist
{
    public static class GUIHelper
    {
        public static void OpenGUI()
        {
            // if (GUI == null || GUI.IsDisposed || GUI.Disposing)
            //     GUI = new GUI();
            // GUI.Show();
        }

        public static void Close()
        {
           // GUI.Close();
        }
        
        public static void OpenOverlay()
        {
            // if (Overlay == null || Overlay.IsDisposed || Overlay.Disposing)
            //     Overlay = new Overlay();
            // Overlay.Show();
        }
        
        public static void CloseOverlay()
        {
           // Overlay.Close();
        }

        private static long targetShowTime;
        public static void ShowInfo(string msg,int time = 0,bool check = true)
        {
            if (check &&  TimeHelper.Now() < targetShowTime)
                return;
            if (time > 0)
                targetShowTime = TimeHelper.Now() + time;
            //Overlay.ShowDebug(msg);
        }

        public static void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}