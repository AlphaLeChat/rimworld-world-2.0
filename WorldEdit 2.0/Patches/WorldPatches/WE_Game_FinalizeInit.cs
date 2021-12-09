using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.Defs;
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

                if (template != null)
                {
                    if (template.IsValidTemplate && GameComponent_WorldEditTemplate.WorldTemplateDef == null)
                    {
                        Find.WindowStack.Add(new Page_CustomStartingSite());
                    } else if (GameComponent_WorldEditTemplate.WorldTemplateDef != null)
                    {
                        Current.Game.Scenario = GameComponent_WorldEditTemplate.SelectedScenario;

                        foreach (var scenPart in Find.Scenario.AllParts)
                        {
                            ScenPart_ConfigPage_ConfigureStartingPawns part = scenPart as ScenPart_ConfigPage_ConfigureStartingPawns;
                            if (part != null)
                            {
                                part.PostIdeoChosen();
                                break;
                            }
                        }

                        List<Page> list = new List<Page>();
                        Page_SelectStoryteller page_SelectStoryteller = new Page_SelectStoryteller();
                        list.Add(page_SelectStoryteller);

                        Page_CustomStartingSite page_CustomStartingSite = new Page_CustomStartingSite();
                        list.Add(page_CustomStartingSite);
                        if (ModsConfig.IdeologyActive)
                        {
                            list.Add(new Page_ChooseIdeoPreset());
                        }
                        foreach (Page item in Current.Game.Scenario.AllParts.SelectMany((ScenPart p) => p.GetConfigPages()))
                        {
                            list.Add(item);
                        }
                        Page page = PageUtility.StitchedPages(list);
                        if (page != null)
                        {
                            Page page2 = page;
                            while (page2.next != null)
                            {
                                page2 = page2.next;
                            }
                            page2.nextAct = delegate
                            {
                                PageUtility.InitGameStart();
                            };
                        }

                        Find.GameInitData.permadeathChosen = true;

                        //Page_ConfigureStartingPawns page_ConfigureStartingPawns = list.Find(x => x.next is Page_ConfigureStartingPawns).next as Page_ConfigureStartingPawns;
                        //page_ConfigureStartingPawns.prev = page_CustomStartingSite;

                        Find.WindowStack.Add(page);

                       // Find.WindowStack.Add(new Page_CustomStartingSite());
                    }
				}
            }

            return true;
        }
    }
}
