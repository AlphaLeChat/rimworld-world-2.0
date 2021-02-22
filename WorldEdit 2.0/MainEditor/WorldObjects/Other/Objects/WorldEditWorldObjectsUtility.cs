using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects
{
    public class WorldEditWorldObjectsUtility
    {
        private static Vector2 factionScroll = Vector2.zero;

        public static void DrawSelectFactionList(Rect inRect, Action<Faction> setCallback, Faction getFaction, List<Faction> avaliableFactions)
        {
            Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 20), Translator.Translate("WorldEditWorldObject_FactionOwner"));

            int factionSize = avaliableFactions.Count * 25;

            Rect scrollRectFact = new Rect(inRect.x, inRect.y + 25, inRect.width, 200);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, factionSize);
            Widgets.BeginScrollView(scrollRectFact, ref factionScroll, scrollVertRectFact);
            int yButtonPos = 0;
            float buttonRectWidth = inRect.width - 10;
            foreach (var faction in avaliableFactions)
            {
                var buttonRect = new Rect(0, yButtonPos, buttonRectWidth, 20);

                if (Widgets.ButtonText(buttonRect, faction.Name))
                {
                    setCallback(faction);
                }

                if (getFaction == faction)
                {
                    Widgets.DrawBox(buttonRect, 2);
                }

                yButtonPos += 22;
            }
            Widgets.EndScrollView();
        }
    }
}
