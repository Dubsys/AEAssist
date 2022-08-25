using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Base : IAIHandler//基础连招（单体）
    {
        uint spell;
        uint spelled;
        public uint GetSpell()
        {
            if (spelled == 7487 && SpellsDefine.TsubameGaeshi.GetSpellEntity().SpellData.Charges >= 1)
                return SpellsDefine.KaeshiSetsugekka;//回返雪月花

            switch (SamuraiSpellHelper.newSenCounts())
            {
                case 0://无闪
                    return Yukikazecombo();//打雪连
                case 1://雪闪
                    return Gekkocombo();//打月连
                case 2://月闪
                    if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//如果自身有明镜buff直接打花车
                        return SpellsDefine.Kasha;//花车
                    else
                        return Yukikazecombo();//打雪连
                case 3://雪+月
                    return Kashacombo();//打花连
                case 4://花闪
                    return Yukikazecombo();//打雪连
                case 5://花+月
                    return Yukikazecombo();//打雪连
                case 6://雪+花
                    return Gekkocombo();//打月连
                case 7://三闪
                    return SpellsDefine.MidareSetsugekka;//纷乱雪月花
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        public int Check(SpellEntity lastSpell)
        {
            
            spell = GetSpell();
            //LogHelper.Info($"当前等级为：{Core.Me.ClassLevel}dot需要补吗 {SamuraiSpellHelper.SenCounts().ToString()}");
           LogHelper.Info($"下一个技能: {spell.ToString()},上一个技能:{ActionManager.LastSpellId} and {spelled},test: {SamuraiSpellHelper.newSenCounts().ToString()}");
            //LogHelper.Info($"下一个技能: {spell.GetSpellEntity().SpellData.LocalizedName},test: {SpellsDefine.TsubameGaeshi.IsReady().ToString()}");
            
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
        static public uint Yukikazecombo()//雪连
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//如果自身有明镜buff直接打雪风
                return SpellsDefine.Yukikaze;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://刃风
                    return SpellsDefine.Yukikaze;
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        static public uint Gekkocombo()//月连
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//如果自身有明镜buff直接打月光
                return SpellsDefine.Gekko;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://刃风
                    return SpellsDefine.Jinpu;//阵风
                case SpellsDefine.Jinpu://阵风
                    return SpellsDefine.Gekko;//月光
                default:
                    return SpellsDefine.Hakaze;
            }
        }
        static public uint Kashacombo()//花连
        {
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui, 1000))//如果自身有明镜buff直接打花车
                return SpellsDefine.Kasha;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Hakaze://刃风
                    return SpellsDefine.Shifu;//士风
                case SpellsDefine.Shifu://士风
                    return SpellsDefine.Kasha;//花车
                default:
                    return SpellsDefine.Hakaze;
            }
        }
    }
}