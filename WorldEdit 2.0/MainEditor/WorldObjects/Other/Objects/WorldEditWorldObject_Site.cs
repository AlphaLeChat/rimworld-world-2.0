using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObject_Site : WorldEditWorldObject
    {
        public override string ObjectName => "WorldEditWorldObject_Site".Translate();

        public override WorldObjectDef WorldObjectEditorDef => WorldObjectDefOf.Site;

        protected override Type Window => typeof(WorldEditWorldObject_SiteWindow);

        public override void WorldObjectCreated(WorldObject worldObject)
        {
            if (worldObject is Site site)
            {
                if (!site.parts.Any())
                    site.AddPart(new SitePart(site, SitePartDefOf.Outpost, SitePartDefOf.Outpost.Worker.GenerateDefaultParams(1, site.Tile, site.Faction)));
            }
        }
    }
}
