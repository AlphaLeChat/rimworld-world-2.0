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
using WorldEdit_2_0.MainEditor.Tiles;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0
{
    [StaticConstructorOnStartup]
    public class WorldEdit : Mod
    {
        private static Harmony HarmonyInstance;

        public static SettingsManager Settings { get; private set; }

        private static List<Type> registeredEditors = new List<Type>();
        public static IEnumerable<Type> RegisteredEditors => registeredEditors.AsEnumerable();

        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public WorldEdit(ModContentPack content) : base(content)
        {
            HarmonyInstance = new Harmony("net.funkyshit.worldedit_2_0");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            RegisterEditor(typeof(TileEditor));

            worldEditor.InitEditor();

            Settings = GetSettings<SettingsManager>();
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
