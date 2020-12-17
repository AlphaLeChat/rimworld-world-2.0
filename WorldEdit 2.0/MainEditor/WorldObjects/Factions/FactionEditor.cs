using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Factions
{
    public class FactionEditor : Editor
    {
        protected override Type WindowType => typeof(FactionEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F5;

        public override string EditorName => "WE_Settings_FactionEditorKey".Translate();

        public FactionEditor()
        {
        }

        public Faction GenerateFaction(FactionDef facDef)
        {
            Faction faction = new Faction();
            faction.def = facDef;
            faction.colorFromSpectrum = FactionGenerator.NewRandomColorFromSpectrum(faction);
            if (!facDef.isPlayer)
            {
                if (facDef.fixedName != null)
                {
                    faction.Name = facDef.fixedName;
                }
                else
                {
                    faction.Name = NameGenerator.GenerateName(facDef.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
                                                                                       select fac.Name);
                }
            }
            faction.centralMelanin = Rand.Value;

            return faction;
        }

        public void DeleteFaction(Faction faction)
        {
            if (faction == null)
                return;

            List<Faction> allFactions = typeof(FactionManager).GetField("allFactions", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Find.FactionManager) as List<Faction>;

            if (!allFactions.Contains(faction))
            {
                return;
            }

            List<Settlement> toDelete = (Find.WorldObjects.Settlements.Where(sett => sett.Faction == faction)).ToList();
            foreach (var del in toDelete)
            {
                Find.WorldObjects.Remove(del);
            }

            List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
            for (int i = 0; i < allMapsWorldAndTemporary_AliveOrDead.Count; i++)
            {
                Pawn p = allMapsWorldAndTemporary_AliveOrDead[i];

                if (p.Faction == faction && faction.leader != p)
                {
                    p.SetFaction(null);
                }
            }
            for (int j = 0; j < Find.Maps.Count; j++)
            {
                Find.Maps[j].pawnDestinationReservationManager.Notify_FactionRemoved(faction);
            }

            Find.LetterStack.Notify_FactionRemoved(faction);
            faction.RemoveAllRelations();
            allFactions.Remove(faction);

            faction.leader.SetFaction(null);

            typeof(FactionManager).GetMethod("RecacheFactions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(Find.FactionManager, null);
        }
    }
}
