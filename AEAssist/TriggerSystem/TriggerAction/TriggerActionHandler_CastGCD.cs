using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.TriggerAction;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Objects;
using System.Linq;

namespace AEAssist.TriggerSystem.TriggerAction
{
    public class TriggerActionHandler_CastGCD : ATriggerActionHandler<TriggerAction_CastGCD>
    {
        protected override void Handle(TriggerAction_CastGCD t)
        {
            var skillTargets = GroupHelper.CastableAlliesWithin30.Where(a => a.CurrentHealth > 0).OrderBy(GetWeight);
            switch (t.TargetType)
            {
                case 0:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, SpellTargetType.Self);
                    break;
                case 1:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, SpellTargetType.Self);
                    break;
                case 2:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, SpellTargetType.CurrTarget);
                    break;
                case 3:
                    var currTar = Core.Me.CurrentTarget as BattleCharacter;
                    if (currTar == null || currTar.TargetCharacter == null)
                        break;
                    var tt = currTar.TargetCharacter as BattleCharacter;
                    if (tt == null)
                        break;
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, tt);
                    break;
                case 4:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, skillTargets.ElementAt(1) as BattleCharacter);
                    break;
                case 5:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, skillTargets.ElementAt(2) as BattleCharacter);
                    break;
                case 7:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, skillTargets.ElementAt(6) as BattleCharacter);
                    break;
                case 8:
                    AIRoot.GetBattleData<BattleData>().NextGcdSpellId = new SpellEntity(t.SpellId, skillTargets.ElementAt(7) as BattleCharacter);
                    break;
            }
        }
        private static int GetWeight(Character c)
        {
            switch (c.CurrentJob)
            {
                case ClassJobType.Astrologian:
                    return 6;

                case ClassJobType.Monk:
                case ClassJobType.Pugilist:
                    return 8;

                case ClassJobType.BlackMage:
                case ClassJobType.Thaumaturge:
                    return 16;

                case ClassJobType.Dragoon:
                case ClassJobType.Lancer:
                    return 9;

                case ClassJobType.Samurai:
                    return 11;

                case ClassJobType.Machinist:
                    return 14;

                case ClassJobType.Summoner:
                case ClassJobType.Arcanist:
                    return 17;

                case ClassJobType.Bard:
                case ClassJobType.Archer:
                    return 13;

                case ClassJobType.Ninja:
                case ClassJobType.Rogue:
                    return 10;

                case ClassJobType.RedMage:
                    return 18;

                case ClassJobType.Dancer:
                    return 15;

                case ClassJobType.Paladin:
                case ClassJobType.Gladiator:
                    return 2;

                case ClassJobType.Warrior:
                case ClassJobType.Marauder:
                    return 1;

                case ClassJobType.DarkKnight:
                    return 4;

                case ClassJobType.Gunbreaker:
                    return 3;

                case ClassJobType.WhiteMage:
                case ClassJobType.Conjurer:
                    return 5;

                case ClassJobType.Scholar:
                    return 7;

                case ClassJobType.Reaper:
                    return 12;

                case ClassJobType.Sage:
                    return 0;

                case ClassJobType.BlueMage:
                    return 19;
            }

            return c.CurrentJob == ClassJobType.Adventurer ? 70 : 0;
        }
    }
}