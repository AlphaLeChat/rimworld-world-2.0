using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Models;

namespace WorldEdit_RimCity.WorldObjects
{
    public class WorldEditWorldObject_RimCity_City : WorldEditWorldObject
    {
        public override string ObjectName => "WorldEditWorldObject_RimCity_City".Translate(WorldObjectEditorDef.LabelCap);

        public override WorldObjectDef WorldObjectEditorDef => null;

        protected override Type Window => typeof(WorldEditWorldObject_RimCity_CityWindow);
    }
}
