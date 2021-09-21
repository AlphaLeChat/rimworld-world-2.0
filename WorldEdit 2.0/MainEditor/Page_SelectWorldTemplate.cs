using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;
using WorldEdit_2_0.Defs;
using WorldEdit_2_0.MainEditor.Templates;
using WorldEdit_2_0.Patches;
using WorldEdit_2_0.Patches.MainMenu;

namespace WorldEdit_2_0.MainEditor
{
	[StaticConstructorOnStartup]
    public class Page_SelectWorldTemplate : Page
    {
		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight");

		public static readonly Vector2 WorldTemplatePortraitSizeTiny = new Vector2(116f, 124f);

		private static Rect explanationInnerRect = default(Rect);
		public override string PageTitle => "Page_SelectWorldTemplate".Translate();

		private Vector2 scrollPosition;

		private WorldTemplateDef worldTemplateDef;
		private static Vector2 explanationScrollPosition = default(Vector2);

		private Listing_Standard infoListing = new Listing_Standard();

		public override void DoWindowContents(Rect rect)
		{
			DrawPageTitle(rect);
			DrawWorldTemplates(GetMainRect(rect));
			DoBottomButtons(rect);
		}

		private void DrawWorldTemplates(Rect rect)
        {
			GUI.BeginGroup(rect);
			Rect outRect = new Rect(0f, 0f, WorldTemplatePortraitSizeTiny.x + 16f, rect.height);
			Widgets.BeginScrollView(viewRect: new Rect(0f, 0f, WorldTemplatePortraitSizeTiny.x, DefDatabase<WorldTemplateDef>.AllDefs.Count() * (WorldTemplatePortraitSizeTiny.y + 10f)), outRect: outRect, scrollPosition: ref scrollPosition);
			Rect rect2 = new Rect(0f, 0f, WorldTemplatePortraitSizeTiny.x, WorldTemplatePortraitSizeTiny.y);
			foreach (WorldTemplateDef item in DefDatabase<WorldTemplateDef>.AllDefs.OrderBy((WorldTemplateDef tel) => tel.listOrder))
			{
				if (Widgets.ButtonImage(rect2, item.portraitTinyTex))
				{
					worldTemplateDef = item;
				}
				if (worldTemplateDef == item)
				{
					GUI.DrawTexture(rect2, StorytellerHighlightTex);
				}
				rect2.y += rect2.height + 8f;
			}
			Widgets.EndScrollView();

			Rect outRect2 = new Rect(outRect.xMax + 8f, 0f, rect.width - outRect.width - 8f, rect.height);
			explanationInnerRect.width = outRect2.width - 16f;
			Widgets.BeginScrollView(outRect2, ref explanationScrollPosition, explanationInnerRect);
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(0f, 0f, outRect2.width - 16f, 999f), "HowWorldTemplateWork".Translate());
			Rect rect3 = new Rect(0f, 120f, outRect2.width - 16f, 9999f);
			float num = 300f;
			if (worldTemplateDef != null)
			{
				Text.Anchor = TextAnchor.UpperLeft;
				infoListing.Begin(rect3);
				Text.Font = GameFont.Medium;
				infoListing.Indent(15f);
				infoListing.Label(worldTemplateDef.label);
				infoListing.Outdent(15f);

				Text.Anchor = TextAnchor.UpperRight;
				infoListing.Label("PSWL_Author".Translate(worldTemplateDef.author));
				Text.Anchor = TextAnchor.UpperLeft;

				Text.Font = GameFont.Small;
				infoListing.Gap(8f);
				infoListing.Label(worldTemplateDef.description);
				num = rect3.y + infoListing.CurHeight;
				infoListing.End();
			}
			explanationInnerRect.height = num;
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}

			if(worldTemplateDef == null)
            {
				Messages.Message("MustChooseWorldTemplateDef".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}

			WorldEditor.InitWorldTemplateDef = worldTemplateDef;

            if (worldTemplateDef != WorldTemplateDefOf.Standart_Template)
            {
				XDocument xDocument = XDocument.Parse($"<savegame>{worldTemplateDef.savegame}</savegame>");

				XmlDocument defDocument = new XmlDocument();
				defDocument.LoadXml(xDocument.Root.ToString());

				SavedGameLoaderNow_LoadGameFromSaveFileNow_WorldEdit.DocumentToLoad = defDocument;
				GameComponent_WorldEditTemplate.WorldTemplateDef = worldTemplateDef;

				LongEventHandler.QueueLongEvent(delegate
				{
					MemoryUtility.ClearAllMapsAndWorld();
					Current.Game = new Game();
					Current.Game.InitData = new GameInitData();
					Current.Game.InitData.gameToLoad = "server";

				}, "Play", "LoadingLongEvent", doAsynchronously: true, null);
            }
            else
            {
				Page_CustomStartingSite.OverrideStartingTile = -1;
				SavedGameLoaderNow_LoadGameFromSaveFileNow_WorldEdit.DocumentToLoad = null;
				GameComponent_WorldEditTemplate.WorldTemplateDef = null;
			}

            return true;
		}
	}
}
