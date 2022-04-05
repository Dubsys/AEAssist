﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using AEAssist.AI;
using ff14bot.Managers;
using Newtonsoft.Json;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class HotkeySetting : IBaseSetting
    {
        public HotkeySetting()
        {
            Reset();
        }

        public void Reset()
        {
            StopKey = Key.F8.ToString();
            CloseBuffKey = Key.F9.ToString();

            ResetHotkeyName();
        }

        public void ResetHotkeyName()
        {
            StopBtnName = $"停手 {StopKey}";
            CloseBuffBtnName = $"关闭爆发 {CloseBuffKey}";
        }

        [JsonIgnore] private List<Hotkey> Hotkeys;

        public void RegisHotkey()
        {
            try
            {
                if (Hotkeys == null)
                    Hotkeys = new List<Hotkey>();
                UnRegisterKey();
                Hotkeys.Clear();
                // Hotkeys.Add(HotkeyManager.Register("StartCoundDown5s", Keys.F8, ModifierKeys.None,
                //     v => { CountDownHandler.Instance.StartCountDown(); }));

                // Hotkeys.Add(HotkeyManager.Register("BattleStartNow", Keys.F9, ModifierKeys.None,
                //     v => { CountDownHandler.Instance.StartNow(); }));
                if (this.StopKey == null || this.CloseBuffKey == null)
                    Reset();
                //  LogHelper.Info("Hotkey_Stop: " + this.StopKey);
                //  LogHelper.Info("Hotkey_CloseBuff: " + this.CloseBuffKey);

                var stopKey = (Keys) Enum.Parse(typeof(Keys), this.StopKey);
                var closeBuffKey = (Keys) Enum.Parse(typeof(Keys), this.CloseBuffKey);

                Hotkeys.Add(HotkeyManager.Register("BattleStop", stopKey, ModifierKeys.None, v =>
                {
                    AIRoot.Instance.Stop =
                        !AIRoot.Instance.Stop;
                }));

                Hotkeys.Add(HotkeyManager.Register("ControlBuff", closeBuffKey, ModifierKeys.None, v =>
                {
                    AIRoot.Instance.CloseBuff =
                        !AIRoot.Instance.CloseBuff;
                }));
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }

        public void UnRegisterKey()
        {
            if (Hotkeys == null)
                return;
            foreach (var v in Hotkeys)
            {
                HotkeyManager.Unregister(v);
            }
        }

        public string StopBtnName { get; set; }
        public string CloseBuffBtnName { get; set; }

        public string StopKey { get; set; }
        public string CloseBuffKey { get; set; }
    }
}