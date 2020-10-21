using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace test
{
    public class TestEditorWindow : Window
    {
        private TestEditor testEditor;

        public TestEditorWindow(TestEditor testEditor)
        {
            this.testEditor = testEditor;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GUI.color = Color.red;
            Widgets.DrawBox(inRect);
        }
    }
}
