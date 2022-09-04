using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    internal class DancerGCD_Finish : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            
                if (Core.Me.CurrentTarget.Distance(Core.Me) > 15)
                {
                    return -1;
                }
                if(!(Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep)))
            {
                return -3;
            }
                if (ActionResourceManager.Dancer.CurrentStep == ActionResourceManager.Dancer.DanceStep.Finish)
                {
                    return 0;
                }
            
            return -2;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Cascade.GetSpellEntity();
            var spellnum = ActionResourceManager.Dancer.Steps.Length;            
            if (spellnum == 3)
            {
                spell = SpellsDefine.DoubleStandardFinish.GetSpellEntity();
            }
            if (spellnum == 5)
            {
                spell = SpellsDefine.QuadrupleTechnicalFinish.GetSpellEntity();
            }
            if (spell == SpellsDefine.Cascade.GetSpellEntity())
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}
