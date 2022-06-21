using GeologicalLandforms;
using GeologicalLandforms.GraphEditor;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Tiles;

namespace WorldEdit_GeologicalLandforms
{
    [HarmonyPatch(typeof(WorldTileInfo))]
    [HarmonyPatch("Get")]
    public class WorldTileInfo_Get_WorldEditPatch
    {
        private static GameComponent_GeologicalLandforms landformCache => Current.Game?.GetComponent<GameComponent_GeologicalLandforms>();

        private static void Postfix(ref WorldTileInfo __result)
        {
            if (landformCache != null && __result != null)
            {
                if (landformCache.TileData.TryGetValue(__result.TileId, out GeoTileData geoTileData))
                {
                    List<Landform> list = (List<Landform>)AccessTools.Field(typeof(WorldTileInfo), "_landforms").GetValue(__result);
                    if(list == null)
                    {
                        list = new List<Landform>();
                        AccessTools.Field(typeof(WorldTileInfo), "_landforms").SetValue(__result, list);
                    }

                    list.Clear();
                    foreach (var landformId in geoTileData.landformsIds)
                    {
                        if(LandformManager.Landforms.TryGetValue(landformId, out Landform landform))
                        {
                            list.Add(landform);
                        }
                    }
                }
            }
        }
    }
}
