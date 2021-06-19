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

namespace WorldEdit_2_0.MainEditor.Tiles
{
    public class TileEditorWindow : EditWindow
    {
        enum SetType : byte
        {
            Temperature = 0,
            Elevation,
            Rainfall,
            Swampiness
        }

        private TileEditor tileEditor;

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public override Vector2 InitialSize => new Vector2(600, 700);
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 scrollPositionNaturalRocks = Vector2.zero;

        private BiomeDef selectedBiome;

        private Hilliness selectedHilliness;

        private int biomeDefSize;

        private List<LayerSubMesh> terrainSubMeshes;
        private List<LayerSubMesh> hillinessSubMeshes;

        private WorldUpdater worldUpdater => worldEditor.WorldUpdater;

        private string temperatureTmpField;
        private float temperature;

        private string elevationTmpField;
        private float elevation;

        private string rainfallTmpField;
        private float rainfall;

        private string swampinessTmpField;
        private float swampiness;

        private int selectedTileId = -1;
 
        private CustomRock customRockData = null;

        private GameComponent_CustomNaturalRocks customNaturalRocks => Current.Game.GetComponent<GameComponent_CustomNaturalRocks>();
        private List<ThingDef> customRocksTmp;

        private IntRange brushRadius = new IntRange();
        private bool brushEnabled = false;

