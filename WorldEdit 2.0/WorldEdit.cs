using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.RiversAndRoads;
using WorldEdit_2_0.MainEditor.Templates;
using WorldEdit_2_0.MainEditor.Tiles;
using WorldEdit_2_0.MainEditor.WorldFeatures;
using WorldEdit_2_0.MainEditor.WorldObjects.Factions;
using WorldEdit_2_0.MainEditor.WorldObjects.Settlements;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0
{
    [StaticConstructorOnStartup]
    public class WorldEdit : Mod
    {
        private static Harmony harmonyInstance;

        public static SettingsManager Settings { get; private set; }

        private static List<Type> registeredEditors = new List<Type>();
        public static IEnumerable<Type> RegisteredEditors => registeredEditors.AsEnumerable();

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        private static bool edbLoaded = false;
        public static bool EdbLoaded => edbLoaded;

        public WorldEdit(ModContentPack content) : base(content)
        {
            harmonyInstance = new Harmony("net.funkyshit.worldedit_2_0");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            RegisterEditors();

            worldEditor.InitEditor();

            Settings = GetSettings<SettingsManager>();

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            //EdbPatch();
        }

        //private void EdbPatch()
        //{
        //    Type type = AccessTools.TypeByName("EdB.PrepareCarefully.Page_PrepareCarefully");
        //    if (type != null)
        //    {
        //        if (harmonyInstance.Patch(type.GetMethod("ShowStartConfirmation"), prefix: new HarmonyMethod(typeof(EdbPatch).GetMethod("Prefix"))) == null)
        //        {
        //            Log.Warning("Error while patching EDB.", false);
        //        }
        //        else
        //        {
        //            edbLoaded = true;
        //            Log.Message("Edb successfully patched", false);
        //        }
        //    }
        //    else
        //    {
        //        Log.Warning("Prepare Carefully not found...skip", false);
        //    }
        //}

        private void RegisterEditors()
        {
            RegisterEditor(typeof(TileEditor));
            RegisterEditor(typeof(TemplateEditor));
            RegisterEditor(typeof(RiversEditor));
            RegisterEditor(typeof(RoadsEditor));
            RegisterEditor(typeof(WorldFeatureEditor));
            RegisterEditor(typeof(FactionEditor));
            RegisterEditor(typeof(SettlementEditor));
        }

        public override string SettingsCategory()
        {
            return "WorldEdit 2.0";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContent(inRect);
        }

        public static void RegisterEditor(Type editorType)
        {
            if(!registeredEditors.Contains(editorType))
            {
                registeredEditors.Add(editorType);
            }
        }
        public static void UnRegisterEditor(Type editorType)
        {
            if (registeredEditors.Contains(editorType))
            {
                registeredEditors.Remove(editorType);
            }
        }
    }
}
