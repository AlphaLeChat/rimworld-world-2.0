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
    }
}
