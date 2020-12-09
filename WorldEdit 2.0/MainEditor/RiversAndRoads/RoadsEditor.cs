using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.RiversAndRoads
{
    public class RoadsEditor : Editor
    {
        protected override Type WindowType => typeof(RoadsEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F3;

        public override string EditorName => "WE_Settings_RoadsEditorKey".Translate();

        public List<RoadDef> AvaliableRoads { get; private set; }

        public RoadsEditor()
        {
            AvaliableRoads = DefDatabase<RoadDef>.AllDefsListForReading;
        }
    }
}
