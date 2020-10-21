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

        public Dictionary<string, WorldLayer> Layers { get; private set; }
        public Dictionary<string, List<LayerSubMesh>> LayersSubMeshes { get; private set; }

        public TileEditor()
        {
            AvaliableBiomes = DefDatabase<BiomeDef>.AllDefsListForReading;
        }

        public override void WorldFinalizeInit()
        {
            FieldInfo fieldlayers = typeof(WorldRenderer).GetField("layers", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo fieldMeshes = typeof(WorldLayer).GetField("subMeshes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static);

            var tempLayers = fieldlayers.GetValue(Find.World.renderer) as List<WorldLayer>;
            Layers = new Dictionary<string, WorldLayer>(tempLayers.Count);
            LayersSubMeshes = new Dictionary<string, List<LayerSubMesh>>(tempLayers.Count);
            foreach (var layer in tempLayers)
            {
                Layers.Add(layer.GetType().Name, layer);
                List<LayerSubMesh> meshes = fieldMeshes.GetValue(layer) as List<LayerSubMesh>;
                LayersSubMeshes.Add(layer.GetType().Name, meshes);
            }
        }
    }
}