        public TileEditorWindow(TileEditor tileEditor)
        {
            resizeable = false;
            this.tileEditor = tileEditor;

            biomeDefSize = tileEditor.AvaliableBiomes.Count * 25;

            terrainSubMeshes = tileEditor.LayersSubMeshes["WorldLayer_Terrain"];
            hillinessSubMeshes = tileEditor.LayersSubMeshes["WorldLayer_Hills"];
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            WidgetRow row = new WidgetRow(0, 0, UIDirection.RightThenDown, 580);
            if (row.ButtonText(Translator.Translate("TileEditorWindow_UpdateAllTiles"), Translator.Translate("TileEditorWindow_UpdateAllTilesInfo")))
            {
                foreach(var layer in tileEditor.Layers)
                {
                    worldUpdater.UpdateLayer(layer.Value);
                }
                //worldUpdater.UpdateMap();
            }
            if (row.ButtonText(Translator.Translate("TileEditorWindow_UpdateHillsLayer"), Translator.Translate("TileEditorWindow_UpdateHillsLayerInfo")))
            {
                worldUpdater.UpdateLayer(tileEditor.Layers["WorldLayer_Hills"]);
            }
            if (row.ButtonText(Translator.Translate("TileEditorWindow_UpdateTerrainLayer"), Translator.Translate("TileEditorWindow_UpdateTerrainLayerInfo")))
            {
                worldUpdater.UpdateLayer(tileEditor.Layers["WorldLayer_Terrain"]);
            }

            Widgets.Label(new Rect(80, 25, 50, 20), Translator.Translate("TileEditorWindow_Biome"));
            Rect scrollRect = new Rect(0, 45, 250, 220);
            Rect scrollVertRect = new Rect(0, 0, scrollRect.x, biomeDefSize);
            Widgets.BeginScrollView(scrollRect, ref scrollPosition, scrollVertRect);
            int yButtonPos = 5;
            Text.Font = GameFont.Small;
            if (Widgets.ButtonText(new Rect(0, yButtonPos, 230, 20), Translator.Translate("TileEditorWindow_NoText")))
            {
                selectedBiome = null;
            }
            if(selectedBiome == null)
            {
                GUI.color = Color.white;
                Widgets.DrawBox(new Rect(0, yButtonPos, 230, 20), 2);
            }
            yButtonPos += 25;
            foreach (BiomeDef def in tileEditor.AvaliableBiomes)
            {
                Rect buttonRect = new Rect(0, yButtonPos, 230, 20);

                if (Widgets.ButtonText(buttonRect, def.label))
                {
                    selectedBiome = def;
                }

                if (def == selectedBiome)
                {
                    GUI.color = Color.white;
                    Widgets.DrawBox(buttonRect, 2);
                }

                yButtonPos += 22;
            }
            Widgets.EndScrollView();
            if (Widgets.ButtonText(new Rect(0, 275, 250, 20), "TileEditorWindow_SetBiomeToWholeMap".Translate()))
            {
                tileEditor.SetBiomeToWholeMap(selectedBiome, false);
            }

            Widgets.Label(new Rect(380, 25, 50, 20), Translator.Translate("TileEditorWindow_Hilliness"));
            yButtonPos = 50;
            foreach (Hilliness hillines in Enum.GetValues(typeof(Hilliness)))
            {
                if (Widgets.RadioButtonLabeled(new Rect(260, yButtonPos, 310, 20), hillines == Hilliness.Undefined ? "TileEditorWindow_Undefined".Translate().RawText : hillines.GetLabel(), hillines == selectedHilliness))
                {
                    selectedHilliness = hillines;
                }
                yButtonPos += 25;
            }

            yButtonPos = 300;
            Widgets.Label(new Rect(0, yButtonPos, 250, 20), "TileEditorWindow_CurrentTileInfo".Translate(selectedTileId));
            yButtonPos = 320;
            DrawTileParameter(Translator.Translate("TileEditorWindow_Temperature"), ref temperatureTmpField, ref temperature, ref yButtonPos, SetType.Temperature);
            DrawTileParameter(Translator.Translate("TileEditorWindow_Elevation"), ref elevationTmpField, ref elevation, ref yButtonPos, SetType.Elevation);
            DrawTileParameter(Translator.Translate("TileEditorWindow_Rainfall"), ref rainfallTmpField, ref rainfall, ref yButtonPos, SetType.Rainfall);
            DrawTileParameter(Translator.Translate("TileEditorWindow_Swampiness"), ref swampinessTmpField, ref swampiness, ref yButtonPos, SetType.Swampiness);

            if (Widgets.RadioButtonLabeled(new Rect(0, yButtonPos, 250, 20), $"{Translator.Translate("TileEditorWindow_BrushEnable")} - {brushRadius.min} / {brushRadius.max}", brushEnabled))
            {
                brushEnabled = !brushEnabled;
            }
            yButtonPos += 25;
            Widgets.IntRange(new Rect(0, yButtonPos, 250, 20), 3424354, ref brushRadius, 0, 100, Translator.Translate("TileEditorWindow_BrushSettings"));

            //right side
            if (customRockData != null)
            {
                yButtonPos = 300;

                if (Widgets.ButtonText(new Rect(260, yButtonPos, 310, 20), Translator.Translate("TileEditorWindow_AddNewNaturalRock")))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (var thing in (from d in DefDatabase<ThingDef>.AllDefs
                                           where d.category == ThingCategory.Building && d.building.isNaturalRock && !d.building.isResourceRock && !d.IsSmoothed
                                           select d))
                        list.Add(new FloatMenuOption(thing.LabelCap, delegate
                        {
                            customRocksTmp.Add(thing);
                        }));
                    Find.WindowStack.Add(new FloatMenu(list));
                }

                yButtonPos += 25;

                int size = customRocksTmp.Count * 22;
                Rect scrollRectFact = new Rect(260, yButtonPos, 310, 190);
                Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, size);
                Widgets.BeginScrollView(scrollRectFact, ref scrollPositionNaturalRocks, scrollVertRectFact);
                int y = 0;
                for (int i = 0; i < customRocksTmp.Count; i++)
                {
                    ThingDef d = customRocksTmp[i];
                    Widgets.Label(new Rect(0, y, 280, 20), d.LabelCap);
                    if (Widgets.ButtonText(new Rect(290, y, 20, 20), "X"))
                    {
                        customRocksTmp.Remove(d);
                    }
                    y += 22;
                }
                Widgets.EndScrollView();

                yButtonPos += 200;

