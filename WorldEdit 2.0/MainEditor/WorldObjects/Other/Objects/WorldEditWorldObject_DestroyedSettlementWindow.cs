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
    public class WorldEditWorldObject_DestroyedSettlementWindow : WorldEditWorldObjectWindow
    {
        protected override string Title => Translator.Translate("WorldEditWorldObject_DestroyedSettlementWindow_Title");

        public WorldEditWorldObject_DestroyedSettlementWindow(WorldObject worldObject, WorldEditWorldObject editor) : base(worldObject, editor)
        {
        }
    }
}
