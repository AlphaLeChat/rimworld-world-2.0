using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.Templates
{
    public class TemplateEditor : Editor
    {
        protected override Type WindowType => typeof(TemplateEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F11;

        public override string EditorName => "WE_Settings_TemplateEditorKey".Translate();
    }
}
