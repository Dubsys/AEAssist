﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.Define
{
    internal static class Combat
    {
        public static readonly List<BattleCharacter> Enemies = new List<BattleCharacter>();
        public static readonly Stopwatch CombatTime = new Stopwatch();
        public static readonly Stopwatch OutOfCombatTime = new Stopwatch();
        public static readonly Stopwatch MovingInCombatTime = new Stopwatch();
        public static readonly Stopwatch NotMovingInCombatTime = new Stopwatch();
        public static int CombatTotalTimeLeft;
        public static readonly Stopwatch DutyTime = new Stopwatch();
        public static double CurrentTargetCombatTimeLeft;
        
        public static void AdjustCombatTime()
        {
            //General Combat Status Check
            if (ff14bot.Core.Me.InCombat)
            {
                AdjustInCombatTimers();
                return;
            }

            //If Our Party Has Tagged Something We're Also InCombat
            if (Globals.InParty && Globals.PartyInCombat)
            {
                AdjustInCombatTimers();
                return;
            }

            AdjustOutOfCombatTimers();

            // Private methods
            void AdjustInCombatTimers()
            {
                if (!CombatTime.IsRunning)
                {
                    CombatTime.Start();
                }

                if (OutOfCombatTime.IsRunning)
                {
                    OutOfCombatTime.Reset();
                }

                if (MovementManager.IsMoving)
                {
                    if (!MovingInCombatTime.IsRunning)
                    {
                        MovingInCombatTime.Restart();
                    }

                    if (NotMovingInCombatTime.IsRunning)
                    {
                        NotMovingInCombatTime.Reset();
                    }
                }
                else
                {
                    if (MovingInCombatTime.IsRunning)
                    {
                        MovingInCombatTime.Reset();
                    }

                    if (!NotMovingInCombatTime.IsRunning)
                    {
                        NotMovingInCombatTime.Restart();
                    }
                }
            }

            void AdjustOutOfCombatTimers()
            {
                if (CombatTime.IsRunning)
                {
                    CombatTime.Reset();
                }

                if (!OutOfCombatTime.IsRunning)
                {
                    OutOfCombatTime.Start();
                }
            }
        }

        public static void AdjustDutyTime()
        {
            var inDuty = DutyManager.InInstance;

            if (inDuty && !DutyTime.IsRunning)
                DutyTime.Start();

            if (inDuty || !DutyTime.IsRunning)
                return;

            DutyTime.Reset();
            DutyTime.Stop();
        }

        // public static BattleCharacter SmartAoeTarget(SpellData spell, bool smartAoeSetting = false)
        // {
        //     if (!ff14bot.Core.Me.InCombat)
        //         return null;
        //
        //     if (!smartAoeSetting)
        //         return ff14bot.Core.Me.CurrentTarget == null ? null : (BattleCharacter) ff14bot.Core.Me.CurrentTarget;
        //
        //     var bestTarget = Enemies.Where(x => x.WithinSpellRange(spell.Range))
        //         .OrderBy(x => x.EnemiesNearby(spell.Radius).Count());
        //     
        //     return bestTarget?.FirstOrDefault();
        // }
    }
}