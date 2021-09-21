using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public class WorldTemplateFactions : IExposable
    {
        public List<Faction> allFactions = new List<Faction>();

        public List<WorldObject> worldObjects = new List<WorldObject>();

		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();
		public void ExposeData()
        {
            Scribe_Collections.Look(ref allFactions, "allFactions", LookMode.Deep);

			if (Scribe.mode == LoadSaveMode.Saving)
			{
				tmpUnsavedWorldObjects.Clear();
				for (int num = worldObjects.Count - 1; num >= 0; num--)
				{
					if (!worldObjects[num].def.saved)
					{
						tmpUnsavedWorldObjects.Add(worldObjects[num]);
						worldObjects.RemoveAt(num);
					}
				}
			}
			Scribe_Collections.Look(ref worldObjects, "worldObjects", LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				worldObjects.AddRange(tmpUnsavedWorldObjects);
				tmpUnsavedWorldObjects.Clear();
			}
		}
    }
}
