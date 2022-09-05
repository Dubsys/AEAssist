using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Sage.GCD
{
    public class SageGCDDyskrasia : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SpellsDefine.PhlegmaIII.IsMaxChargeReady(0.3f))
            {
                LogHelper.Debug("Max charge for plhelgam is ready don't Dykrasia");
                return -1;
            }
            
            var toxikonCharges = DataManager.GetSpellData(SpellsDefine.Toxikon).Charges;
            var toxikonIICharges = DataManager.GetSpellData(SpellsDefine.ToxikonII).Charges;

            if (toxikonCharges >= 2 && toxikonIICharges >= 2)
            {
                LogHelper.Debug("Toxikon Charges greater than 2 stop dykrasia");
                return -2;
            }

            var checkAoE = TargetHelper.CheckNeedUseAOEByMe(5, 5,  3);
            LogHelper.Debug("Currently AOEChecker is: " + checkAoE);
            if (checkAoE)
            {
                return 0;
            }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SageSpellHelper.GetDyskrasia();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}