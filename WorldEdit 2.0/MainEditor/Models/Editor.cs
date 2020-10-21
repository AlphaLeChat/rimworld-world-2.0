using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public abstract class Editor : IExposable
    {
        protected Window EditorWindow;

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
            if (EditorWindow == null)
            {
                EditorWindow = (Window)Activator.CreateInstance(WindowType, this);
            }

            Find.WindowStack.Add(EditorWindow);
        }

        public void SetKey(KeyCode keyCode)
        {
            callKeyCode = keyCode;
        }

        public virtual void WorldFinalizeInit()
        {

        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref callKeyCode, "callKeyCode");
        }
    }
}
