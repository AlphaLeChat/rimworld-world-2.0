using GeologicalLandforms;
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
    [HarmonyPatch(typeof(TileEditorWindow))]
    [HarmonyPatch("DrawCustom")]
    public class TileEditorWindow_DrawCustom_WorldEditPatch
    {
        private static GameComponent_GeologicalLandforms landformCache => Current.Game.GetComponent<GameComponent_GeologicalLandforms>();

        private static void Postfix(TileEditorWindow __instance, ref Rect inRect, ref float lastPos)
        {
            lastPos += 35;
            WorldTileInfo tileInfo = WorldTileInfo.GetWorldTileInfo(__instance.SelectedTileId);
            Landform landform = Main.Settings.Landforms.TryGetValue(tileInfo.LandformId);

            if (Widgets.ButtonText(new Rect(270, lastPos, 310, 25), "GeologicalLandforms_Landform".Translate(landform == null ? "GeologicalLandforms_Landform.NotSet".Translate().ToString() : landform.TranslatedName)))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();

                options.Add(new FloatMenuOption("GeologicalLandforms_Landform.Reset".Translate(), () =>
                {
                    if (landformCache.TileData.ContainsKey(__instance.SelectedTileId))
                    {
                        landformCache.TileData.Remove(__instance.SelectedTileId);
                    }
                    AccessTools.Field(typeof(WorldTileInfo), "_cache").SetValue(null, null);
                }));

                foreach(var landFormPair in Main.Settings.Landforms)
                {
                    options.Add(new FloatMenuOption(landFormPair.Value.TranslatedName, () =>
                    {
                        landformCache.TileData.SetOrAdd(__instance.SelectedTileId, landFormPair.Key);
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
        }
    }
}
