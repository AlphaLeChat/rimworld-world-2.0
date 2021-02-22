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
    public class WorldEditWorldObjectCompUtility
    {
        private static Vector2 compsScroll = Vector2.zero;

        private static int compsSliderSize = 150;

        public static List<WorldEditWorldObjectComp> GetWorldObjectCompsFor(WorldObject worldObject)
        {
            List<WorldEditWorldObjectComp> worldObjectComps = new List<WorldEditWorldObjectComp>();
            ObjectsEditor objectsEditor = WorldEditor.WorldEditorInstance.GetEditor<ObjectsEditor>();

            if(worldObject.AllComps != null)
            {
                for(int i = 0; i < worldObject.AllComps.Count; i++)
                {
                    var compAdapter = objectsEditor.WorldEditWorldObjectComps.FirstOrDefault(cmp => cmp.WorldObjectCompType == worldObject.AllComps[i].GetType());
                    if(compAdapter != null && compAdapter.CanUseWith(worldObject))
                    {
                        worldObjectComps.Add(compAdapter);
                    }
                }
            }

            return worldObjectComps;
        }

        public static WorldEditWorldObjectComp GetWorldObjectCompFor<T>() where T : WorldObjectComp
        {
            ObjectsEditor objectsEditor = WorldEditor.WorldEditorInstance.GetEditor<ObjectsEditor>();

            return objectsEditor.WorldEditWorldObjectComps.FirstOrDefault(cmp => cmp.WorldObjectCompType == typeof(T));
        }

        public static void DrawWorldEditWorldObjectComps(Rect inRect, List<WorldEditWorldObjectComp> comps, WorldObject coreObject)
        {
            Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 20), Translator.Translate("WorldEditWorldObjectCompUtility_DrawWorldEditWorldObjectComps"));

            int compsSize = comps.Count * 25;

            Rect scrollRectFact = new Rect(inRect.x, inRect.y + 25, inRect.width, compsSliderSize);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, compsSize);
            Widgets.BeginScrollView(scrollRectFact, ref compsScroll, scrollVertRectFact);
            int yButtonPos = 0;
            float buttonRectWidth = inRect.width - 10;
            foreach (var comp in comps)
            {
                var buttonRect = new Rect(0, yButtonPos, buttonRectWidth, 25);

                DrawWorldEditWorldObjectComp(buttonRect, comp, coreObject);

                yButtonPos += 25;
            }
            Widgets.EndScrollView();
        }

        public static void DrawWorldEditWorldObjectComp(Rect rect, WorldEditWorldObjectComp worldEditWorldObjectComp, WorldObject coreObject)
        {
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - 130, rect.height);
            Widgets.Label(labelRect, worldEditWorldObjectComp.GuiCompName);
            Rect buttonRect = new Rect(rect.width - 140, rect.y, 130, rect.height);
            if(Widgets.ButtonText(buttonRect, "WorldEditWorldObjectComp_OpenSettings".Translate()))
            {
                if(coreObject != null)
                {
                    var comp = coreObject.GetComponent(worldEditWorldObjectComp.WorldObjectCompType);
                    if(comp != null)
                    {
                        worldEditWorldObjectComp.Edit(comp);
                    }
                }
            }

            if(Mouse.IsOver(labelRect))
                TooltipHandler.TipRegion(labelRect, worldEditWorldObjectComp.GuiDescription);
        }
    }
}
