using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public abstract class WorldEditWorldObject
    {
        public abstract string ObjectName { get; }

        protected abstract Type Window { get; }

        public abstract WorldObjectDef WorldObjectEditorDef { get; }

        public WorldEditWorldObject()
        {

        }

        public virtual void CreateView(WorldObject worldObject)
        {
            Find.WindowStack.Add((Window)Activator.CreateInstance(Window, worldObject, this));
        }

        public virtual void WorldObjectCreated(WorldObject worldObject)
        {

        }
    }
}
