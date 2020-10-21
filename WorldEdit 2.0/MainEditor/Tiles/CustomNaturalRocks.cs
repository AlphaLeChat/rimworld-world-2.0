using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Tiles
{
    public class GameComponent_CustomNaturalRocks : GameComponent
    {
        public Dictionary<int, CustomRock> ResourceData = new Dictionary<int, CustomRock>();

        public GameComponent_CustomNaturalRocks()
        {
        }

        public GameComponent_CustomNaturalRocks(Game game)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref ResourceData, "overrideNaturalRocksIds", LookMode.Value, LookMode.Deep);
        }
    }
}
