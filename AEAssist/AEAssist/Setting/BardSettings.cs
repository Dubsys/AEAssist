﻿using AEAssist.Define;

namespace AEAssist
{
    public class BardSettings
    {
        public static BardSettings Instance = new BardSettings();
        
        
        public double RestHealthPercent = 70.0f;
        public int ApexArrowValue = 95; // 绝峰 多少能量就用

        public SongStrategyEnum CurrentSongPlaylist;

        public int Songs_WM_TimeLeftForSwitch = 3000;
        public int Songs_MB_TimeLeftForSwitch = 10000;
        public int Songs_AP_TimeLeftForSwitch = 3000;
        
    }
}