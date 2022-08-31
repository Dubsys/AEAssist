using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    internal class DancerGCD_DanceStep2 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (ActionResourceManager.Dancer.CurrentStep == ActionResourceManager.Dancer.DanceStep.Finish)
            {
                return null;
            }
            var spell = SpellsDefine.Cascade.GetSpellEntity();
            var spellnum = ActionResourceManager.Dancer.CurrentStep;
            switch (spellnum)
            {
                case ActionResourceManager.Dancer.DanceStep.Pirouette:
                    spell = SpellsDefine.Pirouette.GetSpellEntity();
                    break;
                case ActionResourceManager.Dancer.DanceStep.Emboite:
                    spell = SpellsDefine.Emboite.GetSpellEntity();
                    break;
                case ActionResourceManager.Dancer.DanceStep.Entrechat:
                    spell = SpellsDefine.Entrechat.GetSpellEntity();
                    break;
                case ActionResourceManager.Dancer.DanceStep.Jete:
                    spell = SpellsDefine.Jete.GetSpellEntity();
                    break;
                default:
                    spell = null;
                    break;
            }
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}
