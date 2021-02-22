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
    public class ThingsMenu : Window
    {
        public override Vector2 InitialSize => new Vector2(705, 600);

        private List<Thing> things;

        private static string noStuffLabel = "ThingsMenu_NoStuffLabel".Translate();
        private static string noQualityLabel = "ThingsMenu_NoQualityLabel".Translate();
        private static Vector2 stockListScroll = Vector2.zero;

        private static List<ThingCategoryDef> categories;

        private int sliderHeight;
        private bool useMaxStackCount;

        public ThingsMenu(List<Thing> things, bool useMaxStackCount = false)
        {
            this.things = things;

            resizeable = false;
            doCloseX = true;

            this.useMaxStackCount = useMaxStackCount;
        }

        private static List<ThingCategoryDef> GetCategories()
        {
            if (categories == null)
                categories = DefDatabase<ThingCategoryDef>.AllDefs.Where(cat => cat != ThingCategoryDefOf.Root && DefDatabase<ThingDef>.AllDefs.Any(tDef => tDef.IsWithinCategory(cat))).ToList();

            return categories;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, inRect.width, 20), "ThingsMenu_Title".Translate());
            Text.Anchor = TextAnchor.UpperLeft;

            inRect.y += 30;

            DrawEditableThingsList(ref inRect, things, ref sliderHeight, useMaxStackCount);

            if(Widgets.ButtonText(new Rect(0, inRect.y, inRect.width, 20), "ThingsMenu_Close".Translate()))
            {
                Close();
            }
        }

        public static void DrawEditableThingsList(ref Rect inRect, List<Thing> thingsList, ref int sliderHeight, bool useMaxStackCount = false)
        {
            Widgets.Label(new Rect(inRect.x, inRect.y, 250, 20), "ThingsMenu_ItemName".Translate());
            Widgets.Label(new Rect(inRect.x + 270, inRect.y, 70, 20), "ThingsMenu_ItemCount".Translate());
            Widgets.Label(new Rect(inRect.x + 360, inRect.y, 100, 20), "ThingsMenu_ItemStuff".Translate());
            Widgets.Label(new Rect(inRect.x + 485, inRect.y, 100, 20), "ThingsMenu_ItemQuality".Translate());

            inRect.y += 20;

            sliderHeight = thingsList.Count * 25;

            Rect scrollRectFact = new Rect(inRect.x, inRect.y + 5, 660, 435);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, sliderHeight);
            Widgets.DrawBox(new Rect(inRect.x, inRect.y, 665, 445));
            Widgets.BeginScrollView(scrollRectFact, ref stockListScroll, scrollVertRectFact);
            int x = 5;
            for (int i = 0; i < thingsList.Count; i++)
            {
                if (i >= thingsList.Count)
                    break;

                Thing good = thingsList[i].GetInnerIfMinified();

                Widgets.Label(new Rect(5, x, 250, 25), GenLabel.ThingLabel(good.def, good.Stuff));
                int.TryParse(Widgets.TextField(new Rect(260, x, 70, 20), good.stackCount.ToString()), out good.stackCount);

                if(useMaxStackCount && good.stackCount > good.def.stackLimit)
                {
                    good.stackCount = good.def.stackLimit;
                }

                if (Widgets.ButtonText(new Rect(335, x, 130, 20), good.def.MadeFromStuff ? good.Stuff.LabelCap.RawText : noStuffLabel))
                {
                    if (good.def.MadeFromStuff)
                    {
                        var thingDefStuffs = DefDatabase<ThingDef>.AllDefsListForReading.Where(tDef => tDef.IsStuff && tDef.stuffProps.CanMake(good.def)).ToList();

                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        if (thingDefStuffs.Count > 0)
                        {
                            foreach (ThingDef stuffDef in thingDefStuffs)
                            {
                                list.Add(new FloatMenuOption(stuffDef.LabelCap, delegate
                                {
                                    good.SetStuffDirect(stuffDef);
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
                }
                bool hasQuality = good.TryGetQuality(out QualityCategory qc);
                if (Widgets.ButtonText(new Rect(470, x, 90, 20), hasQuality ? qc.GetLabel() : noQualityLabel))
                {
                    if (hasQuality)
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        foreach (QualityCategory qual in Enum.GetValues(typeof(QualityCategory)))
                        {
                            list.Add(new FloatMenuOption(qual.GetLabel(), delegate
                            {
                                var innerThing = good.GetInnerIfMinified();

                                innerThing.TryGetComp<CompQuality>().SetQuality(qual, ArtGenerationContext.Colony);
                            }));
                        }
                        Find.WindowStack.Add(new FloatMenu(list));
                    }
                }

                if (Widgets.ButtonText(new Rect(565, x, 80, 20), Translator.Translate("ThingsMenu_DeleteGood")))
                {
                    thingsList.Remove(good);
                }
                x += 25;
            }
            Widgets.EndScrollView();

            inRect.y += 450;

            if (Widgets.ButtonText(new Rect(inRect.x, inRect.y, 670, 20), Translator.Translate("ThingsMenu_AddNewGood")))
            {
                List<FloatMenuOption> categoriesList = new List<FloatMenuOption>();
                foreach (var categoryDef in GetCategories())
                {
                    categoriesList.Add(new FloatMenuOption(categoryDef.LabelCap, delegate
                    {
                        var categoryThingDefs = DefDatabase<ThingDef>.AllDefsListForReading.Where(thingDef => thingDef.IsWithinCategory(categoryDef)).ToList();

                        if (categoryThingDefs.Count > 0)
                        {
                            List<FloatMenuOption> thingsList2 = new List<FloatMenuOption>();

                            foreach (ThingDef thingDef in categoryThingDefs)
                            {
                                thingsList2.Add(new FloatMenuOption(thingDef.LabelCap, delegate
                                {
                                    thingsList.Add(GenerateItem(thingDef));
                                }));
                            }

                            Find.WindowStack.Add(new FloatMenu(thingsList2));
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(categoriesList));
            }

            inRect.y += 40;
        }

        private static Thing GenerateItem(ThingDef thingDef)
        {
            Thing thing = ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));

            thing.TryGetComp<CompQuality>()?.SetQuality(QualityCategory.Normal, ArtGenerationContext.Colony);
            if (thing.def.Minifiable)
            {
                thing = thing.MakeMinified();
            }
            thing.stackCount = 1;
            thing.HitPoints = thing.MaxHitPoints;

            return thing;
        }
    }
}
