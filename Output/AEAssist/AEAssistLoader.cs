﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using AEAssist;
using Clio.Utilities;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using TreeSharp;

using Core = ff14bot.Core;

namespace AEAssist
{
    public class AEAssistLoader : CombatRoutine
    {
        private const string ProjectName = "AEAssist";
        private const string ProjectAssemblyName = "AEAssist.dll";
        public override string Name { get => ProjectName; }
        public override float PullRange { get => 25; }

        public override bool WantButton { get=>true; }
        
        private static readonly string ProjectAssembly = Path.Combine(Environment.CurrentDirectory, $@"Routines\{ProjectName}\{ProjectAssemblyName}");
        private static readonly string GreyMagicAssembly = Path.Combine(Environment.CurrentDirectory, @"GreyMagic.dll");
        
        public static readonly HashSet<string> ExternelDlls = new HashSet<string>()
        {
            "MaterialDesignColors",
            "MaterialDesignThemes.Wpf",
            "MaterialDesignExtensions",
            "MongoDB.Bson"
        };

        public override ClassJobType[] Class
        {
            get
            {
                switch (ff14bot.Core.Me.CurrentJob)
                {
                    case ClassJobType.Bard:
                    case ClassJobType.Reaper:
                        return new[] { ff14bot.Core.Me.CurrentJob };
                    default:
                    {
                        Logging.Write( Colors.Red,$@"[AEAssist] {ff14bot.Core.Me.CurrentJob} is not supported.");
                        return new[] { ff14bot.Core.Me.CurrentJob };
                    }
                }
            }
        }

        public Dictionary<string, PropertyInfo> Behaviors = new Dictionary<string, PropertyInfo>();
        public Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

        public object Entry;

        private bool Loaded;
        
        void LoadAsm()
        {
            RedirectAssembly();
            var path = ProjectAssembly;
            var asm = LoadAssembly(path);

            var entryType = asm.GetType("AEAssist.Entry");

            Entry = Activator.CreateInstance(entryType);
            Behaviors.Clear();
            Methods.Clear();

            AddBehavior(entryType,"RestBehavior");
            AddBehavior(entryType,"PreCombatBuffBehavior");
            AddBehavior(entryType,"PullBehavior");
            AddBehavior(entryType,"HealBehavior");
            AddBehavior(entryType,"CombatBuffBehavior");
            AddBehavior(entryType,"CombatBehavior");
            AddBehavior(entryType,"PullBuffBehavior");

            AddMethod(entryType,"Initialize");
            AddMethod(entryType,"Pulse");
            AddMethod(entryType,"Shutdown");
            AddMethod(entryType,"OnButtonPress");
        }
        
        public static void RedirectAssembly()
        {
            ResolveEventHandler handler = (sender, args) =>
            {
                string name = Assembly.GetEntryAssembly().GetName().Name;
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != name ? null : Assembly.GetEntryAssembly();
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;

            ResolveEventHandler greyMagicHandler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != "GreyMagic" ? null : Assembly.LoadFrom(GreyMagicAssembly);
            };
            

            AppDomain.CurrentDomain.AssemblyResolve += greyMagicHandler;
            
            ResolveEventHandler MaterialDesignHandler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (ExternelDlls.Contains(requestedAssembly.Name))
                    return Assembly.LoadFrom(GetAssPath(requestedAssembly.Name));
                return null;
            };
            AppDomain.CurrentDomain.AssemblyResolve += MaterialDesignHandler;
        }

        static string GetAssPath(string name)
        {
           return  Path.Combine(Environment.CurrentDirectory, $@"Routines\{ProjectName}\{name}.dll");
        }

        private static string CompiledAssembliesPath => Path.Combine(Utilities.AssemblyDirectory, "CompiledAssemblies");

        private static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path)) { return null; }
            if (!Directory.Exists(CompiledAssembliesPath))
            {
                Directory.CreateDirectory(CompiledAssembliesPath);
            }

            var t = DateTime.Now.Ticks;
            var name = $"{Path.GetFileNameWithoutExtension(path)}{t}.{Path.GetExtension(path)}";
            var pdbPath = path.Replace(Path.GetExtension(path), "pdb");
            var pdb = $"{Path.GetFileNameWithoutExtension(path)}{t}.pdb";
            var capath = Path.Combine(CompiledAssembliesPath, name);
            Logging.Write($"Asm: {capath} origin {path}");
            if (File.Exists(capath))
            {
                try
                {
                    File.Delete(capath);
                }
                catch (Exception)
                {
                    //
                }
            }
            if (File.Exists(pdb))
            {
                try
                {
                    File.Delete(pdb);
                }
                catch (Exception)
                {
                    //
                }
            }

            if (!File.Exists(capath))
            {
                File.Copy(path, capath);
            }

            if (!File.Exists(pdb) && File.Exists(pdbPath))
            {
                File.Copy(pdbPath, pdb);
            }


            Assembly assembly = null;
            try { assembly = Assembly.LoadFrom(capath); }
            catch (Exception e) { Logging.WriteException(e); }

            return assembly;
        }

        void AddBehavior(Type type, string name)
        {
            Behaviors.Add(name,type.GetProperty(name));
        }

        void AddMethod(Type type, string name)
        {
            Methods.Add(name,type.GetMethod(name));
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                LoadAsm();
                Methods["Initialize"].Invoke(Entry, null);
            }
            catch (Exception e)
            {
               Logging.Write( Colors.Red,e.ToString());
            }
        }

        public override void OnButtonPress()
        {
            base.OnButtonPress();
            if (!Methods.TryGetValue("OnButtonPress", out var method))
            {
                return;
            }
            method.Invoke(Entry, null);
        }

        public override void Pulse()
        {
            base.Pulse();
            if (!Methods.TryGetValue("Pulse", out var method))
            {
                return;
            }
            method.Invoke(Entry, null);
        }

        public override void ShutDown()
        {
            base.ShutDown();
            if (!Methods.TryGetValue("ShutDown", out var method))
            {
                return;
            }
            method.Invoke(Entry, null);
        }

        public override Composite RestBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("RestBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }
       

        public override Composite PreCombatBuffBehavior         
        {
            get
            {
                if (!Behaviors.TryGetValue("PreCombatBuffBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }
        public override Composite PullBehavior 
        {
            get
            {
                if (!Behaviors.TryGetValue("PullBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite HealBehavior 
        {
            get
            {
                if (!Behaviors.TryGetValue("HealBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite CombatBuffBehavior 
        {
            get
            {
                if (!Behaviors.TryGetValue("CombatBuffBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite CombatBehavior 
        {
            get
            {
                if (!Behaviors.TryGetValue("CombatBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite PullBuffBehavior 
        {
            get
            {
                if (!Behaviors.TryGetValue("PullBuffBehavior", out var prop))
                {
                    return default;
                }
                return prop.GetValue(Entry, null) as Composite;
            }
        }
    }
}