using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Templates;

namespace WorldEdit_2_0.Patches.MainMenu
{
    [HarmonyPatch(typeof(Page_SelectScenario))]
    [HarmonyPatch("BeginScenarioConfiguration")]
	[HarmonyPriority(100)]
    public class WE_Scenario_BeginScenarioConfiguration
	{
        static void Postfix(Page_SelectScenario __instance, ref Scenario scen)
        {
            GameComponent_WorldEditTemplate.SelectedScenario = scen;
        }
    }
}
