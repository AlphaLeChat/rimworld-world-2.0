using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Tiles
{
    public class CustomRock : IExposable
    {
        private int tile;
        public int Tile => tile;

        public List<ThingDef> Rocks = new List<ThingDef>();

        public bool Caves;

        public CustomRock()
        {
        }

        public CustomRock(int tile, List<ThingDef> things, bool caves)
        {
            this.tile = tile;
            Rocks = new List<ThingDef>(things);
            Caves = caves;
        }

        public void SetRocksList(List<ThingDef> list) => Rocks = new List<ThingDef>(list);

        public void AddRock(ThingDef rockType)
        {
            if (!Rocks.Contains(rockType))
                Rocks.Add(rockType);
        }

        public void SetTile(int tileId) => tile = tileId;

        public void ExposeData()
        {
            Scribe_Values.Look(ref tile, "tile", -1);
            Scribe_Values.Look(ref Caves, "Caves", false);
            Scribe_Collections.Look(ref Rocks, "rocks", LookMode.Def);
        }
    }
}
