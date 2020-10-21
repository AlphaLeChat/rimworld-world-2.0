using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0;
using WorldEdit_2_0.MainEditor;

namespace test
{
    public class TestMod : Mod
    {
        public TestMod(ModContentPack content) : base(content)
        {
            WorldEdit.RegisterEditor(typeof(TestEditor)); //register editor

            WorldEditor.WorldEditorInstance.CheckMissingEditors(); //call for reload editors
        }
    }
}
