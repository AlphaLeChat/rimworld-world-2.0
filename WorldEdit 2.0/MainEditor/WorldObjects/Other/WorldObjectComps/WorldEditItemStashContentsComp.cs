using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps
{
    public class WorldEditItemStashContentsComp : WorldEditWorldObjectComp
    {
        public override string GuiCompName => "WorldEditItemStashContentsComp".Translate();

        public override Type WorldObjectCompType => typeof(ItemStashContentsComp);

        protected override Type EditWindow => typeof(WorldEditItemStashContentsCompWindow);

        public override string GuiDescription => "WorldEditItemStashContentsComp_Description".Translate();

        public override bool CanUseWith(WorldObject worldObject)
        {
            if(worldObject is Site site)
            {
                if (site.parts.Any(prt => prt.def == Defs.SitePartDefOf.ItemStash))
                    return true;
            }

            return false;
        }
    }
}
