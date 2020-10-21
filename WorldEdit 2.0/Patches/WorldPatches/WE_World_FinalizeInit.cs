using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0.Patches.WorldPatches
{
    [HarmonyPatch("FinalizeInit"), HarmonyPatch(typeof(World))]
    class WE_World_FinalizeInit
    {
        private static SettingsManager settings => WorldEdit.Settings;

        private static WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public static void Postfix()
        {
            if (settings.ActiveEditor)
            {
                if (worldEditor.IsInit)
                {
                    worldEditor.WorldFinalizeInit();
                }
            }
        }
    }
}
