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

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps
{
    public class WorldEditCompWindow: Window 
    {
        protected WorldObjectComp comp;
        protected WorldEditWorldObjectComp worldEditComp;

        public override Vector2 InitialSize => new Vector2(500, 400);

        public WorldEditCompWindow(WorldObjectComp worldObjectComp, WorldEditWorldObjectComp worldEditComp)
        {
            comp = worldObjectComp;
            this.worldEditComp = worldEditComp;

            resizeable = false;

            doCloseX = true;
        }

        public override void DoWindowContents(Rect inRect)
        {

        }

        protected void DrawHeader(ref Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, inRect.width, 20), worldEditComp.GuiCompName);
            Text.Anchor = TextAnchor.UpperLeft;

            inRect.y += 25;
        }

        protected virtual bool AcceptChanges()
        {
            return true;
        }

        protected void DrawBottom(ref Rect inRect)
        {
            if (Widgets.ButtonText(new Rect(0, inRect.y, inRect.width, 20), "WorldEditCompWindow_AcceptChanges".Translate()))
            {
                if (AcceptChanges())
                {
                    Messages.Message("WorldEditWorldObject_Saved".Translate(), MessageTypeDefOf.NeutralEvent, false);

                    Close();
                }
            }
        }
    }
}
