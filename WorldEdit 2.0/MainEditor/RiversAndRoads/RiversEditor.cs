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
    public class RiversEditor : Editor
    {
        protected override Type WindowType => typeof(RiversEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F2;

        public override string EditorName => "WE_Settings_RiversEditorKey".Translate();

        public List<RiverDef> AvaliableRivers { get; private set; }

        public RiversEditor()
        {
            AvaliableRivers = DefDatabase<RiverDef>.AllDefsListForReading;
        }
    }
}
