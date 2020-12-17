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

namespace WorldEdit_2_0.MainEditor.WorldFeatures
{
    public class WorldFeatureEditor : Editor
    {
        protected override Type WindowType => typeof(WorldFeatureEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F4;

        public override string EditorName => "WE_Settings_WorldFeatureEditorKey".Translate();

        public void DeleteAllFeatures()
        {
            WorldGrid grid = Find.WorldGrid;

            foreach (var feature in Find.WorldFeatures.features)
            {
                foreach (var tileID in feature.Tiles)
                {
                    grid[tileID].feature = null;
                }
            }

            Find.WorldFeatures.features.Clear();

            Find.WorldFeatures.textsCreated = false;

            Find.WorldFeatures.UpdateFeatures();
        }

        public void DeleteFeature(WorldFeature worldFeature)
        {
            if (worldFeature == null)
                return;

            WorldGrid grid = Find.WorldGrid;
            foreach (var t in worldFeature.Tiles)
            {
                if (grid[t].feature == worldFeature)
                    grid[t].feature = null;
            }

            Find.WorldFeatures.features.Remove(worldFeature);

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();
        }

        public WorldFeature CreateNewFeature(int tile)
        {
            return CreateNewFeature(tile);
        }

        public WorldFeature CreateNewFeature(int tile, string featureName = "New Feature")
        {
            return CreateNewFeature(tile, featureName);
        }

        public WorldFeature CreateNewFeature(int tile, string featureName, float drawSize = 10f, float drawAngle = 0f)
        {
            WorldFeature worldFeature = new WorldFeature
            {
                uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID(),
                def = DefDatabase<FeatureDef>.GetRandom(),
                name = featureName
            };
            WorldGrid worldGrid = Find.WorldGrid;
            worldGrid[tile].feature = worldFeature;

            worldFeature.drawCenter = worldGrid.GetTileCenter(tile);
            worldFeature.maxDrawSizeInTiles = drawSize;
            worldFeature.drawAngle = drawAngle;

            Find.WorldFeatures.features.Add(worldFeature);

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();

            return worldFeature;
        }
    }
}
