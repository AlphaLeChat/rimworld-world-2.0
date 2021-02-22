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
    public class WorldEditWorldObject_AbandonedSettlement : WorldEditWorldObject
    {
        public override string ObjectName => "WorldEditWorldObject_AbandonedSettlement".Translate();

        public override WorldObjectDef WorldObjectEditorDef => WorldObjectDefOf.AbandonedSettlement;

        protected override Type Window => typeof(WorldEditWorldObject_AbandonedSettlementWindow);
    }
}
