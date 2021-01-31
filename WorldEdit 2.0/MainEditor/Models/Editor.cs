using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.Settings;

namespace WorldEdit_2_0.MainEditor.Models
{
    public abstract class Editor : IExposable
    {
        protected Window editorWindow;

        protected abstract Type WindowType { get; }

        private KeyCode callKeyCode;
        public KeyCode CallKeyCode => callKeyCode;

        protected abstract KeyCode DefaultKeyCode { get; }

        public virtual string EditorName { get; }

        public WorldEditor WorldEditor => WorldEditor.WorldEditorInstance;

        public Editor()
        {
            callKeyCode = DefaultKeyCode;
        }

        public virtual void ShowEditor()
        {
            if (editorWindow == null)
            {
                editorWindow = (Window)Activator.CreateInstance(WindowType, this);
            }

            Find.WindowStack.Add(editorWindow);
        }

        public void SetKey(KeyCode keyCode)
        {
            callKeyCode = keyCode;
        }

        public virtual void WorldFinalizeInit()
        {

        }

        public virtual void CloseEditor()
        {
            if(editorWindow != null)
            {
                editorWindow.Close();
            }
        }

        public T GetEditorWindow<T>() where T : class
        {
            return editorWindow as T;
        }

        public virtual void DrawSettings(Rect inRect, Listing_Standard listing_Standard)
        {
            if (listing_Standard.ButtonText($"{EditorName}: {CallKeyCode}"))
            {
                SettingsManager.SetKeyWithList((code) => SetKey(code));
            }
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref callKeyCode, "callKeyCode");
        }

        public bool IsClickOutsideWindow(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode) && Find.WindowStack.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) == null;
        }

        public bool IsClickOutsideWindow()
        {
            return Find.WindowStack.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) == null;
        }
    }
}
