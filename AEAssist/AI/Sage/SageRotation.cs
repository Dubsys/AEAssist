using AEAssist.AI.Sage.GCD;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage
{
    [Job(ClassJobType.Sage)]
    public class SageRotation : IRotation
    {
        // private readonly AIRoot AiRoot = AIRoot.Instance;
        // private long _lastTime;
        // private long randomTime;

        public void Init()
        {
            CountDownHandler.Instance.AddListener(15000, SageSpellHelper.PrePullEukrasianDiagnosisThreePeople);
            //CountDownHandler.Instance.AddListener(6000, SageSpellHelper.CastEukrasianPrognosis());
            CountDownHandler.Instance.AddListener(2500, () =>
                PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));
            CountDownHandler.Instance.AddListener(1500, () => SageSpellHelper.GetDosis().DoGCD());
            
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<SageSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }

        public async Task<bool> NoTarget()
        {        
            var skill = new SageGCDEukrasianDiagnosis();
            await skill.Run();
            return true;
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SageSpellHelper.GetBaseGcd();
        }
    }
}