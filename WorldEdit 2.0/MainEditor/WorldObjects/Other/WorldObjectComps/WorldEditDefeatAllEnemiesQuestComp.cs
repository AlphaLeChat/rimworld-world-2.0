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
    public class WorldEditDefeatAllEnemiesQuestComp : WorldEditWorldObjectComp
    {
        public override string GuiCompName => "WorldEditDefeatAllEnemiesQuestComp".Translate();

        public override Type WorldObjectCompType => typeof(DefeatAllEnemiesQuestComp);

        protected override Type EditWindow => typeof(WorldEditDefeatAllEnemiesQuestCompWindow);

        public override string GuiDescription => "WorldEditDefeatAllEnemiesQuestComp_Description".Translate();
    }
}
