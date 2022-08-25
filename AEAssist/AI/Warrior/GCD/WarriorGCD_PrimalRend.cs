using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Warrior.GCD
{
    public class WarriorGCD_PrimalRend : IAIHandler
    {
        uint spell;//蛮荒崩裂

        public int Check(SpellEntity lastSpell)
        {
            spell = SpellsDefine.PrimalRend;
            if (!SettingMgr.GetSetting<WarriorSettings>().WarriorPrimalRend)
                return -5;
            if (!Core.Me.HasMyAura(AurasDefine.PrimalRendReady)) return -1;
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