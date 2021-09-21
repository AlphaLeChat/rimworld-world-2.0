using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public class WorldTemplateGrid : IExposable
    {
		public int tilesCount;

		public WorldTemplateGridElement tileBiome;

		public WorldTemplateGridElement tileElevation;

		public WorldTemplateGridElement tileHilliness;

		public WorldTemplateGridElement tileTemperature;

		public WorldTemplateGridElement tileRainfall;

		public WorldTemplateGridElement tileSwampiness;

		public WorldTemplateFeatureGridElement tileFeature;

		public WorldTemplateGridElement tileRoadOrigins;

		public WorldTemplateGridElement tileRoadAdjacency;

		public WorldTemplateGridElement tileRoadDef;

		public WorldTemplateGridElement tileRiverOrigins;

		public WorldTemplateGridElement tileRiverAdjacency;

		public WorldTemplateGridElement tileRiverDef;

		public void ExposeData()
		{
			Scribe_Values.Look(ref tilesCount, "tilesCount");

			Scribe_Deep.Look(ref tileBiome, "tileBiome");
			Scribe_Deep.Look(ref tileElevation, "tileElevation");
			Scribe_Deep.Look(ref tileHilliness, "tileHilliness");
			Scribe_Deep.Look(ref tileTemperature, "tileTemperature");
			Scribe_Deep.Look(ref tileRainfall, "tileRainfall");
			Scribe_Deep.Look(ref tileSwampiness, "tileSwampiness");
			Scribe_Deep.Look(ref tileFeature, "tileFeature");
			Scribe_Deep.Look(ref tileRoadOrigins, "tileRoadOrigins");
			Scribe_Deep.Look(ref tileRoadAdjacency, "tileRoadAdjacency");
			Scribe_Deep.Look(ref tileRoadDef, "tileRoadDef");
			Scribe_Deep.Look(ref tileRiverOrigins, "tileRiverOrigins");
			Scribe_Deep.Look(ref tileRiverAdjacency, "tileRiverAdjacency");
			Scribe_Deep.Look(ref tileRiverDef, "tileRiverDef");
		}
	}
}
