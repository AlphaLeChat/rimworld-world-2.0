using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
	public class WorldTemplateFaction : IExposable
	{
		public FactionDef def;

		private string name;

		public int loadID = -1;

		public int randomKey;

		public float colorFromSpectrum = -999f;

		public float centralMelanin = 0.5f;

		private List<FactionRelation> relations = new List<FactionRelation>();

		public Pawn leader;

		public KidnappedPawnsTracker kidnapped;

		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();

		public bool defeated;

		public int lastTraderRequestTick = -9999999;

		public int lastMilitaryAidRequestTick = -9999999;

		public int lastExecutionTick = -9999999;

		private int naturalGoodwillTimer;

		public bool allowRoyalFavorRewards = true;

		public bool allowGoodwillRewards = true;

		public List<string> questTags;

		public Color? color;

		public bool? hidden;

		public bool temporary;

		public bool factionHostileOnHarmByPlayer;

		public bool neverFlee;

		public FactionIdeosTracker ideos;

		public void ExposeData()
		{
			Scribe_References.Look(ref leader, "leader");
			Scribe_Defs.Look(ref def, "def");
			Scribe_Values.Look(ref name, "name");
			Scribe_Values.Look(ref loadID, "loadID", 0);
			Scribe_Values.Look(ref randomKey, "randomKey", 0);
			Scribe_Values.Look(ref colorFromSpectrum, "colorFromSpectrum", 0f);
			Scribe_Values.Look(ref centralMelanin, "centralMelanin", 0f);
			Scribe_Collections.Look(ref relations, "relations", LookMode.Deep);
			Scribe_Deep.Look(ref kidnapped, "kidnapped", this);
			Scribe_Deep.Look(ref ideos, "ideos", this);
			Scribe_Collections.Look(ref predatorThreats, "predatorThreats", LookMode.Deep);
			Scribe_Values.Look(ref defeated, "defeated", defaultValue: false);
			Scribe_Values.Look(ref lastTraderRequestTick, "lastTraderRequestTick", -9999999);
			Scribe_Values.Look(ref lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999);
			Scribe_Values.Look(ref lastExecutionTick, "lastExecutionTick", -9999999);
			Scribe_Values.Look(ref naturalGoodwillTimer, "naturalGoodwillTimer", 0);
			Scribe_Values.Look(ref allowRoyalFavorRewards, "allowRoyalFavorRewards", defaultValue: true);
			Scribe_Values.Look(ref allowGoodwillRewards, "allowGoodwillRewards", defaultValue: true);
			Scribe_Collections.Look(ref questTags, "questTags", LookMode.Value);
			Scribe_Values.Look(ref hidden, "hidden");
			Scribe_Values.Look(ref temporary, "temporary", defaultValue: false);
			Scribe_Values.Look(ref factionHostileOnHarmByPlayer, "factionHostileOnHarmByPlayer", defaultValue: false);
			Scribe_Values.Look(ref color, "color");
			Scribe_Values.Look(ref neverFlee, "neverFlee", defaultValue: false);
		}
	}
}
