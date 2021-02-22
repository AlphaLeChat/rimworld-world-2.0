using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.Utils;
using WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObject_SiteWindow : WorldEditWorldObjectWindow
    {
        protected override string Title => "WorldEditWorldObject_Site".Translate();

        public override Vector2 InitialSize => new Vector2(636, 636);

        private SitePart mainSitePart;

        private List<SitePart> parts;

        private Site site;

        private static Vector2 partsScroll = Vector2.zero;

        public WorldEditWorldObject_SiteWindow(WorldObject worldObject, WorldEditWorldObject editor) : base(worldObject, editor)
        {
            site = (Site)worldObject;

            mainSitePart = site.parts[0];

            parts = new List<SitePart>(site.parts);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 600, 20), Title);
            Text.Anchor = TextAnchor.UpperLeft;

            int y = 30;
            Widgets.Label(new Rect(0, y, 100, 25), Translator.Translate("WorldEditWorldObject_FactionOwner"));
            if (Widgets.ButtonText(new Rect(105, y, 495, 25), setFaction.Name))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var faction in Find.FactionManager.AllFactionsListForReading)
                {
                    list.Add(new FloatMenuOption(faction.Name, delegate
                    {
                        setFaction = faction;
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }

            y += 30;

            Rect compsRect = new Rect(0, y, 600, 20);
            WorldEditWorldObjectCompUtility.DrawWorldEditWorldObjectComps(compsRect, worldEditWorldObjectComps, worldObject);
            y += 190;

            Widgets.Label(new Rect(0, y, 100, 25), Translator.Translate("WorldEditWorldObject_SiteWindow_MainCorePart"));
            if (Widgets.ButtonText(new Rect(105, y, 495, 25), mainSitePart.def.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var sitePartDef in DefDatabase<SitePartDef>.AllDefs.Where(def => def != mainSitePart.def))
                {
                    list.Add(new FloatMenuOption(sitePartDef.LabelCap, delegate
                    {
                        parts.Clear();

                        mainSitePart = CreateNewPart(sitePartDef);
                        parts.Add(mainSitePart);
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }
            y += 25;
            if (Widgets.ButtonText(new Rect(0, y, 600, 25), "WorldEditWorldObject_SiteWindow_ConfigureSitePart".Translate()))
            {
                Find.WindowStack.Add(new WorldEditSitePartParamsWindow(mainSitePart));
            }

            y += 25;
            Widgets.Label(new Rect(0, y, 600, 20), Translator.Translate("WorldEditWorldObject_SiteWindow_AdditionalParts"));

            y += 25;

            int partsSize = parts.Count * 25;
            Rect scrollRectFact = new Rect(0, y, 600, 200);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, partsSize);
            Widgets.BeginScrollView(scrollRectFact, ref partsScroll, scrollVertRectFact);
            int yButtonPos = 0;
            float buttonRectWidth = inRect.width - 10;
            for(int i = 1; i < parts.Count; i++)
            {
                SitePart part = parts[i];

                Widgets.Label(new Rect(0, yButtonPos, 300, 25), part.def.LabelCap);
                if (Widgets.ButtonText(new Rect(305, yButtonPos, 185, 25), "WorldEditWorldObject_SiteWindow_ConfigureSitePart".Translate()))
                {
                    Find.WindowStack.Add(new WorldEditSitePartParamsWindow(part));
                }
                if (Widgets.ButtonText(new Rect(495, yButtonPos, 95, 25), "WorldEditWorldObject_SiteWindow_DeletePart".Translate()))
                {
                    parts.Remove(part);
                }

                yButtonPos += 25;
            }
            Widgets.EndScrollView();
            y += 210;

            if (Widgets.ButtonText(new Rect(0, y, 600, 20), Translator.Translate("WorldEditWorldObject_SiteWindow_AddNewPart")))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var sitePartDef in DefDatabase<SitePartDef>.AllDefs.Where(def => def != mainSitePart.def && !parts.Any(x => x.def == def)))
                {
                    list.Add(new FloatMenuOption(sitePartDef.LabelCap, delegate
                    {
                        parts.Add(CreateNewPart(sitePartDef));
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }

            y += 40;

            if (Widgets.ButtonText(new Rect(0, y, 600, 20), Translator.Translate("WorldEditWorldObject_SaveOrCreate")))
            {
                SaveObject();
            }
        }

        private SitePart CreateNewPart(SitePartDef sitePartDef)
        {
            return new SitePart(site, sitePartDef, sitePartDef.Worker.GenerateDefaultParams(0, worldObject.Tile, setFaction));
        }

        protected override void SaveObject()
        {
            site.parts = new List<SitePart>(parts);

            base.SaveObject();
        }
    }
}
