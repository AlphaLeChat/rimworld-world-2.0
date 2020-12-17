using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.Tiles;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0.MainEditor
{
    public sealed class WorldEditor : IExposable
    {
        private static WorldEditor worldEditorInstance;

        public static WorldEditor WorldEditorInstance
        {
            get
            {
                if(worldEditorInstance == null)
                {
                    worldEditorInstance = new WorldEditor();
                }

                return worldEditorInstance;
            }
        }

        private bool isInit;
        public bool IsInit => isInit;

        private SettingsManager settings => WorldEdit.Settings;

        private List<Editor> editors;
        public IEnumerable<Editor> Editors => editors;

        private WorldUpdater worldUpdater;

        public WorldUpdater WorldUpdater => worldUpdater;

        private Editor openedEditor;

        public bool HidePrevOpenedEditor = true;

        private bool activeEditor;
        public bool ActiveEditor => activeEditor;

        public WorldEditor()
        {

        }

        public void WorldEditorUpdate()
        {
            if (!ActiveEditor)
                return;

            if (!IsInit)
                return;

            if (!Input.anyKeyDown)
                return;

            var editor = editors.FirstOrDefault(x => Input.GetKey(x.CallKeyCode));
            if (editor != null)
            {
                editor.ShowEditor();

                if(HidePrevOpenedEditor && editor != openedEditor && openedEditor != null)
                {
                    openedEditor.CloseEditor();
                }

                openedEditor = editor;
            }
        }

        public void InitEditor()
        {
            ReloadEditors();

            isInit = true;
        }

        public void ReloadEditors()
        {
            editors = new List<Editor>();
            foreach (var editor in WorldEdit.RegisteredEditors)
            {
                AddEditor(editor);
            }
        }

        public void WorldFinalizeInit()
        {
            worldUpdater = new WorldUpdater();

            foreach (var editor in editors)
            {
                editor.WorldFinalizeInit();
            }
        }

        public void AddEditor(Type type)
        {
            editors.Add((Editor)Activator.CreateInstance(type));
        }

        public void CheckMissingEditors()
        {
            if(editors == null)
            {
                editors = new List<Editor>();
            }

            if(editors.Count != WorldEdit.RegisteredEditors.Count())
            {
                foreach(var editor in WorldEdit.RegisteredEditors)
                {
                    if(!editors.Any(x => x.GetType().Name == editor.Name))
                    {
                        AddEditor(editor);
                    }
                }
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref HidePrevOpenedEditor, "HidePrevOpenedEditor", true);
            Scribe_Values.Look(ref activeEditor, "activeEditor", true);

            Scribe_Collections.Look(ref editors, "Editors", LookMode.Deep);
             
            editors.RemoveAll(x => x == null);
        }

        public void DrawSettings(Rect inRect, Listing_Standard listing_Standard)
        {
            Rect rect = new Rect(0, listing_Standard.CurHeight, 600, 20);
            TooltipHandler.TipRegion(rect, Translator.Translate("WE_Settings_ActiveEditor_ToolTip"));
            if (listing_Standard.RadioButton(Translator.Translate("WE_Settings_ActiveEditor"), activeEditor))
            {
                activeEditor = !activeEditor;
            }
            if (listing_Standard.RadioButton(Translator.Translate("WE_Settings_HidePrevOpenedEditor"), HidePrevOpenedEditor))
            {
                HidePrevOpenedEditor = !HidePrevOpenedEditor;
            }
            listing_Standard.GapLine();
        }

        public T GetEditor<T>() where T : Editor
        {
            foreach(var editor in editors)
            {
                if(editor is T t)
                {
                    return t;
                }
            }

            return null;
        }
    }
}
