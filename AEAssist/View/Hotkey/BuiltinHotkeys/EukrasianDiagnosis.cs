using AEAssist.AI;
using AEAssist.AI.Sage;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Linq;

namespace AEAssist.View.Hotkey.BuiltinHotkeys
{
    public class EukrasianDiagnosis : IBuiltinHotkey
    {
        public void OnHotkeyDown()
        {
            SettingMgr.GetSetting<SageSettings>().zhudongdandun = !SettingMgr.GetSetting<SageSettings>().zhudongdandun;
            UIHelper.RfreshCurrOverlay();
        }

        public string GetDisplayString()
        {
            return Language.Instance.Combox_Hotkey_EukrasianDiagnosis;
        }
    }
}