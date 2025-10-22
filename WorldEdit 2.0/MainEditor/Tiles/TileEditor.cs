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
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.Tiles
{
    public class TileEditor : Editor
    {

        protected override Type WindowType => typeof(TileEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F1;

        public override string EditorName => "WE_Settings_TileEditorKey".Translate();

        public List<BiomeDef> AvaliableBiomes { get; private set; }

        public Dictionary<string, WorldDrawLayerBase> Layers { get; private set; }
        public Dictionary<string, List<LayerSubMesh>> LayersSubMeshes { get; private set; }

        public TileEditor()
        {
            AvaliableBiomes = DefDatabase<BiomeDef>.AllDefsListForReading;
        }

        public override void WorldFinalizeInit()
        {
            FieldInfo fieldMeshes = typeof(WorldDrawLayerBase).GetField("subMeshes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static);

            List<WorldDrawLayerBase> tempLayers = new List<WorldDrawLayerBase>(Find.World.renderer.AllDrawLayers);
            //var tempLayers = fieldlayers.GetValue(Find.World.renderer) as List<WorldLayer>;
            Layers = new Dictionary<string, WorldDrawLayerBase>(tempLayers.Count);
            LayersSubMeshes = new Dictionary<string, List<LayerSubMesh>>(tempLayers.Count);
            var layerToSkip = new List<Type> { typeof(WorldDrawLayer_DebugNoise) };
            foreach (var layer in tempLayers)
            {
                if (layerToSkip.Contains(layer.GetType()))
                    continue;
                if (!Layers.ContainsKey(layer.GetType().Name))
                {
                    Layers.Add(layer.GetType().Name, layer);
                    List<LayerSubMesh> meshes = fieldMeshes.GetValue(layer) as List<LayerSubMesh>;
                    LayersSubMeshes.Add(layer.GetType().Name, meshes);
                }
            }
        }

        public void SetBiomeToWholeMap(BiomeDef biome, bool oceanAlso)
        {
            if (biome == null)
            {
                Messages.Message("SetToAllBiomes_InvalidBiomeMessage".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            WorldGrid grid = Find.WorldGrid;
            if (oceanAlso)
            {
                foreach (var tile in grid.Tiles)
                {
                    tile.PrimaryBiome = biome;
                }
                
            }
            else
            {
                foreach (var tile in grid.Tiles)
                {
                    if (tile.Biomes.All(b => b != BiomeDefOf.Ocean && b != BiomeDefOf.Lake))
                    {
                        tile.PrimaryBiome = biome;
                    }
       
                }
                
         
            }

            LongEventHandler.QueueLongEvent(delegate
            {
                WorldUpdater worldUpdater = WorldEditor.WorldEditorInstance.WorldUpdater;

                foreach (var layer in Layers)
                {
                    LayersSubMeshes[layer.Key].Clear();

                    WorldEditor.WorldEditorInstance.WorldUpdater.UpdateLayer(layer.Value);
                }
            }, "Updating...", doAsynchronously: false, null);
        }
    }
}
