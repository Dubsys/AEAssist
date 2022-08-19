using AEAssist.AI;
using AEAssist.AI.Sage;
using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.View.Hotkey.BuiltinHotkeys
{
    public class EukrasianPrognosis : IBuiltinHotkey
    {
        public void OnHotkeyDown()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            slot.SetGCDQueue((SpellsDefine.Eukrasia, SpellTargetType.Self),
                (SpellsDefine.EukrasianPrognosis, SpellTargetType.Self));
            AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
        }

        public string GetDisplayString()
        {
            return Language.Instance.Combox_Hotkey_EukrasianPrognosis;
        }
    }
}