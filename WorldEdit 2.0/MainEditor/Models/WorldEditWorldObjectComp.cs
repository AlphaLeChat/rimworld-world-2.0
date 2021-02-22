using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps;

namespace WorldEdit_2_0.MainEditor.Models
{
    public abstract class WorldEditWorldObjectComp
    {
        public abstract string GuiCompName { get; }

        public virtual string GuiDescription { get; }

        public abstract Type WorldObjectCompType { get; }

        protected abstract Type EditWindow { get; }

        public virtual void Edit(WorldObjectComp worldObjectComp)
        {
            Find.WindowStack.Add((Window)Activator.CreateInstance(EditWindow, worldObjectComp, this));
        }

        public virtual bool CanUseWith(WorldObject worldObject)
        {
            return true;
        }
    }
}