                if (Widgets.RadioButtonLabeled(new Rect(270, yButtonPos, 240, 40), Translator.Translate("TileEditorWindow_HasCaves"), customRockData.Caves == true))
                {
                    customRockData.Caves = !customRockData.Caves;
                }
            }
        }

        private void DrawTileParameter(string label, ref string tmpField, ref float param, ref int yButtonPos, SetType setType)
        {
            Widgets.Label(new Rect(0, yButtonPos, 100, 20), label);
            tmpField = Widgets.TextField(new Rect(110, yButtonPos, 140, 20), tmpField);
            if (float.TryParse(tmpField, out float value))
            {
                param = value;
            }
            yButtonPos += 25;
            if (Widgets.ButtonText(new Rect(0, yButtonPos, 250, 20), Translator.Translate("TileEditorWindow_SetToAllMap")))
            {
                SetToAllMap(setType);
            }
            yButtonPos += 25;
            if (Widgets.ButtonText(new Rect(0, yButtonPos, 250, 20), Translator.Translate("TileEditorWindow_SetToAllBiome")))
            {
                SetToAllBiomes(setType);
            }
            yButtonPos += 25;
        }


        public override void WindowUpdate()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if(brushEnabled)
                {
                    RadiusTile();
                }
                else
                {
                    int tileID = GenWorld.MouseTile();

                    SingleTile(tileID);
                }
            }

            int clickTileId = Find.WorldSelector.selectedTile;
            if (clickTileId >= 0 && clickTileId != selectedTileId)
            {
                selectedTileId = clickTileId;
                UpdateTileInfo(clickTileId);
            }

        }

        private void SingleTile(int tileID)
        {
            if (tileID >= 0)
            {
                Tile tile = Find.WorldGrid[tileID];

                if (tile != null)
                {
                    if (selectedBiome != null)
                    {
                        if (selectedBiome != tile.biome)
                        {
                            tile.biome = selectedBiome;

                            if (selectedBiome == BiomeDefOf.Ocean || selectedBiome == BiomeDefOf.Lake)
                            {
                                tile.elevation = -400f;
                            }

                            worldEditor.WorldUpdater.RenderSingleTile(tileID, tile.biome.DrawMaterial, terrainSubMeshes);
                        }
                    }

                    if (selectedHilliness != Hilliness.Undefined)
                    {
                        if (tile.hilliness != selectedHilliness)
                        {
                            tile.hilliness = selectedHilliness;
                            Find.WorldPathGrid.RecalculatePerceivedMovementDifficultyAt(tileID);
                            worldEditor.WorldUpdater.RenderSingleHill(tileID, hillinessSubMeshes);
                        }
                    }

                    if (customRocksTmp != null)
                    {
                        customRockData.Rocks = new List<ThingDef>(customRocksTmp);
                        if (!customNaturalRocks.ResourceData.Keys.Contains(tileID))
                        {
                            customNaturalRocks.ResourceData.Add(tileID, customRockData);
                        }
                    }

                    tile.temperature = temperature;
                    tile.elevation = elevation;
                    tile.rainfall = rainfall;
                    tile.swampiness = swampiness;
                }
            }
        }

        private void RadiusTile()
        {
            int tileID = GenWorld.MouseTile();

            List<int> radiusTiles = new List<int>();
            Find.WorldFloodFiller.FloodFill(tileID, (int tile) => Find.WorldGrid.InBounds(tile), delegate (int tile, int dist)
            {
                if (dist > brushRadius.max)
                    return true;

                if (dist >= brushRadius.min)
                {
                    radiusTiles.Add(tile);
                }

                return false;
            });

            foreach(var tile in radiusTiles)
            {
                GetCustomRocksFor(tile);

                SingleTile(tile);
            }
        }

        private void UpdateTileInfo(int tileId)
        {
            Tile tile = Find.WorldGrid[tileId];

            temperatureTmpField = tile.temperature.ToString();
            swampinessTmpField = tile.swampiness.ToString();
            rainfallTmpField = tile.rainfall.ToString();
            elevationTmpField = tile.elevation.ToString();

            temperature = tile.temperature;
            swampiness = tile.swampiness;
            rainfall = tile.rainfall;
            elevation = tile.elevation;

            GetCustomRocksFor(tileId);

            customRocksTmp = new List<ThingDef>(customRockData.Rocks);
        }

        private void GetCustomRocksFor(int tileId)
        {
            if (customNaturalRocks.ResourceData.ContainsKey(tileId))
            {
                customRockData = customNaturalRocks.ResourceData[tileId];
            }
            else
            {
                customRockData = new CustomRock(tileId, Find.World.NaturalRockTypesIn(tileId).ToList(), Find.World.HasCaves(tileId));
            }
        }

        private string GetString(SetType setType)
        {
            switch(setType)
            {
                case SetType.Temperature:
                    return "SetType_Temperature".Translate();
                case SetType.Elevation:
                    return "SetType_Elevation".Translate();
                case SetType.Rainfall:
                    return "SetType_Rainfall".Translate();
                case SetType.Swampiness:
                    return "SetType_Swampiness".Translate();
                default:
                    {
                        Log.Error($"Invalid {setType} type");
                        return "";
                    }
            }
        }

        private void SetToAllMap(SetType type)
        {
            WorldGrid grid = Find.WorldGrid;

            switch (type)
            {
                case SetType.Temperature:
                    {
                        grid.tiles.Where(tile => tile.biome != BiomeDefOf.Ocean && tile.biome != BiomeDefOf.Lake).ToList().ForEach(tile => tile.temperature = temperature);

                        Messages.Message("SetToAllMap_Message".Translate(GetString(type), temperature), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Elevation:
                    {
                        grid.tiles.Where(tile => tile.biome != BiomeDefOf.Ocean && tile.biome != BiomeDefOf.Lake).ToList().ForEach(tile => tile.elevation = elevation);

                        Messages.Message("SetToAllMap_Message".Translate(GetString(type), elevation), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Rainfall:
                    {
                        grid.tiles.Where(tile => tile.biome != BiomeDefOf.Ocean && tile.biome != BiomeDefOf.Lake).ToList().ForEach(tile => tile.rainfall = rainfall);

                        Messages.Message("SetToAllMap_Message".Translate(GetString(type), rainfall), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Swampiness:
                    {
                        grid.tiles.Where(tile => tile.biome != BiomeDefOf.Ocean && tile.biome != BiomeDefOf.Lake).ToList().ForEach(tile => tile.swampiness = swampiness);

                        Messages.Message("SetToAllMap_Message".Translate(GetString(type), swampiness), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
            }
        }

        private void SetToAllBiomes(SetType type)
        {
            if (selectedBiome == null)
            {
                Messages.Message("SetToAllBiomes_InvalidBiomeMessage".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            WorldGrid grid = Find.WorldGrid;

            switch (type)
            {
                case SetType.Temperature:
                    {
                        grid.tiles.Where(tile => tile.biome == selectedBiome).ToList().ForEach(tile => tile.temperature = temperature);

                        Messages.Message("SetToAllBiomes_Message".Translate(GetString(type), temperature, selectedBiome.defName), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Elevation:
                    {
                        grid.tiles.Where(tile => tile.biome == selectedBiome).ToList().ForEach(tile => tile.elevation = elevation);

                        Messages.Message("SetToAllBiomes_Message".Translate(GetString(type), elevation, selectedBiome.defName), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Rainfall:
                    {
                        grid.tiles.Where(tile => tile.biome == selectedBiome).ToList().ForEach(tile => tile.rainfall = rainfall);

                        Messages.Message("SetToAllBiomes_Message".Translate(GetString(type), rainfall, selectedBiome.defName), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
                case SetType.Swampiness:
                    {
                        grid.tiles.Where(tile => tile.biome == selectedBiome).ToList().ForEach(tile => tile.swampiness = swampiness);

                        Messages.Message("SetToAllBiomes_Message".Translate(GetString(type), swampiness, selectedBiome.defName), MessageTypeDefOf.NeutralEvent, false);

                        break;
                    }
            }
        }
    }
}
