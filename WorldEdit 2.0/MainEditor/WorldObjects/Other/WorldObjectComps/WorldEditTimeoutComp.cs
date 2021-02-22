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
    public class WorldEditTimeoutComp : WorldEditWorldObjectComp
    {
        public override string GuiCompName => "WorldEditTimeoutComp".Translate();

        public override Type WorldObjectCompType => typeof(TimeoutComp);

        protected override Type EditWindow => typeof(WorldEditTimeoutCompWindow);

        public override string GuiDescription => "WorldEditTimeoutComp_Description".Translate();
    }
}
