using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.WorldObjects.Other;

namespace WorldEdit_RimCity.WorldObjects
{
    public class WorldEditWorldObject_RimCity_CityWindow : WorldEditWorldObjectWindow
    {
        protected override string Title => editor.ObjectName;

        public WorldEditWorldObject_RimCity_CityWindow(WorldObject worldObject, WorldEditWorldObject editor) : base(worldObject, editor)
        {
        }
    }
}
