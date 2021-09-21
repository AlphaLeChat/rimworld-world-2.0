using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Profile;
using WorldEdit_2_0.Defs;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Templates;

namespace WorldEdit_2_0.Patches.MainMenu
{
    [HarmonyPatch(typeof(ScenPart_PlayerFaction))]
    [HarmonyPatch("PreMapGenerate")]
    [HarmonyPriority(100)]
    public class WE_ScenPart_PlayerFaction_PreMapGenerate
    {
        static bool Prefix(ScenPart_PlayerFaction __instance)
        {
            if (Page_CustomStartingSite.OverrideStartingTile > -1)
            {
                Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
                settlement.SetFaction(Find.GameInitData.playerFaction);
                settlement.Tile = Page_CustomStartingSite.OverrideStartingTile;
                settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
                Find.WorldObjects.Add(settlement);

                return false;
            }

            return true;
        }
    }
}
