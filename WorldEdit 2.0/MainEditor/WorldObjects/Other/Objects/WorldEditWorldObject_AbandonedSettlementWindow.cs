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

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObject_AbandonedSettlementWindow : WorldEditWorldObjectWindow
    {
        protected override string Title => Translator.Translate("WorldEditWorldObject_AbandonedSettlementWindow_Title");

        public WorldEditWorldObject_AbandonedSettlementWindow(WorldObject worldObject, WorldEditWorldObject editor) : base(worldObject, editor)
        {
        }
    }
}
