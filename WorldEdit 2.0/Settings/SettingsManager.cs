using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.Settings
{
    public class SettingsManager : ModSettings
    {
        private bool activeEditor;
        public bool ActiveEditor => activeEditor;

        private IEnumerable<Editor> editors => WorldEditor.WorldEditorInstance.Editors;

        public SettingsManager() : base()
        {
            DefaultSettings();
        }

        public void DefaultSettings()
        {
            activeEditor = true;
        }

        public void DoSettingsWindowContent(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);

            listing_Standard.GapLine();
            listing_Standard.Label(Translator.Translate("WE_Settings_General"));

            Rect rect = new Rect(0, listing_Standard.CurHeight, 600, 20);
            TooltipHandler.TipRegion(rect, Translator.Translate("WE_Settings_ActiveEditor_ToolTip"));
            if (listing_Standard.RadioButton(Translator.Translate("WE_Settings_ActiveEditor"), activeEditor))
            {
                activeEditor = !activeEditor;
            }
            listing_Standard.GapLine();

            foreach(var editor in editors)
            {
                if (editor == null)
                    continue;

                if (listing_Standard.ButtonText($"{editor.EditorName}: {editor.CallKeyCode}"))
                {
                    SetKeyWithList((code) => editor.SetKey(code));
                }
            }
           
            listing_Standard.End();
        }

        private void SetKeyWithList(Action<KeyCode> callback)
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                list.Add(new FloatMenuOption(code.ToString(), delegate
                {
                    callback(code);

                    Messages.Message("WE_Settings_Key_Update".Translate(code.ToString()), MessageTypeDefOf.NeutralEvent, false);
                }));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        public override void ExposeData()
        {
            base.ExposeData();

            WorldEditor.WorldEditorInstance.ExposeData();

            WorldEditor.WorldEditorInstance.CheckMissingEditors();
        }
    }
}
