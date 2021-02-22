using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObject_PeaceTalksWindow : WorldEditWorldObjectWindow
    {
        protected override string Title => "WorldEditWorldObject_PeaceTalks".Translate();

        public WorldEditWorldObject_PeaceTalksWindow(WorldObject worldObject, WorldEditWorldObject editor) : base(worldObject, editor)
        {
        }
    }
}
