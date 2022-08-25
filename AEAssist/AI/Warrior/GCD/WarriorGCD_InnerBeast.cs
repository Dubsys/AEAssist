using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;
using ff14bot.Managers;
namespace AEAssist.AI.Warrior.GCD
{
    public class WarriorGCD_InnerBeast : IAIHandler
    {
        uint spell = SpellsDefine.InnerBeast;//狂魂
        public int Check(SpellEntity lastSpell)
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.WhiteMageAOECount);
            if (aoeChecker) return -1;//需要AOE就不放
            if (!Core.Me.HasMyAura(AurasDefine.NascentChaos)) return -1;//没有战嚎BUFF就不放
            if (!Core.Me.HasMyAura(AurasDefine.SurgingTempest)) return -1;//没有红斩BUFF就不放
            if (ActionResourceManager.Warrior.BeastGauge < 50) return -1;//兽魂不足50就不放
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}