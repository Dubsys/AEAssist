using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Base : IAIHandler//�������У����壩
    {
        uint spell;
        uint spelled;
        public uint GetSpell()
        {
            if (spelled == 7487 && SpellsDefine.TsubameGaeshi.GetSpellEntity().SpellData.Charges >= 1)
                return SpellsDefine.KaeshiSetsugekka;//�ط�ѩ�»�

            switch (SamuraiSpellHelper.newSenCounts())
            {
                case 0://����
                    return Yukikazecombo();//��ѩ��
                case 1://ѩ��
                    return Gekkocombo();//������
                case 2://����
                    if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//�������������buffֱ�Ӵ򻨳�
                        return SpellsDefine.Kasha;//����
                    else
                        return Yukikazecombo();//��ѩ��
                case 3://ѩ+��
                    return Kashacombo();//����
                case 4://����
                    return Yukikazecombo();//��ѩ��
                case 5://��+��
                    return Yukikazecombo();//��ѩ��
                case 6://ѩ+��
                    return Gekkocombo();//������
                case 7://����
                    return SpellsDefine.MidareSetsugekka;//����ѩ�»�
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        public int Check(SpellEntity lastSpell)
        {
            
            spell = GetSpell();
            //LogHelper.Info($"��ǰ�ȼ�Ϊ��{Core.Me.ClassLevel}dot��Ҫ���� {SamuraiSpellHelper.SenCounts().ToString()}");
           LogHelper.Info($"��һ������: {spell.ToString()},��һ������:{ActionManager.LastSpellId} and {spelled},test: {SamuraiSpellHelper.newSenCounts().ToString()}");
            //LogHelper.Info($"��һ������: {spell.GetSpellEntity().SpellData.LocalizedName},test: {SpellsDefine.TsubameGaeshi.IsReady().ToString()}");
            
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var ret = await spell.DoGCD();
            if (ret)
            {
                if (spell == 7487)
                    spelled = spell;
                else
                    spelled = 0;
                return spell.GetSpellEntity(); 
            }

            return null;
        }
        static public uint Yukikazecombo()//ѩ��
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//�������������buffֱ�Ӵ�ѩ��
                return SpellsDefine.Yukikaze;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://�з�
                    return SpellsDefine.Yukikaze;
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        static public uint Gekkocombo()//����
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//�������������buffֱ�Ӵ��¹�
                return SpellsDefine.Gekko;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://�з�
                    return SpellsDefine.Jinpu;//���
                case SpellsDefine.Jinpu://���
                    return SpellsDefine.Gekko;//�¹�
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        static public uint Kashacombo()//����
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//�������������buffֱ�Ӵ򻨳�
                return SpellsDefine.Kasha;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://�з�
                    return SpellsDefine.Shifu;//ʿ��
                case SpellsDefine.Shifu://ʿ��
                    return SpellsDefine.Kasha;//����
                default:
                    return SpellsDefine.Hakaze;
            }
        }
    }
}