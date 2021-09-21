using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using static RimWorld.Planet.Tile;

namespace WorldEdit_2_0.MainEditor.Templates
{
    public class TemplateEditor : Editor
    {
		public static string TemplateFolder => "Templates";

        protected override Type WindowType => typeof(TemplateEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F11;

        public override string EditorName => "WE_Settings_TemplateEditorKey".Translate();

        public static WorldTemplateDef GenerateTemplateFromCurrentWorld(string defName, string name, string author, string description)
        {
            WorldTemplateDef worldTemplateDef = new WorldTemplateDef();

            worldTemplateDef.defName = defName;
            worldTemplateDef.label = name;
            worldTemplateDef.description = description;
            worldTemplateDef.author = author;

            //World world = Find.World;
            //int tilesCount = world.grid.TilesCount;

            //WorldTemplateDef worldTemplateDef = new WorldTemplateDef();

            //worldTemplateDef.defName = defName;
            //worldTemplateDef.label = name;
            //worldTemplateDef.description = description;
            //worldTemplateDef.author = author;
            //worldTemplateDef.planetCoverage = world.info.planetCoverage;

            //WorldTemplateGrid grid = new WorldTemplateGrid();
            //grid.tilesCount = tilesCount;

            //grid.tileBiome = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(tilesCount, (int i) => world.grid.tiles[i].biome.shortHash));
            //grid.tileElevation = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(tilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((world.grid.tiles[i].WaterCovered ? world.grid.tiles[i].elevation : Mathf.Max(world.grid.tiles[i].elevation, 1f)) + 8192f), 0, 65535)));
            //grid.tileHilliness = new WorldTemplateGridElement(DataSerializeUtility.SerializeByte(tilesCount, (int i) => (byte)world.grid.tiles[i].hilliness));
            //grid.tileTemperature = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(tilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((world.grid.tiles[i].temperature + 300f) * 10f), 0, 65535)));
            //grid.tileRainfall = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(tilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(world.grid.tiles[i].rainfall), 0, 65535)));
            //grid.tileSwampiness = new WorldTemplateGridElement(DataSerializeUtility.SerializeByte(tilesCount, (int i) => (byte)Mathf.Clamp(Mathf.RoundToInt(world.grid.tiles[i].swampiness * 255f), 0, 255)));
            //grid.tileFeature = new WorldTemplateFeatureGridElement(DataSerializeUtility.SerializeUshort(tilesCount, (int i) => (world.grid.tiles[i].feature != null) ? ((ushort)world.grid.tiles[i].feature.uniqueID) : ushort.MaxValue), world.features.features);
            //List<int> list = new List<int>();
            //List<byte> list2 = new List<byte>();
            //List<ushort> list3 = new List<ushort>();
            //for (int j = 0; j < tilesCount; j++)
            //{
            //	List<Tile.RoadLink> potentialRoads = world.grid.tiles[j].potentialRoads;
            //	if (potentialRoads == null)
            //	{
            //		continue;
            //	}
            //	for (int k = 0; k < potentialRoads.Count; k++)
            //	{
            //		Tile.RoadLink roadLink = potentialRoads[k];
            //		if (roadLink.neighbor >= j)
            //		{
            //			byte b = (byte)world.grid.GetNeighborId(j, roadLink.neighbor);
            //			if (b < 0)
            //			{
            //				Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014);
            //				continue;
            //			}
            //			list.Add(j);
            //			list2.Add(b);
            //			list3.Add(roadLink.road.shortHash);
            //		}
            //	}
            //}
            //grid.tileRoadOrigins = new WorldTemplateGridElement(DataSerializeUtility.SerializeInt(list.ToArray()));
            //grid.tileRoadAdjacency = new WorldTemplateGridElement(DataSerializeUtility.SerializeByte(list2.ToArray()));
            //grid.tileRoadDef = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(list3.ToArray()));
            //List<int> list4 = new List<int>();
            //List<byte> list5 = new List<byte>();
            //List<ushort> list6 = new List<ushort>();
            //for (int l = 0; l < tilesCount; l++)
            //{
            //	List<Tile.RiverLink> potentialRivers = world.grid.tiles[l].potentialRivers;
            //	if (potentialRivers == null)
            //	{
            //		continue;
            //	}
            //	for (int m = 0; m < potentialRivers.Count; m++)
            //	{
            //		Tile.RiverLink riverLink = potentialRivers[m];
            //		if (riverLink.neighbor >= l)
            //		{
            //			byte b2 = (byte)world.grid.GetNeighborId(l, riverLink.neighbor);
            //			if (b2 < 0)
            //			{
            //				Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014);
            //				continue;
            //			}
            //			list4.Add(l);
            //			list5.Add(b2);
            //			list6.Add(riverLink.river.shortHash);
            //		}
            //	}
            //}
            //grid.tileRiverOrigins = new WorldTemplateGridElement(DataSerializeUtility.SerializeInt(list4.ToArray()));
            //grid.tileRiverAdjacency = new WorldTemplateGridElement(DataSerializeUtility.SerializeByte(list5.ToArray()));
            //grid.tileRiverDef = new WorldTemplateGridElement(DataSerializeUtility.SerializeUshort(list6.ToArray()));

            //worldTemplateDef.grid = grid;
            //worldTemplateDef.factions = new WorldTemplateFactions() { 
            //	allFactions = world.factionManager.AllFactions.ToList() ,
            //	worldObjects = world.worldObjects.AllWorldObjects
            //};

            return worldTemplateDef;
		}
    }
}
