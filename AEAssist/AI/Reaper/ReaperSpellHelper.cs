﻿using System.Threading.Tasks;
using AEAssist;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Reaper
{
    public static class ReaperSpellHelper
    {
        static async Task<SpellData> UseAOECombo(GameObject target)
        {
            if (AIRoot.Instance.ReaperBattleData.CurrCombo != ReaperComboStages.NightmareScythe
            || ActionManager.ComboTimeLeft<=0)
            {
                if (await SpellHelper.CastGCD(SpellsDefine.SpinningScythe, target))
                {
                    AIRoot.Instance.ReaperBattleData.CurrCombo = ReaperComboStages.NightmareScythe;
                    return SpellsDefine.SpinningScythe;
                }
            }
            else if (await SpellHelper.CastGCD(SpellsDefine.NightmareScythe, target))
            {
                AIRoot.Instance.ReaperBattleData.CurrCombo = ReaperComboStages.SpinningScythe;
                return SpellsDefine.NightmareScythe;
            }

            return null;
        }

        static async Task<SpellData> UseSingleCombo(GameObject target)
        {
            if (ActionManager.ComboTimeLeft > 0)
            {
                if (AIRoot.Instance.ReaperBattleData.CurrCombo == ReaperComboStages.InfernalSlice)
                {
                    if (await SpellHelper.CastGCD(SpellsDefine.InfernalSlice, target))
                    {
                        AIRoot.Instance.ReaperBattleData.CurrCombo = ReaperComboStages.Slice;
                        return SpellsDefine.InfernalSlice;
                    }
                }
                else if (AIRoot.Instance.ReaperBattleData.CurrCombo == ReaperComboStages.WaxingSlice)
                {
                    if (await SpellHelper.CastGCD(SpellsDefine.WaxingSlice, target))
                    {
                        AIRoot.Instance.ReaperBattleData.CurrCombo = ReaperComboStages.InfernalSlice;
                        return SpellsDefine.WaxingSlice;
                    }
                }
            }
            if (await SpellHelper.CastGCD(SpellsDefine.Slice, target))
            {
                AIRoot.Instance.ReaperBattleData.CurrCombo = ReaperComboStages.WaxingSlice;
                return SpellsDefine.Slice;
            }

            return null;
        }

        public static async Task<SpellData> BaseGCDCombo(GameObject target)
        {
            if (TargetHelper.CheckNeedUseAOE(target,5, 5))
            {
                return await UseAOECombo(target);
            }

            return await UseSingleCombo(target);
        }

        public static SpellData CanUseSoulSlice_Scythe(GameObject target)
        {
            if (!SpellsDefine.SoulSlice.IsChargeReady())
                return null;
            if (TargetHelper.CheckNeedUseAOE(target,5, 5))
            {
                return SpellsDefine.SoulScythe;
            }

            return SpellsDefine.SoulSlice;
        }

        public static SpellData Gibbit_Gallows(GameObject target)
        {
            if (!Core.Me.HasAura(AurasDefine.SoulReaver)) return null;

            if (SpellsDefine.Guillotine.IsUnlock() && TargetHelper.CheckNeedUseAOE(8, 8))
            {
                return SpellsDefine.Guillotine;
            }

            if (Core.Me.HasAura(AurasDefine.EnhancedGibbet))
            {
                return SpellsDefine.Gibbet;
            }
           else if (Core.Me.HasAura(AurasDefine.EnhancedGallows))
            {
                return SpellsDefine.Gallows;
            }
            else
            {
                if (SettingMgr.GetSetting<ReaperSettings>().GallowsPrefer)
                    return SpellsDefine.Gallows;
                return SpellsDefine.Gibbet;
            }
        }

        public static SpellData GetEnshroudGCDSpell(GameObject target)
        {
            if (!Core.Me.HasAura(AurasDefine.Enshrouded))
                return null;
            
            if (ActionResourceManager.Reaper.LemureShroud < 2 
                && SpellsDefine.Communio.IsUnlock())
                return SpellsDefine.Communio;

            if (TargetHelper.CheckNeedUseAOE(target, 8, 8))
            {
                return SpellsDefine.GrimReaping;
            }

            if (Core.Me.HasAura(AurasDefine.EnhancedVoidReaping))
                return SpellsDefine.VoidReaping;

            if (Core.Me.HasAura(AurasDefine.EnhancedCrossReaping))
            {
                return SpellsDefine.CrossReaping;
            }

            return SpellsDefine.VoidReaping;
        }

        public static int CheckCanUsePlentifulHarvest()
        {
            if (!SpellsDefine.PlentifulHarvest.IsUnlock())
                return -200;
            
            // 死亡祭祀
            if (Core.Me.HasAura(AurasDefine.BloodsownCircle))
                return -201;

            // 妖异
            if (Core.Me.HasAura(AurasDefine.SoulReaver))
                return -202;
            
            // 死亡祭品
            if (!Core.Me.HasAura(AurasDefine.ImmortalSacrifice))
            {
                return -203;
            }

            // 50点蓝条以上,而且神秘环buff没有接近消失,就延后
            if (ActionResourceManager.Reaper.ShroudGauge > 50 && !Core.Me.ContainsMyInEndAura(AurasDefine.ArcaneCircle,3000))
            {
                return -204;
            }
            
            return 200;
        }

        public static bool CheckIfNeedTrueNorth()
        {
            var target = Core.Me.CurrentTarget;
            var targetSpell = Gibbit_Gallows(target);

            if (targetSpell == SpellsDefine.Guillotine)
                return false;

            if (target.IsFlanking && targetSpell == SpellsDefine.Gallows)
            {
                return true;
            }
            
            if (target.IsBehind && targetSpell == SpellsDefine.Gibbet)
            {
                return true;
            }

            return false;
        }

        public static async Task<SpellData> UseTruthNorth()
        {
            if (!AEAssist.DataBinding.Instance.UseTrueNorth)
                return null;
            if (!CheckIfNeedTrueNorth())
                return null;
            if (await SpellHelper.CastAbility(SpellsDefine.TrueNorth, Core.Me,100))
            {
                return SpellsDefine.TrueNorth;
            }

            return null;
        }

        public static int ReadyToEnshroud()
        {
            if (AIRoot.Instance.CloseBuff)
                return -100;
            if (Core.Me.HasAura(AurasDefine.Enshrouded))
                return -101;
            if (Core.Me.HasAura(AurasDefine.SoulReaver))
                return -102;
            if (ActionResourceManager.Reaper.ShroudGauge < 50)
                return -103;
            
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -104;
            if (!Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget))
                return -105;

            var coolDown = SpellsDefine.ArcaneCircle.Cooldown.TotalMilliseconds;
            
            if (AEAssist.DataBinding.Instance.DoubleEnshroudPrefer
                && ActionResourceManager.Reaper.ShroudGauge < 90
                && coolDown> 2000
                && coolDown < ConstValue.ReaperDoubleEnshroudMaxCheckTime)
            {
                return -106;
            }

            return 0;
        }
    }
}