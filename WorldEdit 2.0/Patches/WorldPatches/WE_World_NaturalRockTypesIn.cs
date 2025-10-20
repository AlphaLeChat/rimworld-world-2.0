using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Tiles;

namespace WorldEdit_2_0.Patches.WorldPatches
{
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(World)), HarmonyPatch("NaturalRockTypesIn")]
    public class WE_World_NaturalRockTypesIn
    {
        private static GameComponent_CustomNaturalRocks customNaturalRocks => Current.Game.GetComponent<GameComponent_CustomNaturalRocks>();

        public static bool Prefix(PlanetTile tile, ref IEnumerable<ThingDef> __result)
        {
            if (customNaturalRocks.ResourceData.ContainsKey(tile))
            {
                __result = customNaturalRocks.ResourceData[tile].Rocks;
                return false;
            }

            return true;

        }
    }
}
