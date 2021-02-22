using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.WorldFeatures;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other
{
    public class ObjectsEditorWindow : EditWindow
    {
        private ObjectsEditor objectsEditor;

        enum ObjectGroupBy : byte
        {
            None = 0,
            Def,
            Faction
        }
        private ObjectGroupBy groupBy = ObjectGroupBy.None;
        private SortWorldObjectBy sortWorldObjectBy = SortWorldObjectBy.ABC;

        public override Vector2 InitialSize => new Vector2(345, 650);

        private List<WorldObject> worldObjects;
        private List<IGrouping<string, WorldObject>> worldObjectsGrouped;
        private List<WorldObject> worldObjectsSorted;

        private string searchBuff;
        private string oldSearchBuff;

        private Func<WorldObject, string> objectGroupFunc = delegate (WorldObject obj) { return obj.def.LabelCap; };
        private Func<WorldObject, string> objectFactionGroupFunc = delegate (WorldObject obj) { return obj.Faction.Name; };

        private int sliderSize = 0;

        private WorldObject selectedObject;

        private Vector2 objectsScrollPosition = Vector2.zero;

        private WorldEditWorldObject createWorldObjectType;
        private bool createObjectClick = false;
        private WorldObject movedObject;

        private Texture2D mouseObjectTexture;

        public ObjectsEditorWindow(ObjectsEditor editor)
        {
            objectsEditor = editor;
            resizeable = false;

            shadowAlpha = 0f;

            mouseObjectTexture = ContentFinder<Texture2D>.Get("World/WorldObjects/Expanding/Town");
        }

        public override void PostOpen()
        {
            base.PostOpen();

            searchBuff = string.Empty;
            oldSearchBuff = string.Empty;

            RecacheObjects();
        }

        public override void PostClose()
        {
            base.PostClose();

            createObjectClick = false;
            movedObject = null;
        }

        public void RecacheObjects()
        {
            worldObjects = Find.WorldObjects.AllWorldObjects.Where(obj => !(obj is Settlement) && (string.IsNullOrEmpty(searchBuff) || (!string.IsNullOrEmpty(searchBuff) && obj.Label.Contains(searchBuff)))).ToList();
            worldObjectsSorted = WorldObjectsUtils.SortWorldObjectsBy(worldObjects, sortWorldObjectBy).ToList();

            switch (groupBy)
            {
                case ObjectGroupBy.Def:
                    {
                        worldObjectsGrouped = worldObjectsSorted.GroupBy(gKey => objectGroupFunc(gKey)).ToList();
                        break;
                    }
                case ObjectGroupBy.Faction:
                {
                        worldObjectsGrouped = worldObjectsSorted.GroupBy(gKey => objectFactionGroupFunc(gKey)).ToList();
                    break;
                }
            }

            if (groupBy == ObjectGroupBy.None)
            {
                sliderSize = worldObjects.Count * 22;
            }
            else
            {
                sliderSize = worldObjectsGrouped.Count * 20;
                foreach (var gValue in worldObjectsGrouped)
                {
                    sliderSize += gValue.Count() * 22;
                }
            }
            
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (createObjectClick)
                return;

            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 320, 30), Translator.Translate("ObjectsEditorWindow_ObjectsList"));
            Text.Anchor = TextAnchor.UpperLeft;

            searchBuff = Widgets.TextField(new Rect(0, 24, 320, 20), searchBuff);
            if (searchBuff != oldSearchBuff)
            {
                oldSearchBuff = searchBuff;

                RecacheObjects();
            }

            Rect scrollRectFact = new Rect(10, 50, 320, 420);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, sliderSize);
            Widgets.BeginScrollView(scrollRectFact, ref objectsScrollPosition, scrollVertRectFact);
            int x = 0;
            if (groupBy == ObjectGroupBy.None)
            {
                foreach (var wObject in worldObjectsSorted)
                {
                    var buttonRect = new Rect(0, x, 305, 20);

                    if (Widgets.ButtonText(buttonRect, wObject.LabelCap))
                    {
                        SelectNewObject(wObject);
                    }

                    if (selectedObject == wObject)
                    {
                        Widgets.DrawBox(buttonRect, 2);
                    }

                    x += 22;
                }

            }
            else
            {
                foreach (var wObjectGroup in worldObjectsGrouped)
                {
                    Widgets.Label(new Rect(0, x, 305, 20), wObjectGroup.Key);

                    x += 20;

                    foreach (var wObject in wObjectGroup)
                    {
                        var settlButtonRect = new Rect(15, x, 290, 20);

                        if (Widgets.ButtonText(settlButtonRect, wObject.LabelCap))
                        {
                            SelectNewObject(wObject);
                        }

                        if (selectedObject == wObject)
                        {
                            Widgets.DrawBox(settlButtonRect, 2);
                        }

                        x += 22;
                    }
                }
            }

            if (selectedObject != null && Event.current.clickCount > 1)
            {
                CameraJumper.TryJumpAndSelect(new GlobalTargetInfo(selectedObject));
            }

            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(10, 480, 300, 20), Translator.Translate("ObjectsEditorWindow_AddNewObject")))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (WorldEditWorldObject manageObject in objectsEditor.WorldEditWorldObjects)
                {
                    list.Add(new FloatMenuOption(manageObject.ObjectName, () =>
                    {
                        createWorldObjectType = manageObject;

                        createObjectClick = true;
                        closeOnCancel = false;

                        Messages.Message("ObjectsEditorWindow_SelectNewPlace".Translate(), MessageTypeDefOf.NeutralEvent, false);
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (Widgets.ButtonText(new Rect(10, 500, 300, 20), Translator.Translate("ObjectsEditorWindow_DeleteSelectedObject")))
            {
                DeleteObject(selectedObject);
            }

            if (Widgets.ButtonText(new Rect(10, 520, 300, 20), Translator.Translate("ObjectsEditorWindow_DeleteAllObjects")))
            {
                DeleteAllObjects();
            }

            if (Widgets.ButtonText(new Rect(10, 560, 300, 20), "SettlementEditorWindow_Group".Translate(TranslateObjectGroupLabel(groupBy))))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (ObjectGroupBy groupParam in Enum.GetValues(typeof(ObjectGroupBy)))
                {
                    list.Add(new FloatMenuOption(TranslateObjectGroupLabel(groupParam), () =>
                    {
                        groupBy = groupParam;

                        RecacheObjects();
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }
            if (Widgets.ButtonText(new Rect(10, 580, 300, 20), "SettlementEditorWindow_Sort".Translate(sortWorldObjectBy.TranslateSortWorldObjectBy())))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (SortWorldObjectBy groupParam in Enum.GetValues(typeof(SortWorldObjectBy)))
                {
                    list.Add(new FloatMenuOption(groupParam.TranslateSortWorldObjectBy(), () =>
                    {
                        sortWorldObjectBy = groupParam;

                        RecacheObjects();
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }
        }

        public override void WindowOnGUI()
        {
            if (movedObject != null)
            {
                GenUI.DrawMouseAttachment(movedObject.ExpandingIcon);
                return;
            }

            if (createObjectClick)
            {
                GenUI.DrawMouseAttachment(createWorldObjectType.WorldObjectEditorDef.ExpandingIconTexture ?? mouseObjectTexture);
                return;
            }

            base.WindowOnGUI();
        }

        public override void WindowUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                int clickTile = GenWorld.MouseTile();
                if (clickTile >= 0)
                {
                    if (createObjectClick)
                    {
                        if (!Find.WorldObjects.AnyWorldObjectAt(clickTile))
                        {
                            createObjectClick = false;
                            closeOnCancel = true;

                            AddNewObject(clickTile);
                        }
                    }
                    else
                    {
                        var tileObject = Find.WorldObjects.ObjectsAt(clickTile).FirstOrDefault();

                        if (tileObject != null && tileObject != selectedObject)
                        {
                            SelectNewObject(tileObject);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(objectsEditor.DragAndDropKey))
            {
                int mouseTile = GenWorld.MouseTile();
                if (mouseTile >= 0)
                {
                    var objectAt = Find.WorldObjects.ObjectsAt(mouseTile).FirstOrDefault();
                    if (movedObject == null && objectAt != null)
                    {
                        closeOnCancel = false;
                        createObjectClick = false;

                        movedObject = objectAt;
                    }
                    else if (movedObject != null && objectAt == null)
                    {
                        movedObject.Tile = mouseTile;

                        closeOnCancel = true;
                        movedObject = null;
                    }
                }
            }

            if (Input.GetKeyDown(objectsEditor.EditKey))
            {
                int mouseTile = GenWorld.MouseTile();
                if (mouseTile >= 0)
                {
                    var objectAt = Find.WorldObjects.ObjectsAt(mouseTile).FirstOrDefault();
                    if (objectAt != null)
                    {
                        var editor = objectsEditor.WorldEditWorldObjects.FirstOrDefault(e => e.WorldObjectEditorDef == objectAt.def);
                        if(editor != null)
                        {
                            editor.CreateView(objectAt);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (createObjectClick)
                {
                    createObjectClick = false;
                }
                if (movedObject != null)
                {
                    movedObject = null;
                }

                closeOnCancel = true;
            }
        }

        private void AddNewObject(int tile)
        {
            if(createWorldObjectType != null)
            {
                WorldObject newWorldObject = WorldObjectMaker.MakeWorldObject(createWorldObjectType.WorldObjectEditorDef);
                newWorldObject.Tile = tile;
                newWorldObject.SetFaction(Find.FactionManager.AllFactionsVisible.First());

                createWorldObjectType.WorldObjectCreated(newWorldObject);

                createWorldObjectType.CreateView(newWorldObject);

                Find.WorldObjects.Add(newWorldObject);
            }
        }

        private string TranslateObjectGroupLabel(ObjectGroupBy groupBy)
        {
            return $"ObjectGroupBy_{groupBy}".Translate();
        }

        private void SelectNewObject(WorldObject worldObject)
        {
            selectedObject = worldObject;
        }

        private void DeleteObject(WorldObject worldObject)
        {
            if (selectedObject == null)
                return;

            Find.WorldObjects.Remove(worldObject);

            RecacheObjects();
        }

        private void DeleteAllObjects()
        {
            objectsEditor.DeleteAllObjects();

            RecacheObjects();

            Messages.Message("ObjectsEditorWindow_AllObjectsDeleted".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }
    }
}
