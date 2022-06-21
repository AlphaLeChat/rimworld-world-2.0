using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_GeologicalLandforms
{
    public class GeoTileData : IExposable
    {
        public List<string> landformsIds = new List<string>();

        public GeoTileData()
        {

        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref landformsIds, "landformsIds", LookMode.Value);
        }
    }
}
