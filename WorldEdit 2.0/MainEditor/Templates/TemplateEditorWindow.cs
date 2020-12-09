using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Templates.PawnEditor;

namespace WorldEdit_2_0.MainEditor.Templates
{
    public class TemplateEditorWindow : EditWindow
    {
        private TemplateEditor templateEditor;

        public override Vector2 InitialSize => new Vector2(720, 600);
        private Vector2 scrollDesc = Vector2.zero;

        private string templateName;
        private string author;
        private string description;
        private bool canSelectPawns;

        private bool edbEditor = false;

        private GameComponent_WorldEditTemplate worldTemplate;


        public TemplateEditorWindow(TemplateEditor templateEditor)
        {
            this.templateEditor = templateEditor;

            worldTemplate = Current.Game.GetComponent<GameComponent_WorldEditTemplate>();

            if(worldTemplate.IsValidTemplate)
            {
                templateName = worldTemplate.TemplateName;
                author = worldTemplate.Author;
                description = worldTemplate.Description;

                canSelectPawns = worldTemplate.CanSelectPawns;
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Widgets.Label(new Rect(0, 0, 150, 20), Translator.Translate("TemplateEditorWindow_TemplateName"));
            templateName = Widgets.TextField(new Rect(0, 20, 300, 20), templateName);

            Widgets.Label(new Rect(0, 50, 150, 20), Translator.Translate("TemplateEditorWindow_Author"));
            author = Widgets.TextField(new Rect(0, 70, 300, 20), author);

            Widgets.Label(new Rect(0, 100, 150, 20), Translator.Translate("TemplateEditorWindow_Description"));
            description = Widgets.TextAreaScrollable(new Rect(0, 120, 300, 430), description, ref scrollDesc);

            Widgets.Label(new Rect(310, 15, 400, 20), Translator.Translate("TemplateEditorWindow_SelectStoryteller"));
            if (Widgets.ButtonText(new Rect(310, 40, 400, 20), Current.Game.storyteller.def.LabelCap))
            {
                Find.WindowStack.Add(new Page_SelectStorytellerInGame());
            }
            Widgets.Label(new Rect(310, 80, 400, 20), Translator.Translate("TemplateEditorWindow_SelectScenario"));
            if (Widgets.ButtonText(new Rect(310, 105, 400, 20), Current.Game.Scenario.name))
            {
                Find.WindowStack.Add(new Page_SelectScenarioInGame());
            }

            Widgets.Label(new Rect(310, 140, 400, 20), Translator.Translate("TemplateEditorWindow_SelectPawnMode"));
            Rect rect1 = new Rect(310, 165, 400, 20);
            TooltipHandler.TipRegion(rect1, Translator.Translate("TemplateEditorWindow_SelectPawnModeInfo"));
            if (Widgets.ButtonText(new Rect(310, 165, 400, 20), canSelectPawns ? "TemplateEditorWindow_Allowed".Translate() : "TemplateEditorWindow_NotAllowed".Translate()))
            {
                canSelectPawns = !canSelectPawns;
            }
            if (!canSelectPawns)
            {
                Widgets.Label(new Rect(310, 190, 200, 20), Translator.Translate("TemplateEditorWindow_EditStartPawns"));
                if (Widgets.ButtonText(new Rect(515, 190, 195, 20), "Classic"))
                {
                }
                if (Widgets.ButtonText(new Rect(310, 215, 400, 20), Translator.Translate("TemplateEditorWindow_SelectPawnInfoButton")))
                {
                    if (edbEditor)
                    {
                        ShowEdbEditor();
                    }
                    else
                    {
                        ShowStandartEditor();
                    }
                }
            }

            if (Widgets.ButtonText(new Rect(0, 555, 710, 20), Translator.Translate("TemplateEditorWindow_CreateTemplate")))
            {
                CreateWorldTemplate();
            }
        }

        private void ShowStandartEditor()
        {
            Find.WindowStack.Add(new PawnMenu(Find.GameInitData.startingAndOptionalPawns, 99));
        }

        private void ShowEdbEditor()
        {
            
        }

        private void CreateWorldTemplate()
        {
            if (string.IsNullOrEmpty(templateName))
            {
                Messages.Message("TemplateEditorWindow_EnterCorrectName".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            if (string.IsNullOrEmpty(author))
            {
                Messages.Message("TemplateEditorWindow_EnterCorrectAuthor".Translate(), MessageTypeDefOf.NeutralEvent, false); ;
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                Messages.Message("TemplateEditorWindow_EnterCorrectDescription".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            worldTemplate.SetTemplateName(templateName);
            worldTemplate.SetAuthor(author);
            worldTemplate.SetDescription(description);
            worldTemplate.SetCanSelectPawns(canSelectPawns);
            worldTemplate.SetForceStartPawns(Find.GameInitData.startingAndOptionalPawns);

            GameDataSaveLoader.SaveGame(worldTemplate.TemplateName);

            worldTemplate.ClearTemplate();

            Page_SelectStartingSite page_SelectStartingSite = Find.WindowStack.Windows.FirstOrDefault(window => window is Page_SelectStartingSite) as Page_SelectStartingSite;
            if (page_SelectStartingSite != null)
            {
                Find.WindowStack.Add(Current.Game.Scenario.GetFirstConfigPage());
                page_SelectStartingSite.Close();
            }

            Messages.Message("TemplateEditorWindow_SuccessSave".Translate(), MessageTypeDefOf.PositiveEvent, false);

            Close();
        }
    }
}
