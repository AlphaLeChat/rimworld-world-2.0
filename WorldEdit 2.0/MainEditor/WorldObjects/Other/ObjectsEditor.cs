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

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other
{
    public class ObjectsEditor : Editor
    {
        protected override Type WindowType => typeof(ObjectsEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F7;

        public override string EditorName => "WE_Settings_ObjectsEditorKey".Translate();

        private List<WorldEditWorldObject> worldEditWorldObjects = new List<WorldEditWorldObject>();
        public IEnumerable<WorldEditWorldObject> WorldEditWorldObjects => worldEditWorldObjects.AsEnumerable();

        public KeyCode DragAndDropKey => dragAndDropKey;
        private KeyCode dragAndDropKey;

        public KeyCode EditKey => editKey;
        private KeyCode editKey;

        private List<WorldEditWorldObjectComp> worldEditWorldObjectComps = new List<WorldEditWorldObjectComp>();
        public IEnumerable<WorldEditWorldObjectComp> WorldEditWorldObjectComps => worldEditWorldObjectComps.AsEnumerable();

        public ObjectsEditor()
        {

        }

        public void RegisterWorldEditObject(WorldEditWorldObject worldEditObject)
        {
            if (worldEditWorldObjects.Contains(worldEditObject))
                return;

            worldEditWorldObjects.Add(worldEditObject);
        }

        public void RegisterWorldEditWorldOjectComp(WorldEditWorldObjectComp worldEditWorldObjectComp)
        {
            if (worldEditWorldObjectComps.Contains(worldEditWorldObjectComp))
                return;

            worldEditWorldObjectComps.Add(worldEditWorldObjectComp);
        }

        public void DeleteAllObjects()
        {
            List<WorldObject> allObjects = new List<WorldObject>(Find.WorldObjects.AllWorldObjects.Where(stl =>
                                            !(stl is Settlement) &&
                                            stl.Faction != Faction.OfAncients && stl.Faction != Faction.OfInsects &&
                                            stl.Faction != Faction.OfMechanoids && stl.Faction != Faction.OfAncientsHostile &&
                                            stl.Faction != Faction.OfPlayer));
            foreach (var wObj in allObjects)
            {
                Find.WorldObjects.Remove(wObj);
            }
        }

        public override void DrawSettings(Rect inRect, Listing_Standard listing_Standard)
        {
            base.DrawSettings(inRect, listing_Standard);

            if (listing_Standard.ButtonText($"{EditorName} {"ObjectsEditor_DragAndDropKey".Translate()}: {dragAndDropKey}"))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    list.Add(new FloatMenuOption(code.ToString(), delegate
                    {
                        dragAndDropKey = code;

                        Messages.Message("WE_Settings_Key_Update".Translate(code.ToString()), MessageTypeDefOf.NeutralEvent, false);
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (listing_Standard.ButtonText($"{EditorName} {"ObjectsEditor_EditKey".Translate()}: {editKey}"))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    list.Add(new FloatMenuOption(code.ToString(), delegate
                    {
                        editKey = code;

                        Messages.Message("WE_Settings_Key_Update".Translate(code.ToString()), MessageTypeDefOf.NeutralEvent, false);
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref dragAndDropKey, "dragAndDropKey", KeyCode.Mouse2);
            Scribe_Values.Look(ref editKey, "editKey", KeyCode.Mouse1);
        }
    }
}
