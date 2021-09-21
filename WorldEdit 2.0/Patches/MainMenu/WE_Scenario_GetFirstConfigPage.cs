using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor;

namespace WorldEdit_2_0.Patches.MainMenu
{
    [HarmonyPatch(typeof(Scenario))]
    [HarmonyPatch("GetFirstConfigPage")]
	[HarmonyPriority(100)]
    public class WE_Scenario_GetFirstConfigPage
    {
        static bool Prefix(Scenario __instance, ref Page __result)
        {
			__result = GetFirstConfigPage(__instance);

			return false;
		}

		public static Page GetFirstConfigPage(Scenario scenario)
        {
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectWorldTemplate());
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectStartingSite());
			if (ModsConfig.IdeologyActive)
			{
				list.Add(new Page_ChooseIdeoPreset());
			}
			foreach (Page item in scenario.AllParts.SelectMany((ScenPart p) => p.GetConfigPages()))
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

			return page;
		}
    }
}
