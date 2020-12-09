using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.WorldFeatures
{
    public class WorldFeatureEditorWindow : EditWindow
    {
        private WorldFeatureEditor worldFeatureEditor;

        public override Vector2 InitialSize => new Vector2(660, 460);

        private Vector2 scrollPosition = Vector2.zero;

        private WorldFeature selectedFeature = null;

        private string featureName = string.Empty;
        private float rotate = 0f;
        private string rotateBuff = string.Empty;
        private float maxLength = 10f;
        private string maxLengthBuff = "1";

        private bool createFeatureClick = false;

        public WorldFeatureEditorWindow(WorldFeatureEditor editor)
        {
            worldFeatureEditor = editor;
            resizeable = false;

            shadowAlpha = 0f;
        }

        public override void PostClose()
        {
            base.PostClose();

            createFeatureClick = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (createFeatureClick)
                return;

            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(10, 0, 310, 20), Translator.Translate("WorldFeatureEditorWindow_WorldPrintsTitle"));
            Text.Anchor = TextAnchor.UpperLeft;

            int size1 = Find.WorldFeatures.features.Count * 22;
            Rect scrollRectFact = new Rect(0, 50, 300, 280);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, size1);
            Widgets.BeginScrollView(scrollRectFact, ref scrollPosition, scrollVertRectFact);
            int x = 0;
            foreach (var feat in Find.WorldFeatures.features)
            {
                if (Widgets.ButtonText(new Rect(0, x, 290, 20), feat.name))
                {
                    SelectNewFeature(feat);
                }
                x += 22;
            }
            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(0, 340, 300, 20), Translator.Translate("WorldFeatureEditorWindow_CreateNewFeature")))
            {
                createFeatureClick = true;
                closeOnCancel = false;

                Messages.Message("WorldFeatureEditorWindow_SelectWorldFeaturePlace".Translate(), MessageTypeDefOf.NeutralEvent, false);
            }

            if (Widgets.ButtonText(new Rect(0, 365, 300, 20), Translator.Translate("WorldFeatureEditorWindow_DeleteFeature")))
            {
                DeleteFeature(selectedFeature);
            }

            if (Widgets.ButtonText(new Rect(0, 390, 300, 20), Translator.Translate("WorldFeatureEditorWindow_DeleteAllFeatures")))
            {
                DeleteAllFeatures();
            }

            Widgets.DrawLineVertical(305, 5, inRect.height - 10);

            if (selectedFeature != null)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(new Rect(310, 0, 320, 20), Translator.Translate("WorldFeatureEditorWindow_FeaturesInfoTitle"));
                Text.Anchor = TextAnchor.UpperLeft;

                Widgets.Label(new Rect(310, 25, 120, 20), Translator.Translate("WorldFeatureEditorWindow_WorldFeatureName"));
                featureName = Widgets.TextField(new Rect(410, 25, 215, 20), featureName);

                Widgets.Label(new Rect(310, 50, 120, 20), Translator.Translate("WorldFeatureEditorWindow_RotateFeature"));
                Widgets.TextFieldNumeric(new Rect(410, 50, 215, 20), ref rotate, ref rotateBuff, 0, 360);

                Widgets.Label(new Rect(310, 75, 120, 20), Translator.Translate("WorldFeatureEditorWindow_FeatureLengthMax"));
                Widgets.TextFieldNumeric(new Rect(410, 75, 215, 20), ref maxLength, ref maxLengthBuff, 10f, 10000f);

                if (Widgets.ButtonText(new Rect(310, 110, 345, 20), Translator.Translate("WorldFeatureEditorWindow_SaveWorldFeature")))
                {
                    SaveFeature();
                }
            }
        }

        public override void WindowOnGUI()
        {
            if(createFeatureClick)
            {
                GenUIUtils.DrawMouseAttachment("New text");
                return;
            }

            base.WindowOnGUI();
        }

        public override void WindowUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                int clickTile = GenWorld.MouseTile();
                if (clickTile >= 0)
                {
                    if (createFeatureClick)
                    {
                        createFeatureClick = false;
                        closeOnCancel = true;

                        AddNewFeature(clickTile, true);
                    }
                    else
                    {
                        var tileFeature = Find.WorldGrid[clickTile].feature;

                        if (tileFeature != null && tileFeature != selectedFeature)
                        {
                            SelectNewFeature(tileFeature);
                        }
                    }
                }
            }

            if(createFeatureClick && Input.GetKeyDown(KeyCode.Escape))
            {
                createFeatureClick = false;
                closeOnCancel = true;
            }
        }

        private void SaveFeature()
        {
            if (selectedFeature == null)
                return;

            selectedFeature.name = featureName;
            selectedFeature.drawAngle = rotate;
            selectedFeature.maxDrawSizeInTiles = maxLength;

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();

            Messages.Message("WorldFeatureEditorWindow_SaveWorldFeatureInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void DeleteAllFeatures()
        {
            WorldGrid grid = Find.WorldGrid;

            foreach (var feature in Find.WorldFeatures.features)
            {
                foreach (var tileID in feature.Tiles)
                {
                    grid[tileID].feature = null;
                }
            }

            Find.WorldFeatures.features.Clear();

            Find.WorldFeatures.textsCreated = false;

            Find.WorldFeatures.UpdateFeatures();

            selectedFeature = null;

            Messages.Message("WorldFeatureEditorWindow_DeleteAllFeaturesMessage".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void DeleteFeature(WorldFeature worldFeature)
        {
            if (worldFeature == null)
                return;

            WorldGrid grid = Find.WorldGrid;
            foreach (var t in worldFeature.Tiles)
            {
                if (grid[t].feature == worldFeature)
                    grid[t].feature = null;
            }

            Find.WorldFeatures.features.Remove(worldFeature);

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();

            selectedFeature = null;
        }

        private void AddNewFeature(bool select = false)
        {
            int tile = Find.WorldSelector.selectedTile;
            if (tile < 0)
            {
                Messages.Message("WorldFeatureEditorWindow_WrongTile".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            WorldFeature worldFeature = new WorldFeature
            {
                uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID(),
                def = DefDatabase<FeatureDef>.GetRandom(),
                name = "New feature"
            };
            WorldGrid worldGrid = Find.WorldGrid;
            worldGrid[tile].feature = worldFeature;

            worldFeature.drawCenter = worldGrid.GetTileCenter(tile);
            worldFeature.maxDrawSizeInTiles = 10f;
            worldFeature.drawAngle = 0f;

            Find.WorldFeatures.features.Add(worldFeature);

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();

            if (select)
            {
                SelectNewFeature(worldFeature);
            }

            Messages.Message("WorldFeatureEditorWindow_Created".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void AddNewFeature(int tile, bool select = false)
        {
            if (tile < 0)
            {
                Messages.Message("WorldFeatureEditorWindow_WrongTile".Translate(), MessageTypeDefOf.NeutralEvent, false);
                return;
            }

            WorldFeature worldFeature = new WorldFeature
            {
                uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID(),
                def = DefDatabase<FeatureDef>.GetRandom(),
                name = "New feature"
            };
            WorldGrid worldGrid = Find.WorldGrid;
            worldGrid[tile].feature = worldFeature;

            worldFeature.drawCenter = worldGrid.GetTileCenter(tile);
            worldFeature.maxDrawSizeInTiles = 10f;
            worldFeature.drawAngle = 0f;

            Find.WorldFeatures.features.Add(worldFeature);

            Find.WorldFeatures.textsCreated = false;
            Find.WorldFeatures.UpdateFeatures();

            if(select)
            {
                SelectNewFeature(worldFeature);
            }

            Messages.Message("WorldFeatureEditorWindow_Created".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void SelectNewFeature(WorldFeature worldFeature)
        {
            selectedFeature = worldFeature;

            if (selectedFeature != null)
            {
                featureName = selectedFeature.name;
                rotate = selectedFeature.drawAngle;
                rotateBuff = $"{selectedFeature.drawAngle}";

                maxLength = selectedFeature.maxDrawSizeInTiles;
                maxLengthBuff = $"{selectedFeature.maxDrawSizeInTiles}";
            }
        }
    }
}
