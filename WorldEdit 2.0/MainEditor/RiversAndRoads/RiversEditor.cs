using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.Tiles;

namespace WorldEdit_2_0.MainEditor.RiversAndRoads
{
    public class RiversEditor : Editor
    {
        protected override Type WindowType => typeof(RiversEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F2;

        public override string EditorName => "WE_Settings_RiversEditorKey".Translate();

        public List<RiverDef> AvaliableRivers { get; private set; }

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public WorldLayer RiversLayer { get; private set; }
        public List<LayerSubMesh> SingleTileSubMesh { get; private set; }

        public RiversEditor()
        {
            AvaliableRivers = DefDatabase<RiverDef>.AllDefsListForReading;
        }

        public override void WorldFinalizeInit()
        {
            base.WorldFinalizeInit();

            RiversLayer = WorldEditor.WorldEditorInstance.GetEditor<TileEditor>().Layers["WorldLayer_Rivers"];
            SingleTileSubMesh = WorldEditor.WorldEditorInstance.GetEditor<TileEditor>().LayersSubMeshes["WorldLayer_SelectedTile"];
        }

        public List<int> FindOceansOrLakesAround(int rootTile)
        {
            if (rootTile < 0)
            {
                Messages.Message($"RiversEditorWindow_CreateSource_NoTile".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return null;
            }

            if (Find.WorldGrid[rootTile].biome == BiomeDefOf.Ocean || Find.WorldGrid[rootTile].biome == BiomeDefOf.Lake)
            {
                Messages.Message($"RiversEditorWindow_CreateSource_WaterTile".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return null;
            }

            List<int> outList = new List<int>();
            Find.WorldGrid.GetTileNeighbors(rootTile, outList);

            List<int> oceansOrLakes = outList.Where(id => Find.WorldGrid[id].biome == BiomeDefOf.Ocean || Find.WorldGrid[id].biome == BiomeDefOf.Lake).ToList();
            if (oceansOrLakes.Count == 0)
            {
                Messages.Message($"RiversEditorWindow_CreateSource_NoOceansOrLakes".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return null;
            }

            return oceansOrLakes;
        }

        public void RemoveAllRivers()
        {
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                Tile tile = Find.WorldGrid[i];

                tile.potentialRivers = null;
            }

            worldEditor.WorldUpdater.UpdateLayer(RiversLayer);

            Messages.Message($"RiversEditorWindow_RemoveAllRiversInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        public void CreateRiver(int tile1ID, int tile2ID, RiverDef river)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            var path = Find.WorldPathFinder.FindPath(tile1ID, tile2ID, null);

            for (int i = 0; i < path.NodesLeftCount - 1; i++)
            {
                worldGrid.OverlayRiver(path.Peek(i), path.Peek(i + 1), river);
            }

            worldEditor.WorldUpdater.UpdateLayer(RiversLayer);
        }

        public void RemoveRiver(int tile1ID, int tile2ID)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            var path = Find.WorldPathFinder.FindPath(tile1ID, tile2ID, null);
            for (int i = 0; i < path.NodesLeftCount - 1; i++)
            {
                Tile tile = worldGrid[path.Peek(i)];
                tile.potentialRivers = null;
            }

            worldGrid[tile2ID].potentialRivers = null;

            worldEditor.WorldUpdater.UpdateLayer(RiversLayer);
        }
    }
}
