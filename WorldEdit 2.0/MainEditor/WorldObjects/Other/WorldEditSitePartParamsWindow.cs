using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Utils;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other
{
    public class WorldEditSitePartParamsWindow : Window
    {
        private SitePart sitePart;

        public override Vector2 InitialSize => new Vector2(466, 200);

        private float setThreatPoints;
        private int setTurretsCount;
        private int setMortarsCount;
        private PawnKindDef setAnimalKind;
        private ThingDef setPreciousLumpResources;
        private List<Thing> setSiteThings;

        public WorldEditSitePartParamsWindow(SitePart sitePart)
        {
            this.sitePart = sitePart;

            doCloseX = true;
            resizeable = false;

            setThreatPoints = sitePart.parms.threatPoints;
            setTurretsCount = sitePart.parms.turretsCount;
            setMortarsCount = sitePart.parms.mortarsCount;
            setAnimalKind = sitePart.parms.animalKind ?? DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef k) => k.RaceProps.Animal).First();
            setPreciousLumpResources = sitePart.parms.preciousLumpResources ?? DefDatabase<ThingDef>.AllDefs.Where(tDef => tDef.mineable).First();

            setSiteThings = sitePart.things == null ? new List<Thing>() : sitePart.things.ToList();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 430, 25), sitePart.def.LabelCap);
            Text.Anchor = TextAnchor.UpperLeft;

            int y = 40;
            Widgets.Label(new Rect(0, 40, 220, 25), "WorldEditSitePartParamsWindow_ThreatPoints".Translate());
            float.TryParse(Widgets.TextField(new Rect(225, 40, 205, 25), setThreatPoints.ToString()), out setThreatPoints);

            if(sitePart.def == SitePartDefOf.Turrets)
            {
                y += 30;

                Widgets.Label(new Rect(0, y, 220, 25), "WorldEditSitePartParamsWindow_TurretsCount".Translate());
                int.TryParse(Widgets.TextField(new Rect(225, y, 205, 25), setTurretsCount.ToString()), out setTurretsCount);

                y += 30;

                Widgets.Label(new Rect(0, y, 220, 25), "WorldEditSitePartParamsWindow_MortarsCount".Translate());
                int.TryParse(Widgets.TextField(new Rect(225, y, 205, 25), setMortarsCount.ToString()), out setMortarsCount);
            }else if(sitePart.def == SitePartDefOf.Manhunters)
            {
                y += 30;

                Widgets.Label(new Rect(0, y, 220, 25), "WorldEditSitePartParamsWindow_AnimalKind".Translate());
                if(Widgets.ButtonText(new Rect(225, y, 205, 25), setAnimalKind.LabelCap))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach(var pawnKindDef in DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef k) => k.RaceProps.Animal))
                    {
                        list.Add(new FloatMenuOption(pawnKindDef.LabelCap, () =>
                        {
                            setAnimalKind = pawnKindDef;
                        }));
                    }

                    Find.WindowStack.Add(new FloatMenu(list));
                }
            }else if(sitePart.def == SitePartDefOf.PreciousLump)
            {
                y += 30;

                Widgets.Label(new Rect(0, y, 220, 25), "WorldEditSitePartParamsWindow_LumpResources".Translate());
                if (Widgets.ButtonText(new Rect(225, y, 205, 25), setPreciousLumpResources.LabelCap))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (var thingDef in DefDatabase<ThingDef>.AllDefs.Where(tDef => tDef.mineable))
                    {
                        list.Add(new FloatMenuOption(thingDef.LabelCap, () =>
                        {
                            setPreciousLumpResources = thingDef;
                        }));
                    }

                    Find.WindowStack.Add(new FloatMenu(list));
                }
            }else if(sitePart.def == Defs.SitePartDefOf.ItemStash)
            {
                y += 30;
                if (Widgets.ButtonText(new Rect(0, y, 430, 25), "WorldEditSitePartParamsWindow_ConfigureMapItems".Translate()))
                {
                    Find.WindowStack.Add(new ThingsMenu(setSiteThings, true));
                }
            }

            if(Widgets.ButtonText(new Rect(0, 134, 430, 25), "WorldEditSitePartParamsWindow_Save".Translate()))
            {
                Save();
            }
        }

        private void Save()
        {
            if (setThreatPoints < 35 && sitePart.def.wantsThreatPoints)
                setThreatPoints = 35;

            sitePart.parms.threatPoints = setThreatPoints;
            if (sitePart.def == SitePartDefOf.Turrets)
            {
                sitePart.parms.turretsCount = setTurretsCount;
                sitePart.parms.mortarsCount = setMortarsCount;
            }
            else if (sitePart.def == SitePartDefOf.Manhunters)
            {
                sitePart.parms.animalKind = setAnimalKind;
            }
            else if (sitePart.def == SitePartDefOf.PreciousLump)
            {
                sitePart.parms.preciousLumpResources = setPreciousLumpResources;
            }else if(sitePart.def == Defs.SitePartDefOf.ItemStash)
            {
                if (setSiteThings.Count > 0)
                {
                    if (sitePart.things == null)
                        sitePart.things = new ThingOwner<Thing>();

                    sitePart.things.TryAddRangeOrTransfer(setSiteThings);
                }
            }

            Messages.Message("WorldEditWorldObject_Saved".Translate(), MessageTypeDefOf.NeutralEvent, false);

            Close();
        }
    }
}
