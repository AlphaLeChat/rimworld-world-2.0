using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.Utils
{
    public class ThingMenu : Window
    {
        public override Vector2 InitialSize => new Vector2(536, 268);

        private ThingCategoryDef category = null;

        private List<ThingDef> categoryThingDefs = new List<ThingDef>();

        private List<ThingDef> thingDefStuffs = new List<ThingDef>();

        private List<ThingCategoryDef> categories;

        private ThingDef selectedThingDef = null;
        private ThingDef selectedStuff = null;

        private List<Thing> stockList;

        private string stackBuffer = "1";
        private int stackCount = 1;

        private QualityCategory quality = QualityCategory.Normal;

        public ThingMenu(List<Thing> stockList)
        {
            this.stockList = stockList;

            resizeable = false;
            doCloseX = true;

            categories = DefDatabase<ThingCategoryDef>.AllDefs.Where(cat => cat != ThingCategoryDefOf.Root).ToList();
            category = categories.First();
            thingDefStuffs = DefDatabase<ThingDef>.AllDefsListForReading.Where(tDef => tDef.IsStuff).ToList();
            selectedStuff = thingDefStuffs.FirstOrDefault();

            UpdateThingDefs(category);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 500, 20), Translator.Translate("ThingMenu_Title"));
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.Label(new Rect(0, 30, 150, 20), Translator.Translate("ThingsMenu_Category"));
            if (Widgets.ButtonText(new Rect(0, 50, 240, 20), category.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var thing in categories)
                    list.Add(new FloatMenuOption(thing.label, delegate
                    {
                        category = thing;

                        UpdateThingDefs(category);
                    }));
                Find.WindowStack.Add(new FloatMenu(list));
            }
            Widgets.Label(new Rect(250, 30, 150, 20), Translator.Translate("ThingsMenu_CategoryItem"));
            if (Widgets.ButtonText(new Rect(250, 50, 250, 20), selectedThingDef?.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                if (categoryThingDefs.Count > 0)
                {
                    foreach (ThingDef thingDef in categoryThingDefs)
                    {
                        list.Add(new FloatMenuOption(thingDef.LabelCap, delegate
                        {
                            selectedThingDef = thingDef;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                }
                else
                {
                    list.Add(new FloatMenuOption("ThingsMenu_NoItemsFromThisCategory".Translate(), delegate
                    {
                    }));
                }
            }

            if (selectedThingDef != null)
            {
                int thingSettingsY = 75;

                if(selectedThingDef.MadeFromStuff)
                {
                    Widgets.Label(new Rect(0, thingSettingsY, 150, 20), Translator.Translate("ThingsMenu_ThingDefStuff"));
                    thingSettingsY += 20;
                    if (Widgets.ButtonText(new Rect(0, thingSettingsY, 500, 20), selectedStuff.LabelCap))
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        if (thingDefStuffs.Count > 0)
                        {
                            foreach (ThingDef stuffDef in thingDefStuffs)
                            {
                                list.Add(new FloatMenuOption(stuffDef.LabelCap, delegate
                                {
                                    selectedStuff = stuffDef;
                                }));
                            }
                            Find.WindowStack.Add(new FloatMenu(list));
                        }
                        else
                        {
                            list.Add(new FloatMenuOption("ThingsMenu_NoStuffsAvaliable".Translate(), delegate
                            {
                            }));
                        }
                    }

                    thingSettingsY += 25;
                }

                if (selectedThingDef.FollowQualityThingFilter())
                {
                    Widgets.Label(new Rect(0, thingSettingsY, 150, 20), Translator.Translate("ThingsMenu_ThingDefQuality"));
                    thingSettingsY += 20;
                    if (Widgets.ButtonText(new Rect(0, thingSettingsY, 500, 20), quality.GetLabel()))
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        foreach (QualityCategory qual in Enum.GetValues(typeof(QualityCategory)))
                        {
                            list.Add(new FloatMenuOption(qual.GetLabel(), delegate
                            {
                                quality = qual;
                            }));
                        }
                        Find.WindowStack.Add(new FloatMenu(list));
                    }

                    thingSettingsY += 25;
                }

                Widgets.Label(new Rect(0, thingSettingsY, 150, 20), Translator.Translate("ThingsMenu_StackCount"));
                Widgets.TextFieldNumeric(new Rect(155, thingSettingsY, 345, 20), ref stackCount, ref stackBuffer, 0);
            }

            if (Widgets.ButtonText(new Rect(0, inRect.height - 38, 500, 20), Translator.Translate("ThingsMenu_GenerateItem")))
            {
                GenerateAndAddToStock(selectedThingDef, quality, stackCount, selectedStuff);
            }
        }

        private void UpdateThingDefs(ThingCategoryDef categoryDef)
        {
            categoryThingDefs = DefDatabase<ThingDef>.AllDefsListForReading.Where(thingDef => thingDef.IsWithinCategory(categoryDef)).ToList();

            selectedThingDef = categoryThingDefs.FirstOrDefault();
        }

        private void GenerateAndAddToStock(ThingDef thingDef, QualityCategory qualityCategory, int stackCount, ThingDef stuffDef = null)
        {
            if (thingDef == null)
            {
                Messages.Message("ThingsMenu_NoThingDef".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            Thing thing = ThingMaker.MakeThing(thingDef, thingDef.MadeFromStuff ? stuffDef : null);
            thing.TryGetComp<CompQuality>()?.SetQuality(qualityCategory, ArtGenerationContext.Colony);
            if (thing.def.Minifiable)
            {
                thing = thing.MakeMinified();
            }
            thing.stackCount = stackCount;

            stockList.Add(thing);

            Close();

            Messages.Message("ThingsMenu_SuccessAdded".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }
    }
}
