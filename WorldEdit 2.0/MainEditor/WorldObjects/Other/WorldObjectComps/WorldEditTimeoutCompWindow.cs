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
    public class WorldEditTimeoutCompWindow : WorldEditCompWindow
    {
        private TimeoutComp timeoutComp;

        public override Vector2 InitialSize => new Vector2(325, 118);

        private string timeString;

        public WorldEditTimeoutCompWindow(WorldObjectComp worldObjectComp, WorldEditWorldObjectComp worldEditComp) : base(worldObjectComp, worldEditComp)
        {
            timeoutComp = (TimeoutComp)worldObjectComp;

            timeString = (timeoutComp.TicksLeft / 60000).ToString();
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawHeader(ref inRect);

            Widgets.Label(new Rect(0, inRect.y, 230, 20), "WorldEditTimeoutCompWindow_Time".Translate());
            timeString = Widgets.TextField(new Rect(235, inRect.y, 50, 20), timeString);

            inRect.y += 25;

            DrawBottom(ref inRect);
        }

        protected override bool AcceptChanges()
        {
            if(!int.TryParse(timeString, out int time))
            {
                Messages.Message("WorldEditTimeoutCompWindow_EnterCorrectTime".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return false;
            }

            if (time <= 0)
                return true;

            timeoutComp.StartTimeout(time * 60000);

            return true;
        }
    }
}
