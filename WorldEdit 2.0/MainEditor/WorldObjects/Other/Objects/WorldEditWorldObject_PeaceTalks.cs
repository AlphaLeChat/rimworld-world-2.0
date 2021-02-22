using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObject_PeaceTalks : WorldEditWorldObject
    {
        public override string ObjectName => "WorldEditWorldObject_PeaceTalks".Translate();

        public override WorldObjectDef WorldObjectEditorDef => WorldObjectDefOf.PeaceTalks;

        protected override Type Window => typeof(WorldEditWorldObject_PeaceTalksWindow);
    }
}
