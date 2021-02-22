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
using WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects;
using WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other
{
    public class WorldEditWorldObjectWindow : Window
    {
        protected WorldEditWorldObject editor;
        protected WorldObject worldObject;

        public override Vector2 InitialSize => new Vector2(400, 310);

        protected Faction setFaction;

        protected List<WorldEditWorldObjectComp> worldEditWorldObjectComps;

        protected virtual string Title { get; }

        public WorldEditWorldObjectWindow(WorldObject worldObject, WorldEditWorldObject editor)
        {
            this.editor = editor;
            this.worldObject = worldObject;

            setFaction = worldObject.Faction;

            worldEditWorldObjectComps = WorldEditWorldObjectCompUtility.GetWorldObjectCompsFor(worldObject);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 364, 20), Title);
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.Label(new Rect(0, 30, 100, 25), Translator.Translate("WorldEditWorldObject_FactionOwner"));
            if(Widgets.ButtonText(new Rect(105, 30, 260, 25), setFaction.Name))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach(var faction in Find.FactionManager.AllFactionsListForReading)
                {
                    list.Add(new FloatMenuOption(faction.Name, delegate
                    {
                        setFaction = faction;
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }

            Rect compsRect = new Rect(0, 60, 364, 20);
            WorldEditWorldObjectCompUtility.DrawWorldEditWorldObjectComps(compsRect, worldEditWorldObjectComps, worldObject);

            if (Widgets.ButtonText(new Rect(0, 240, 364, 20), Translator.Translate("WorldEditWorldObject_SaveOrCreate")))
            {
                SaveObject();
            }
        }

        protected virtual void SaveObject()
        {
            worldObject.SetFaction(setFaction);

            Messages.Message("WorldEditWorldObject_Saved".Translate(), MessageTypeDefOf.NeutralEvent, false);

            WorldEditor.WorldEditorInstance.GetEditor<ObjectsEditor>().GetEditorWindow<ObjectsEditorWindow>().RecacheObjects();

            Close();
        }
    }
}
