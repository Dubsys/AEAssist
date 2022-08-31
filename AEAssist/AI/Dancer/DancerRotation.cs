using AEAssist.AI.Dancer.GCD;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Enums;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer
{
    [Job(ClassJobType.Dancer)]
    public class DancerRotation : IRotation
    {
        public void Init()
        {
            Random rnd = new Random();
            int step1 = rnd.Next(9000, 13000);
            int PotionTimer = rnd.Next(1650, 1800);
            CountDownHandler.Instance.AddListener(14000, () => SpellsDefine.StandardStep.DoGCD());
            CountDownHandler.Instance.AddListener(step1, () => DancerSpellHelper.PreCombatDanceSteps());
            CountDownHandler.Instance.AddListener(PotionTimer, () =>
                PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId));
            CountDownHandler.Instance.AddListener(100, () => SpellsDefine.DoubleStandardFinish.DoGCD());
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<DancerSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }
        public async Task<bool> NoTarget()
        {
            if (AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs <= 0)
            {
                return false;
            }
            var skill = new DancerGCD_DanceStep2();
            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                await skill.Run();
            }
            return false;
        }
        public SpellEntity GetBaseGCDSpell()
        {
            return SpellsDefine.Cascade.GetSpellEntity();
        }
    }
}