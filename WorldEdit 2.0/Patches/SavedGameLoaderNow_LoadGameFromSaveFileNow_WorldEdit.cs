using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace WorldEdit_2_0.Patches
{
    [HarmonyPatch(typeof(SavedGameLoaderNow))]
    [HarmonyPatch(nameof(SavedGameLoaderNow.LoadGameFromSaveFileNow))]
    [HarmonyPatch(new[] { typeof(string) })]
    public static class SavedGameLoaderNow_LoadGameFromSaveFileNow_WorldEdit
    {
        public static XmlDocument DocumentToLoad;

        static bool Prefix()
        {
            if (DocumentToLoad == null)
                return true;

            ScribeMetaHeaderUtility.loadedGameVersion = VersionControl.CurrentVersionStringWithRev;
            Scribe.loader.curXmlParent = DocumentToLoad.DocumentElement;
            Scribe.mode = LoadSaveMode.LoadingVars;

            ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, false);
            Scribe.EnterNode("game");

            Current.Game.LoadGame();

            Current.Game.InitData.gameToLoad = null;
            DocumentToLoad = null;

            return false;
        }
    }
}
