using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_GeologicalLandforms
{
    public class GameComponent_GeologicalLandforms : GameComponent
    {
        public Dictionary<int, string> TileData = new Dictionary<int, string>();

        public GameComponent_GeologicalLandforms()
        {
        }

        public GameComponent_GeologicalLandforms(Game game)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref TileData, "TileData", LookMode.Value, LookMode.Value);
        }
    }
}
