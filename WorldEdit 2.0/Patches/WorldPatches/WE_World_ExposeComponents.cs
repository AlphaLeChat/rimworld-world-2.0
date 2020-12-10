using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Templates;

namespace WorldEdit_2_0.Patches.WorldPatches
{
    //[HarmonyPatch(typeof(World))]
    //[HarmonyPatch("ExposeComponents")]
    //class WE_World_ExposeComponents
    //{
    //    static void Postfix()
    //    {
    //        if (Current.ProgramState == ProgramState.MapInitializing)
    //        {
    //            GameComponent_WorldEditTemplate template = Current.Game.GetComponent<GameComponent_WorldEditTemplate>();

    //            if (template != null && template.IsValidTemplate)
    //            {
    //                Find.WindowStack.Add(new Page_CustomStartingSite());
    //            }
    //        }
    //    }
    //}
}
