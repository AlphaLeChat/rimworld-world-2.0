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
    public class RoadsEditor : Editor
    {
        protected override Type WindowType => typeof(RoadsEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F3;

        public override string EditorName => "WE_Settings_RoadsEditorKey".Translate();

        public List<RoadDef> AvaliableRoads { get; private set; }

        public WorldLayer RoadsLayer { get; private set; }

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public RoadsEditor()
        {
            AvaliableRoads = DefDatabase<RoadDef>.AllDefsListForReading;
        }

        public override void WorldFinalizeInit()
        {
            RoadsLayer = WorldEditor.WorldEditorInstance.GetEditor<TileEditor>().Layers["WorldLayer_Roads"];
        }

        public void RemoveAllRoads()
        {
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                Tile tile = Find.WorldGrid[i];

                tile.potentialRoads = null;
            }

            worldEditor.WorldUpdater.UpdateLayer(RoadsLayer);

            Messages.Message($"RoadsEditorWindow_RemoveAllRoadsInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        public void CreateRoad(int tile1ID, int tile2ID, RoadDef road)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            var path = Find.WorldPathFinder.FindPath(tile1ID, tile2ID, null);

            for (int i = 0; i < path.NodesLeftCount - 1; i++)
            {
                worldGrid.OverlayRoad(path.Peek(i), path.Peek(i + 1), road);
            }

            worldEditor.WorldUpdater.UpdateLayer(RoadsLayer);
        }

        public void RemoveRoad(int tile1ID, int tile2ID)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            var path = Find.WorldPathFinder.FindPath(tile1ID, tile2ID, null);
            for (int i = 0; i < path.NodesLeftCount - 1; i++)
            {
                Tile tile = worldGrid[path.Peek(i)];
                tile.potentialRoads = null;
            }

            worldGrid[tile2ID].potentialRoads = null;

            worldEditor.WorldUpdater.UpdateLayer(RoadsLayer);
        }
    }
}
