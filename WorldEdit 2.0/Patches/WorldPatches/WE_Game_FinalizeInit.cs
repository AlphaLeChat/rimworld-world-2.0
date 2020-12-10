using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Templates;

namespace WorldEdit_2_0.Patches.WorldPatches
{
    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch("FinalizeInit")]
    class WE_Game_FinalizeInit
    {
        static bool Prefix()
        {
            if (Current.ProgramState == ProgramState.MapInitializing && Find.Maps.Count == 0)
            {
                GameComponent_WorldEditTemplate template = Current.Game.GetComponent<GameComponent_WorldEditTemplate>();

                if (template != null && template.IsValidTemplate)
                {
                    Find.WindowStack.Add(new Page_CustomStartingSite());
                }
            }

            return true;
        }
    }
}
