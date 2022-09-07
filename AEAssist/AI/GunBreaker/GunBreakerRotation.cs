using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Gunbreaker
{
    [Job(ClassJobType.Gunbreaker)]
    public class GunBreakerRotation : IRotation
    {
        private readonly AIRoot AiRoot = AIRoot.Instance;

        public void Init()
        {
            //CountDownHandler.Instance.AddListener(300, () =>
            //    SpellsDefine.LightningShot.DoGCD());
            int time = SettingMgr.GetSetting<GunBreakerSettings>().UsePotionEarly;
            if (time > 0)
            {
                CountDownHandler.Instance.AddListener(time, () => PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId));
                LogHelper.Info("UsePotionEarly: " + time + " ms");
            }
            CountDownHandler.Instance.AddListener(300, () =>
            {
                if (DataBinding.Instance.GNBManualControl)
                    return Task.FromResult(false);
                if (DataBinding.Instance.GNBOpen)
                    return SpellsDefine.RoughDivide.DoAbility();
                return SpellsDefine.LightningShot.DoGCD();
            });

            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<GunBreakerSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }

        public async Task<bool> NoTarget()
        {
            var BattleData = AIRoot.GetBattleData<BattleData>();
            var spell = BattleData.NextAbilitySpellId;
            if (spell == null)
                return false;
            if ( await spell.DoAbility())
            {
                AIRoot.GetBattleData<BattleData>().NextAbilitySpellId=null;
            }
            return true;
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SpellsDefine.KeenEdge.GetSpellEntity();
        }
    }
}

