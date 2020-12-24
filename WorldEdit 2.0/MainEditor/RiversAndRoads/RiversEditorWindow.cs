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
    public class RiversEditorWindow : EditWindow
    {
        public override Vector2 InitialSize => new Vector2(460, 500);

        private Vector2 riverScrollPosition = Vector2.zero;
        private Vector2 mainScrollPosition = Vector2.zero;
        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        private RiversEditor riversEditor;

        private RiverDef selectedRiver;

        private int startRiverTile;
        private int endRiverTile;

        private WorldLayer riversLayer => riversEditor.RiversLayer;
        private List<LayerSubMesh> singleTileSubMesh => riversEditor.SingleTileSubMesh;

        private bool removeMode;

        private int edgeTile = -1;
        private List<int> edgeTiles = new List<int>();
        private bool setEdgeRiver = false;

        private List<RiverDef> avaliableRivers => riversEditor.AvaliableRivers;

        public RiversEditorWindow(RiversEditor editor)
        {
            riversEditor = editor;
            resizeable = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Rect mainScrollRect = new Rect(0, 20, inRect.width, inRect.height);
            Rect mainScrollVertRect = new Rect(0, 0, mainScrollRect.x, inRect.width);
            Widgets.BeginScrollView(mainScrollRect, ref mainScrollPosition, mainScrollVertRect);

            if (Widgets.ButtonText(new Rect(10, 10, 420, 20), Translator.Translate("RiversEditorWindow_RemoveAllRivers")))
            {
                riversEditor.RemoveAllRivers();
            }

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(10, 35, 420, 20), "RiversEditorWindow_RiverList".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            if (Widgets.ButtonText(new Rect(10, 53, 420, 20), Translator.Translate("NoText")))
            {
                selectedRiver = null;
                Messages.Message("RiversEditorWindow_SelectedRiver".Translate(Translator.Translate("NoText")), MessageTypeDefOf.NeutralEvent, false);
            }
            int yButtonPos = 5;
            Rect riverScrollRect = new Rect(10, 75, 420, 170);
            Rect riverScrollVertRect = new Rect(0, 0, riverScrollRect.x, avaliableRivers.Count * 25);
            Widgets.BeginScrollView(riverScrollRect, ref riverScrollPosition, riverScrollVertRect);
            foreach (var river in avaliableRivers)
            {
                if (Widgets.ButtonText(new Rect(5, yButtonPos, 410, 20), river.label))
                {
                    selectedRiver = river;
                    Messages.Message("RiversEditorWindow_SelectedRiver".Translate(selectedRiver.LabelCap), MessageTypeDefOf.NeutralEvent, false);
                }
                yButtonPos += 22;
            }
            Widgets.EndScrollView();
            yButtonPos = 250;

            if (Widgets.ButtonText(new Rect(10, yButtonPos, 420, 20), Translator.Translate("RiversEditorWindow_CreateSource")))
            {
                CreateSource();
            }

            yButtonPos += 30;

            if (Widgets.RadioButtonLabeled(new Rect(10, yButtonPos, 420, 20), Translator.Translate("RiversEditorWindow_RemoveMode"), removeMode))
            {
                removeMode = !removeMode;
                Messages.Message("RiversEditorWindow_RemoveModeChanged".Translate(), MessageTypeDefOf.NeutralEvent, false);
            }

            Widgets.EndScrollView();
        }

        private void CreateSource()
        {
            if (selectedRiver == null)
            {
                Messages.Message($"RiversEditorWindow_CreateSource_NoRiverDef".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            int tileID = Find.WorldSelector.selectedTile;
            List<int> oceansOrLakes = riversEditor.FindOceansOrLakesAround(tileID);

            worldEditor.WorldUpdater.RenderSingleTile(oceansOrLakes, WorldMaterials.SelectedTile, singleTileSubMesh);

            Messages.Message($"RiversEditorWindow_CreateSource_ClickInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);

            setEdgeRiver = true;
            edgeTiles = oceansOrLakes;
            edgeTile = tileID;
        }

        public override void WindowUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                startRiverTile = GenWorld.MouseTile();

                if (setEdgeRiver)
                {
                    setEdgeRiver = false;

                    if (edgeTiles.Count > 0 && selectedRiver != null)
                    {
                        if (edgeTiles.Contains(startRiverTile))
                        {
                            Find.WorldGrid.OverlayRiver(edgeTile, startRiverTile, selectedRiver);
                        }

                        edgeTiles.Clear();

                        worldEditor.WorldUpdater.UpdateLayer(riversLayer);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                endRiverTile = GenWorld.MouseTile();

                if (startRiverTile >= 0 && endRiverTile >= 0)
                {
                    if (!removeMode && selectedRiver != null)
                        riversEditor.CreateRiver(startRiverTile, endRiverTile, selectedRiver);
                    else if (removeMode)
                        riversEditor.RemoveRiver(startRiverTile, endRiverTile);

                }
            }
        }
    }
}
