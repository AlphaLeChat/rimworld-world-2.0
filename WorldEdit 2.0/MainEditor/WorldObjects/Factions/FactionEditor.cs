using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Factions
{
    public class FactionEditor : Editor
    {
        protected override Type WindowType => typeof(FactionEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F5;

        public override string EditorName => "WE_Settings_FactionEditorKey".Translate();

        public FactionEditor()
        {
        }
    }
}
