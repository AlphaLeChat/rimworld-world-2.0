using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0
{
    public class WorldTemplateDef : Def//, IExposable
    {
		public string author;

        public int listOrder;

        [Unsaved(false)]
        public Texture2D portraitTinyTex;

		[NoTranslate]
		private string portraitTiny;

		public string savegame;

		//public float planetCoverage;

		//public WorldTemplateGrid grid;

		//public WorldTemplateFactions factions;

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!portraitTiny.NullOrEmpty())
				{
					portraitTinyTex = ContentFinder<Texture2D>.Get(portraitTiny);
                }
                else
                {
					portraitTinyTex = BaseContent.BadTex;
				}
			});
		}

  //      public void ExposeData()
  //      {
		//	//Scribe_Values.Look(ref defName, "defName");
		//	//Scribe_Values.Look(ref label, "label");
		//	//Scribe_Values.Look(ref description, "description");
		//	//Scribe_Values.Look(ref author, "author");

		//	//Scribe_Values.Look(ref savegame, "savegame", null);

		//	//Scribe_Values.Look(ref planetCoverage, "planetCoverage");

		//	//Scribe_Deep.Look(ref grid, "grid");
		//	//Scribe_Deep.Look(ref factions, "factions");
		//}
    }
}
