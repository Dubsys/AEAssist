﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class BardAbility_UsePotion : IAIHandler
    {
        public bool Check(SpellData lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return false;
            if (AIRoot.Instance.CloseBuff)
                return false;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return false;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<BardSettings>().PotionId))
                return false;

            return true;
        }

        public async Task<SpellData> Run()
        {
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<BardSettings>().PotionId);
            if (ret)
            {
                AIRoot.Instance.MuteAbilityTime();
            }

            return null;
        }
    }
}