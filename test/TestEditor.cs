using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WorldEdit_2_0.MainEditor.Models;

namespace test
{
    public class TestEditor : Editor
    {
        protected override Type WindowType => typeof(TestEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.Z;

        public override string EditorName => "Test Editor";
    }
}
