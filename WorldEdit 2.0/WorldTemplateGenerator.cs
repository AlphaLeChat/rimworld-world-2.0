using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.Defs;

namespace WorldEdit_2_0
{
	/*
    public static class WorldTemplateGenerator
    {
        public static World GenerateTemplate(WorldTemplateDef worldTemplateDef)
        {
            Rand.PushState();
            string seedString = GenText.RandomSeedString();
            int seed = (Rand.Seed = GenText.StableStringHash(seedString));
            try
            {
                Current.CreatingWorld = new World();
                Current.CreatingWorld.info.seedString = seedString;
                Current.CreatingWorld.info.planetCoverage = worldTemplateDef.planetCoverage;
                Current.CreatingWorld.info.overallRainfall = OverallRainfall.Normal;
                Current.CreatingWorld.info.overallTemperature = OverallTemperature.Normal;
                Current.CreatingWorld.info.overallPopulation = OverallPopulation.Normal;
                Current.CreatingWorld.info.name = worldTemplateDef.label;
                Current.CreatingWorld.info.factionCounts = null;

                Rand.Seed = seed;

				TemplateRawDataToTiles(seedString, Current.CreatingWorld, worldTemplateDef);

				Current.CreatingWorld.grid.StandardizeTileData();
                Current.CreatingWorld.FinalizeInit();
                Find.Scenario.PostWorldGenerate();
                if (!ModsConfig.IdeologyActive)
                {
                    Find.Scenario.PostIdeoChosen();
                }

				return Current.CreatingWorld;
            }
            finally
            {
                Rand.PopState();
                Current.CreatingWorld = null;
            }
        }

		private static void TemplateRawDataToTiles(string seedString, World world, WorldTemplateDef worldTemplateDef)
		{
			world.grid = new WorldGrid();
			world.pathGrid = new WorldPathGrid();

			GenerateTerrain(world, worldTemplateDef);

			Current.CreatingWorld.ConstructComponents();

			GenerateRivers(world, worldTemplateDef);

			if (worldTemplateDef.factions == null)
			{
				WorldGenStepDefOf.Factions.worldGenStep.GenerateFresh(seedString);
            }
            else
            {
				foreach(Faction fac in worldTemplateDef.factions.allFactions)
                {
					world.factionManager.Add(fac);
                }

				foreach(WorldObject worldObject in worldTemplateDef.factions.worldObjects)
                {
					Find.WorldObjects.Add(worldObject);
                }

			}

			GenerateRoads(world, worldTemplateDef);

			Current.CreatingWorld.features = new WorldFeatures();
			Current.CreatingWorld.features.features = new List<WorldFeature>(worldTemplateDef.grid.tileFeature.features);
			DataSerializeUtility.LoadUshort(worldTemplateDef.grid.tileFeature.GetBytes(), worldTemplateDef.grid.tilesCount, delegate (int i, ushort data)
			{
				world.grid[i].feature = ((data == ushort.MaxValue) ? null : worldTemplateDef.grid.tileFeature.GetFeatureWithID(data));
			});
		}

		private static void GenerateRoads(World world, WorldTemplateDef worldTemplateDef)
        {
			int[] array = DataSerializeUtility.DeserializeInt(worldTemplateDef.grid.tileRoadOrigins.GetBytes());
			byte[] array2 = DataSerializeUtility.DeserializeByte(worldTemplateDef.grid.tileRoadAdjacency.GetBytes());
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(worldTemplateDef.grid.tileRoadDef.GetBytes());
			for (int l = 0; l < array.Length; l++)
			{
				int num = array[l];
				int tileNeighbor = world.grid.GetTileNeighbor(num, array2[l]);
				RoadDef byShortHash = DefDatabase<RoadDef>.GetByShortHash(array3[l]);
				if (byShortHash != null)
				{
					if (world.grid.tiles[num].potentialRoads == null)
					{
						world.grid.tiles[num].potentialRoads = new List<Tile.RoadLink>();
					}
					if (world.grid.tiles[tileNeighbor].potentialRoads == null)
					{
						world.grid.tiles[tileNeighbor].potentialRoads = new List<Tile.RoadLink>();
					}
					List<Tile.RoadLink> potentialRoads = world.grid.tiles[num].potentialRoads;
					Tile.RoadLink item = new Tile.RoadLink
					{
						neighbor = tileNeighbor,
						road = byShortHash
					};
					potentialRoads.Add(item);
					List<Tile.RoadLink> potentialRoads2 = world.grid.tiles[tileNeighbor].potentialRoads;
					item = new Tile.RoadLink
					{
						neighbor = num,
						road = byShortHash
					};
					potentialRoads2.Add(item);
				}
			}
		}

		private static void GenerateRivers(World world, WorldTemplateDef worldTemplateDef)
        {
			int[] array4 = DataSerializeUtility.DeserializeInt(worldTemplateDef.grid.tileRiverOrigins.GetBytes());
			byte[] array5 = DataSerializeUtility.DeserializeByte(worldTemplateDef.grid.tileRiverAdjacency.GetBytes());
			ushort[] array6 = DataSerializeUtility.DeserializeUshort(worldTemplateDef.grid.tileRiverDef.GetBytes());
			for (int m = 0; m < array4.Length; m++)
			{
				int num2 = array4[m];
				int tileNeighbor2 = world.grid.GetTileNeighbor(num2, array5[m]);
				RiverDef byShortHash2 = DefDatabase<RiverDef>.GetByShortHash(array6[m]);
				if (byShortHash2 != null)
				{
					if (world.grid.tiles[num2].potentialRivers == null)
					{
						world.grid.tiles[num2].potentialRivers = new List<Tile.RiverLink>();
					}
					if (world.grid.tiles[tileNeighbor2].potentialRivers == null)
					{
						world.grid.tiles[tileNeighbor2].potentialRivers = new List<Tile.RiverLink>();
					}
					List<Tile.RiverLink> potentialRivers = world.grid.tiles[num2].potentialRivers;
					Tile.RiverLink item2 = new Tile.RiverLink
					{
						neighbor = tileNeighbor2,
						river = byShortHash2
					};
					potentialRivers.Add(item2);
					List<Tile.RiverLink> potentialRivers2 = world.grid.tiles[tileNeighbor2].potentialRivers;
					item2 = new Tile.RiverLink
					{
						neighbor = num2,
						river = byShortHash2
					};
					potentialRivers2.Add(item2);
				}
			}
		}

		private static void GenerateTerrain(World world, WorldTemplateDef worldTemplateDef)
        {
			if (world.grid.tiles.Count != world.grid.TilesCount)
			{
				world.grid.tiles.Clear();
				for (int j = 0; j < world.grid.TilesCount; j++)
				{
					world.grid.tiles.Add(new Tile());
				}
			}
			else
			{
				for (int k = 0; k < world.grid.TilesCount; k++)
				{
					world.grid.tiles[k].potentialRoads = null;
					world.grid.tiles[k].potentialRivers = null;
				}
			}
			DataSerializeUtility.LoadUshort(worldTemplateDef.grid.tileBiome.GetBytes(), world.grid.TilesCount, delegate (int i, ushort data)
			{
				world.grid.tiles[i].biome = DefDatabase<BiomeDef>.GetByShortHash(data) ?? BiomeDefOf.TemperateForest;
			});
			DataSerializeUtility.LoadUshort(worldTemplateDef.grid.tileElevation.GetBytes(), world.grid.TilesCount, delegate (int i, ushort data)
			{
				world.grid.tiles[i].elevation = data - 8192;
			});
			DataSerializeUtility.LoadByte(worldTemplateDef.grid.tileHilliness.GetBytes(), world.grid.TilesCount, delegate (int i, byte data)
			{
				world.grid.tiles[i].hilliness = (Hilliness)data;
			});
			DataSerializeUtility.LoadUshort(worldTemplateDef.grid.tileTemperature.GetBytes(), world.grid.TilesCount, delegate (int i, ushort data)
			{
				world.grid.tiles[i].temperature = (float)(int)data / 10f - 300f;
			});
			DataSerializeUtility.LoadUshort(worldTemplateDef.grid.tileRainfall.GetBytes(), world.grid.TilesCount, delegate (int i, ushort data)
			{
				world.grid.tiles[i].rainfall = (int)data;
			});
			DataSerializeUtility.LoadByte(worldTemplateDef.grid.tileSwampiness.GetBytes(), world.grid.TilesCount, delegate (int i, byte data)
			{
				world.grid.tiles[i].swampiness = (float)(int)data / 255f;
			});
		}
	}
	*/
}
