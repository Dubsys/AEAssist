﻿using System;
using System.Threading.Tasks;
using AEAssist.AI;
using AEAssist.Define;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.Helper
{
    public static class SpellHelper
    {
        public static async Task<bool> CastGCD(SpellData spell, GameObject target)
        {
            if (!CanCastGCD(spell, target))
                return false;
            if (spell.GroundTarget)
            {
                if (!ActionManager.DoActionLocation(spell.Id, target.Location))
                    return false;
            }
            else
            {
                if (!ActionManager.DoAction(spell, target))
                    return false;
            }
            
            if (spell.AdjustedCastTime != TimeSpan.Zero)
                if (!await Coroutine.Wait(spell.BaseCastTime + TimeSpan.FromMilliseconds(500), () => Core.Me.IsCasting))
                    return false;


            return true;
        }


        public static bool CanCastGCD(SpellData spell, GameObject target)
        {
            if (!ActionManager.HasSpell(spell.Id))
                return false;

            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;

            if (spell.GroundTarget)
            {
                if (!ActionManager.CanCastLocation(spell.Id, target.Location))
                    return false;
            }
            else
            {
                if (DataBinding.Instance.EarlyDecisionMode && !SpellsDefine.IgnoreEarlyDecisionSet.Contains(spell.Id))
                {
                    if (!ActionManager.CanCastOrQueue(spell, target))
                        return false;
                }
                else
                {
                    if (!ActionManager.CanCast(spell, target))
                        return false;
                }
            }

            return true;
        }

        public static async Task<bool> CastAbility(SpellData spell, GameObject target, int waitTime = 0)
        {
            if (waitTime == 0)
                waitTime = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;

            if (!ActionManager.HasSpell(spell.Id))
                return false;
            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;
            
            if (!ActionManager.DoAction(spell, target))
                return false;

            var needTime = (int) spell.AdjustedCastTime.TotalMilliseconds + waitTime;

            if (needTime <= 10)
                return true;

            await Coroutine.Wait(needTime, () => false);
            return true;
        }

        public static bool IsUnlock(this SpellData spellData)
        {
            if (Core.Me.ClassLevel < spellData.LevelAcquired
                || !ActionManager.HasSpell(spellData.Id))
                return false;

            if (AIRoot.GetBattleData<BattleData>().LockSpellId.Contains(spellData.Id))
                return false;
            
            return true;
        }
        
        public static bool IsUnlock(this SpellEntity spellEntity)
        {
            var spellData = spellEntity.SpellData;
            return spellData.IsUnlock();
        }

        public static bool IsReady(this SpellData spellData)
        {
            // LogHelper.Debug($"检测技能 {spellData.Name} {spellData.LocalizedName} AdCoolDown {spellData.AdjustedCooldown.TotalMilliseconds}");
            if (!spellData.IsUnlock())
                return false;

            if (spellData.RecentlyUsed())
                return false;
            
            if (spellData.MaxCharges >= 1)
            {
                if (spellData.Charges >= 1)
                    return true;

                LogHelper.Debug($" {spellData.LocalizedName} Charge {spellData.Charges} MaxCharge {spellData.MaxCharges}!");
                
                if (spellData.SpellType == SpellType.Ability)
                    return false;
                var time = 0;
                if (DataBinding.Instance.EarlyDecisionMode)
                    time = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;
                if (spellData.Cooldown.TotalMilliseconds > time)
                    return false;
                return true;
            }

            if (spellData.SpellType == SpellType.Ability)
            {
                if (spellData.Cooldown.TotalMilliseconds > 0)
                    return false;
            }
            
            return true;
        }
        
        public static bool IsReady(this SpellEntity spellEntity)
        {
            var spellData = spellEntity.SpellData;
            return spellData.IsReady();
        }


        public static bool IsMaxChargeReady(this SpellData spellData,float delta = 0.5f)
        {
            // LogHelper.Debug($"检测技能 {spellData.Name} {spellData.LocalizedName} AdCoolDown {spellData.AdjustedCooldown.TotalMilliseconds}");
            var checkMax = spellData.MaxCharges - delta;
            if (!spellData.IsUnlock()
                || spellData.Charges < checkMax)
                return false;
            return true;
        }
        
        public static bool IsMaxChargeReady(this SpellEntity spellEntity,float delta = 0.5f)
        {
            var spellData = spellEntity.SpellData;
            return spellData.IsMaxChargeReady(delta);
        }

        public static bool CoolDownInGCDs(this SpellData spellData,int count)
        {
            var baseGCD = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            if (spellData.Cooldown.TotalMilliseconds <= baseGCD * count)
            {
                return true;
            }

            return false;
        }
        
        public static bool CoolDownInGCDs(this SpellEntity spellEntity,int count)
        {
            var SpellData = spellEntity.SpellData;
            return SpellData.CoolDownInGCDs(count);
        }
    }
}