﻿using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;

namespace AEAssist.AI.Reaper
{
    [Opener(ClassJobType.Reaper,90)]
    public class Opener_Reaper_90 : IOpener
    {
        public int Check()
        {
            if (!DataBinding.Instance.Burst)
                return -100;
            if (!SpellsDefine.ArcaneCircle.IsReady())
                return -1;
            if (!SpellsDefine.SoulSlice.IsMaxChargeReady(0.1f))
                return -2;
            if (!SpellsDefine.Gluttony.CoolDownInGCDs(7))
                return -3;
            if (!SpellsDefine.Enshroud.IsReady())
                return -4;
            return 0;
        }

        public int StepCount => 4;
        
        [OpenerStep(0)]
        SpellQueueSlot Step0()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = ReaperSpellHelper.GetShadowOfDeath().Id;
            slot.Abilitys.Enqueue((SpellsDefine.ArcaneCircle.Id,SpellTargetType.Self));
            return slot;
        }
        
        [OpenerStep(1)]
        SpellQueueSlot Step1()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            return slot;
        }
        
        [OpenerStep(2)]
        SpellQueueSlot Step2()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            slot.GCDSpellId = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            slot.UsePotion = true;
            return slot;
        }
        
        [OpenerStep(3)]
        SpellQueueSlot Step3()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = SpellsDefine.PlentifulHarvest.Id;
            slot.Abilitys.Enqueue((SpellsDefine.Enshroud.Id,SpellTargetType.Self));
            return slot;
        }
    }
}