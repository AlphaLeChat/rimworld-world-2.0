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
        public Dictionary<int, GeoTileData> TileData = new Dictionary<int, GeoTileData>();

        public GameComponent_GeologicalLandforms()
        {
        }

        public GameComponent_GeologicalLandforms(Game game)
        {
        }

        public void Add(int tileID, string landformId)
        {
            if(!TileData.TryGetValue(tileID, out GeoTileData geoTileData))
            {
                geoTileData = new GeoTileData();
                TileData.Add(tileID, geoTileData);
            }

            geoTileData.landformsIds.Add(landformId);
        }

        public void Remove(int tileID, string landformId)
        {
            if (TileData.TryGetValue(tileID, out GeoTileData geoTileData))
            {
                geoTileData.landformsIds.Remove(landformId);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref TileData, "TileData", LookMode.Value, LookMode.Deep);
        }
    }
}
