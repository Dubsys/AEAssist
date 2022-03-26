﻿//using Clio.Utilities.Collections;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AEAssist;
using TreeSharp;

namespace AEAssist
{
    public class Entry
    {
        private Entry()
        {
        }

        public static Entry Instance { get; } = new Entry();

        public void Initialize()
        {
            LogHelper.Debug("Init....");
            HookBehaviors();
        }

        private ClassJobType CurrentJob { get; set; }
        private ushort CurrentZone { get; set; }

        public void Pulse()
        {
           // LogHelper.Debug("Pulse....");

        }

        public void Shutdown()
        {
            LogHelper.Debug("Shutdown....");
        }

        public void OnButtonPress()
        {
            LogHelper.Debug("OnButtonPress....");
            GUIHelper.OpenGUI();
        }
        

        #region Behavior Composites

        public void HookBehaviors()
        {
            TreeHooks.Instance.ReplaceHook("Rest", RestBehavior);
            TreeHooks.Instance.ReplaceHook("PreCombatBuff", PreCombatBuffBehavior);
            TreeHooks.Instance.ReplaceHook("Pull", PullBehavior);
            TreeHooks.Instance.ReplaceHook("Heal", HealBehavior);
            TreeHooks.Instance.ReplaceHook("CombatBuff", CombatBuffBehavior);
            TreeHooks.Instance.ReplaceHook("Combat", CombatBehavior);
        }

        public Composite RestBehavior
        {
            get
            {
                return
                    new ActionRunCoroutine(ctx => RotationManager.Instance.Rest());
            }
        }

        public Composite PreCombatBuffBehavior
        {
            get
            {
                return
                    new ActionRunCoroutine(ctx => RotationManager.Instance.PreCombatBuff());
            }
        }

        public Composite PullBehavior
        {
            get { return new ActionRunCoroutine(ctx => RotationManager.Instance.Pull()); }
        }

        public Composite HealBehavior
        {
            get { return new ActionRunCoroutine(ctx => RotationManager.Instance.Heal()); }
        }

        public Composite CombatBuffBehavior
        {
            get { return new ActionRunCoroutine(ctx => RotationManager.Instance.CombatBuff()); }
        }

        public Composite CombatBehavior
        {
            get { return new ActionRunCoroutine(ctx => RotationManager.Instance.Combat()); }
        }

        #endregion Behavior Composites
    }
}