using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    public class SageNotar : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {            
            return -1;
        }

        public Task<SpellEntity> Run()
        {
            LogHelper.Debug("��Ŀ����");
            return SageSpellHelper.Notar();
        }
    }
}