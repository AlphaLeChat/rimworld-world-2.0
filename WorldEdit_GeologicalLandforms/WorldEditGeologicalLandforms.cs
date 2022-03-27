using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor;

namespace WorldEdit_GeologicalLandforms
{
    public class WorldEditGeologicalLandforms : Mod
    {
        private static Harmony harmonyInstance;

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public WorldEditGeologicalLandforms(ModContentPack content) : base(content)
        {
            harmonyInstance = new Harmony("net.funkyshit.worldedit_2_0");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
