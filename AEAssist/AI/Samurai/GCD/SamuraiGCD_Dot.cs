using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Dot : IAIHandler
    {
        uint spell = SpellsDefine.Higanbana;
        public int Check(SpellEntity lastSpell)
        {
            var tar = Core.Me.CurrentTarget as Character;
            bool hasHig = tar.HasMyAuraWithTimeleft(AurasDefine.Higanbana, 10000);
            return -1;
            if (hasHig)//����Ҫ��dotֱ������
                return -1;
            if (SamuraiSpellHelper.SenCounts()>1)
                return -1;

                if (ActionManager.LastSpellId == 7477)//����Ѿ��ͷ��з�ʹ�ѩ��
                    spell = SpellsDefine.Yukikaze;
                else
                    spell = SpellsDefine.Hakaze;

            if(Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//�������������buffֱ�Ӵ�ѩ��
                spell = SpellsDefine.Yukikaze;

            if (!hasHig && SamuraiSpellHelper.SenCounts() == 1)//��Ҫ��dot���������ʹ�˰���
                spell = SpellsDefine.Higanbana;

            //LogHelper.Info($"��������Ϊ��{SamuraiSpellHelper.SenCounts()},{hasHig}��dot��Ҫ���� {adddot}");
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