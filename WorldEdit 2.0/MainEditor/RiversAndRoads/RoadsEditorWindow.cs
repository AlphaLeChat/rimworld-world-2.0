using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Tiles;

namespace WorldEdit_2_0.MainEditor.RiversAndRoads
{
    public class RoadsEditorWindow : EditWindow
    {
        public override Vector2 InitialSize => new Vector2(460, 500);

        private Vector2 riverScrollPosition = Vector2.zero;
        private Vector2 mainScrollPosition = Vector2.zero;
        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        private RoadsEditor roadsEditor;

        private RoadDef selectedRoad;

        private int startRoadTile;
        private int endRoadTile;

        private WorldLayer roadsLayer => roadsEditor.RoadsLayer;

        private bool removeMode;

        private List<RoadDef> avaliableRoads => roadsEditor.AvaliableRoads;

        public RoadsEditorWindow(RoadsEditor editor)
        {
            roadsEditor = editor;
            resizeable = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Rect mainScrollRect = new Rect(0, 20, inRect.width, inRect.height);
            Rect mainScrollVertRect = new Rect(0, 0, mainScrollRect.x, inRect.width);
            Widgets.BeginScrollView(mainScrollRect, ref mainScrollPosition, mainScrollVertRect);

            if (Widgets.ButtonText(new Rect(10, 10, 420, 20), Translator.Translate("RoadsEditorWindow_RemoveAllRoads")))
            {
                roadsEditor.RemoveAllRoads();
            }

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(10, 35, 420, 20), "RoadsEditorWindow_RoadsList".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            if (Widgets.ButtonText(new Rect(10, 53, 420, 20), Translator.Translate("NoText")))
            {
                selectedRoad = null;
                Messages.Message("RoadsEditorWindow_SelectedRoad".Translate(Translator.Translate("NoText")), MessageTypeDefOf.NeutralEvent, false);
            }
            int yButtonPos = 5;
            Rect riverScrollRect = new Rect(10, 75, 420, 170);
            Rect riverScrollVertRect = new Rect(0, 0, riverScrollRect.x, avaliableRoads.Count * 25);
            Widgets.BeginScrollView(riverScrollRect, ref riverScrollPosition, riverScrollVertRect);
            foreach (var road in avaliableRoads)
            {
                if (Widgets.ButtonText(new Rect(5, yButtonPos, 410, 20), road.label))
                {
                    selectedRoad = road;
                    Messages.Message("RoadsEditorWindow_SelectedRoad".Translate(selectedRoad.LabelCap), MessageTypeDefOf.NeutralEvent, false);
                }
                yButtonPos += 22;
            }
            Widgets.EndScrollView();
            yButtonPos = 250;

            if (Widgets.RadioButtonLabeled(new Rect(10, yButtonPos, 420, 20), Translator.Translate("RoadsEditorWindow_RemoveMode"), removeMode))
            {
                removeMode = !removeMode;
                Messages.Message("RoadsEditorWindow_RemoveModeChanged".Translate(), MessageTypeDefOf.NeutralEvent, false);
            }

            Widgets.EndScrollView();
        }

        public override void WindowUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                startRoadTile = GenWorld.MouseTile();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                endRoadTile = GenWorld.MouseTile();

                if (startRoadTile >= 0 && endRoadTile >= 0)
                {
                    if (!removeMode && selectedRoad != null)
                        roadsEditor.CreateRoad(startRoadTile, endRoadTile, selectedRoad);
                    else if (removeMode)
                        roadsEditor.RemoveRoad(startRoadTile, endRoadTile);

                }
            }
        }
    }
}
