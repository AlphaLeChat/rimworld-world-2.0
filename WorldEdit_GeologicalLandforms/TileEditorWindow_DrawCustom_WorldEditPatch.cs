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
    [HarmonyPatch(typeof(TileEditorWindow))]
    [HarmonyPatch("DrawCustom")]
    public class TileEditorWindow_DrawCustom_WorldEditPatch
    {
        private static GameComponent_GeologicalLandforms landformCache => Current.Game.GetComponent<GameComponent_GeologicalLandforms>();

        private static Vector2 scrollPosition;

        private static void Postfix(TileEditorWindow __instance, ref Rect inRect, ref float lastPos)
        {
            lastPos += 35;
            WorldTileInfo tileInfo = WorldTileInfo.Get(__instance.SelectedTileId);
            if (tileInfo == null)
                return;

            Widgets.Label(new Rect(270, lastPos, 360, 25), "WE2.0.GeologicalLandforms".Translate());
            lastPos += 25;
            if(Widgets.ButtonText(new Rect(270, lastPos, 145, 25), "WE2.0.GeologicalLandforms.Add".Translate()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();

                foreach (var landFormPair in LandformManager.Landforms)
                {
                    if (!landformCache.TileData.ContainsKey(__instance.SelectedTileId))
                    {
                        foreach (Landform landform in tileInfo.Landforms)
                        {
                            landformCache.Add(__instance.SelectedTileId, landform.Id);
                        }
                    }

                    options.Add(new FloatMenuOption(landFormPair.Value.TranslatedName, () =>
                    {
                        landformCache.Add(__instance.SelectedTileId, landFormPair.Key);
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
            if (Widgets.ButtonText(new Rect(420, lastPos, 145, 25), "WE2.0.GeologicalLandforms.Reset".Translate()))
            {
                if (landformCache.TileData.ContainsKey(__instance.SelectedTileId))
                {
                    landformCache.TileData.Remove(__instance.SelectedTileId);
                }
                AccessTools.Field(typeof(WorldTileInfo), "_cache").SetValue(null, null);
            }

            lastPos += 25;

            if (tileInfo.Landforms == null)
                return;

            Rect scrollRect = new Rect(270, lastPos, 310, 130);
            Rect viewRect = new Rect(0, 0, scrollRect.width - 16f, tileInfo.Landforms.Count * 30);
            Widgets.BeginScrollView(scrollRect, ref scrollPosition, viewRect);
            float y = 0;
            if (tileInfo.Landforms != null)
            {
                for (int i = 0; i < tileInfo.Landforms.Count; i++)
                {
                    Rect rect = new Rect(0, y, viewRect.width - 80f, 25);
                    Widgets.Label(rect, tileInfo.Landforms[i].TranslatedName);

                    Rect buttonRect = new Rect(rect.xMax, y, viewRect.width - rect.xMax, 25);
                    if (Widgets.ButtonText(buttonRect, "WE2.0.GeologicalLandforms.Delete".Translate()))
                    {
                        if (!landformCache.TileData.ContainsKey(__instance.SelectedTileId))
                        {
                            foreach (Landform landform in tileInfo.Landforms)
                            {
                                landformCache.Add(__instance.SelectedTileId, landform.Id);
                            }
                        }

                        landformCache.Remove(__instance.SelectedTileId, tileInfo.Landforms[i].Id);
                    }

                    y += 30;
                }
            }

            Widgets.EndScrollView();

            //if (Widgets.ButtonText(new Rect(270, lastPos, 310, 25), "GeologicalLandforms_Landform".Translate(tileInfo.Landform == null ? "GeologicalLandforms_Landform.NotSet".Translate().ToString() : tileInfo.Landform.TranslatedName)))
            //{
            //    List<FloatMenuOption> options = new List<FloatMenuOption>();

            //    options.Add(new FloatMenuOption("GeologicalLandforms_Landform.Reset".Translate(), () =>
            //    {
            //        if (landformCache.TileData.ContainsKey(__instance.SelectedTileId))
            //        {
            //            landformCache.TileData.Remove(__instance.SelectedTileId);
            //        }
            //        AccessTools.Field(typeof(WorldTileInfo), "_cache").SetValue(null, null);
            //    }));

            //    foreach(var landFormPair in LandformManager.Landforms)
            //    {
            //        options.Add(new FloatMenuOption(landFormPair.Value.TranslatedName, () =>
            //        {
            //            landformCache.TileData.SetOrAdd(__instance.SelectedTileId, landFormPair.Key);
            //        }));
            //    }

            //    Find.WindowStack.Add(new FloatMenu(options));
            //}
        }
    }
}
