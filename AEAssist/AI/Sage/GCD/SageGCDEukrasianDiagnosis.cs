﻿using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    internal class SageGCDEukrasianDiagnosis : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SettingMgr.GetSetting<SageSettings>().zhudongdandun)
            {
                int num = GroupHelper.CastableAlliesWithin30.Count(r => r.CurrentHealth > 0 && (r.CurrentHealthPercent < 50 && r.IsTank() || r.CurrentHealthPercent < 30) && !r.HasAura(AurasDefine.EukrasianDiagnosis) && !r.HasAura(AurasDefine.DifferentialDiagnosis) && r.IsTank());
                if (num > 0)
                {
                    return 1;
                }
                
            }
            if (!MovementManager.IsMoving) return -1;
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            LogHelper.Debug("刷单盾");
            //Character character = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && !r.HasAura(AurasDefine.EukrasianDiagnosis) && !r.HasAura(AurasDefine.DifferentialDiagnosis));
            //var spell = SageSpellHelper.CastEukrasianDiagnosis(character);
            return SageSpellHelper.CastEukrasianDiagnosisTest();

        }
    }
}
