using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Profile;
using WorldEdit_2_0.Defs;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Templates;

namespace WorldEdit_2_0.Patches.MainMenu
{
    [HarmonyPatch(typeof(MainButtonsRoot))]
    [HarmonyPatch("MainButtonsOnGUI")]
    [HarmonyPriority(100)]
    public class WE_MainButtonsRoot_MainButtonsOnGUI
    {
        static bool Prefix(ScenPart_PlayerFaction __instance)
        {
            if (GameComponent_WorldEditTemplate.WorldTemplateDef != null)
                return false;

            return true;
        }
    }
}
