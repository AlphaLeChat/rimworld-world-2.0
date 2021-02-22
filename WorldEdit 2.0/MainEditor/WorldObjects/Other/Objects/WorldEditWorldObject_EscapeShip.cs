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
    public class WorldEditWorldObject_EscapeShip : WorldEditWorldObject
    {
        public override string ObjectName => "WorldEditWorldObject_EscapeShip".Translate();

        public override WorldObjectDef WorldObjectEditorDef => WorldObjectDefOf.EscapeShip;

        protected override Type Window => null;

        public override void CreateView(WorldObject worldObject)
        {
            Messages.Message("WorldEditWorldObject_Saved".Translate(), MessageTypeDefOf.NeutralEvent, false);

            WorldEditor.WorldEditorInstance.GetEditor<ObjectsEditor>().GetEditorWindow<ObjectsEditorWindow>().RecacheObjects();
        }


    }
}
