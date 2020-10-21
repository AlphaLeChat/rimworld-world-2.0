using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0.Patches.WorldPatches
{
    /// <summary>
    /// Базовая инициализация редактора
    /// </summary>
    [HarmonyPatch("WorldUpdate"), HarmonyPatch(typeof(World))]
    class WE_World_WorldUpdate_Patch
    {
        private static SettingsManager settings => WorldEdit.Settings;

        private static WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public static void Postfix()
        {
            worldEditor.WorldEditorUpdate();
        }
    }
}
