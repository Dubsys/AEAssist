﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityCelestialIntersection:IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<AstSettings>().Heal)
            {
                return -5;
            }
            if (!SpellsDefine.CelestialIntersection.IsReady()) return -1;
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 80f && r.IsTank());
            if (skillTarget == null)
            {
                return -2;
            }
            if (!SpellsDefine.CelestialIntersection.IsMaxChargeReady())
            {
                return -1;
            }
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            return AstSpellHelper.CastCelestialIntersection();
        }
    }
}