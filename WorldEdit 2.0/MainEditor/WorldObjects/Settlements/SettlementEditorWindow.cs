using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.WorldFeatures;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Settlements
{
    public class SettlementEditorWindow : EditWindow
    {
        enum SettlementGroupBy : byte
        {
            None = 0,
            FactionName,
            FactionDef
        }

        private SettlementEditor settlementEditor;

        private Settlement selectedSettlement;

        private Vector2 settlementsScrollPosition = Vector2.zero;

        public override Vector2 InitialSize => new Vector2(1000, 650);

        private Faction fixedFactionOnSpawn = null;
        private string tmpSettlementName;
        private Faction tmpFaction;

        private bool createSettlementClick = false;

        private Texture2D mouseSettlementTexture;

        private List<Faction> avaliableFactions;

        private FactionManager rimFactionManager;

        private string searchBuff;
        private string oldSearchBuff;

        private List<Settlement> settlements;
        private List<IGrouping<string, Settlement>> settlementsGrouped;
        private List<Settlement> settlementsSorted;

        private Func<Settlement, string> factionGroupFunc = delegate (Settlement settl) { return settl.Faction.Name; };
        private Func<Settlement, string> factionDefGroupFunc = delegate (Settlement settl) { return settl.Faction.def.LabelCap; };

        private int sliderSize = 0;
        private SettlementGroupBy groupBy = SettlementGroupBy.None;
        private SortWorldObjectBy sortWorldObjectBy = SortWorldObjectBy.ABC;

        private Settlement movedSettlement;

        public SettlementEditorWindow(SettlementEditor editor)
        {
            settlementEditor = editor;
            resizeable = false;

            shadowAlpha = 0f;

            mouseSettlementTexture = ContentFinder<Texture2D>.Get("World/WorldObjects/Expanding/Town");

            rimFactionManager = Find.FactionManager;
        }

        public override void PostOpen()
        {
            base.PostOpen();

            searchBuff = string.Empty;
            oldSearchBuff = string.Empty;

            avaliableFactions = Find.FactionManager.AllFactions.Where(faction =>
                                        !faction.IsPlayer && faction != Faction.OfAncients && faction != Faction.OfAncientsHostile &&
                                        faction != Faction.OfInsects && faction != Faction.OfMechanoids).ToList();

            RecacheSettlements();
        }

        public override void PostClose()
        {
            base.PostClose();

            createSettlementClick = false;
            movedSettlement = null;
        }

        private void RecacheSettlements()
        {
            settlements = Find.WorldObjects.Settlements.Where(settl => string.IsNullOrEmpty(searchBuff) || (!string.IsNullOrEmpty(searchBuff) && settl.Name.Contains(searchBuff))).ToList();
            settlementsSorted = WorldObjectsUtils.SortWorldObjectsBy(settlements, sortWorldObjectBy).ToList();

            switch (groupBy)
            {
                case SettlementGroupBy.FactionName:
                    {
                        settlementsGrouped = settlementsSorted.GroupBy(gKey => factionGroupFunc(gKey)).ToList();
                        break;
                    }
                case SettlementGroupBy.FactionDef:
                    {
                        settlementsGrouped = settlementsSorted.GroupBy(gKey => factionDefGroupFunc(gKey)).ToList();
                        break;
                    }
            }

            if (groupBy == SettlementGroupBy.None)
            {
                sliderSize = settlements.Count * 22;
            }
            else
            {
                sliderSize = settlementsGrouped.Count * 20;
                foreach (var gValue in settlementsGrouped)
                {
                    sliderSize += gValue.Count() * 22;
                }
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (createSettlementClick)
                return;

            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 350, 30), Translator.Translate("SettlementEditorWindow_SettlementsList"));
            Text.Anchor = TextAnchor.UpperLeft;

            searchBuff = Widgets.TextField(new Rect(0, 24, 320, 20), searchBuff);
            if (searchBuff != oldSearchBuff)
            {
                oldSearchBuff = searchBuff;

                RecacheSettlements();
            }

            Rect scrollRectFact = new Rect(10, 50, 320, 420);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, sliderSize);
            Widgets.BeginScrollView(scrollRectFact, ref settlementsScrollPosition, scrollVertRectFact);
            int x = 0;
            if (groupBy == SettlementGroupBy.None)
            {
                foreach (var settlement in settlementsSorted)
                {
                    var settlButtonRect = new Rect(0, x, 305, 20);

                    if (Widgets.ButtonText(settlButtonRect, settlement.Name))
                    {
                        SelectNewSettlement(settlement);
                    }

                    if (selectedSettlement == settlement)
                    {
                        Widgets.DrawBox(settlButtonRect, 2);
                    }

                    x += 22;
                }

            }
            else
            {
                foreach (var settlementGroup in settlementsGrouped)
                {
                    Widgets.Label(new Rect(0, x, 305, 20), settlementGroup.Key);

                    x += 20;

                    foreach (var settlement in settlementGroup)
                    {
                        var settlButtonRect = new Rect(15, x, 290, 20);

                        if (Widgets.ButtonText(settlButtonRect, settlement.Name))
                        {
                            SelectNewSettlement(settlement);
                        }

                        if (selectedSettlement == settlement)
                        {
                            Widgets.DrawBox(settlButtonRect, 2);
                        }

                        x += 22;
                    }
                }
            }

            if(selectedSettlement != null && Event.current.clickCount > 1)
            {
                CameraJumper.TryJumpAndSelect(new GlobalTargetInfo(selectedSettlement));
            }

            Widgets.EndScrollView();


            if (Widgets.ButtonText(new Rect(10, 480, 300, 20), Translator.Translate("SettlementEditorWindow_AddNewSettlement")))
            {
                if (avaliableFactions.Count > 0)
                {
                    createSettlementClick = true;
                    closeOnCancel = false;

                    Messages.Message("SettlementEditorWindow_SelectSettlementPlace".Translate(), MessageTypeDefOf.NeutralEvent, false);
                }
                else
                {
                    Messages.Message("SettlementEditorWindow_NoAvaliableFactions".Translate(), MessageTypeDefOf.NeutralEvent, false);
                }
            }

            if (Widgets.ButtonText(new Rect(10, 500, 300, 20), Translator.Translate("SettlementEditorWindow_DeleteSelectedSettlement")))
            {
                DeleteSettlement(selectedSettlement);
            }

            if (Widgets.ButtonText(new Rect(10, 520, 300, 20), Translator.Translate("SettlementEditorWindow_DeleteAllSettlements")))
            {
                DeleteAllSettlements();
            }

            if (Widgets.ButtonText(new Rect(10, 560, 300, 20), "SettlementEditorWindow_Group".Translate(TranslateSettlementGroupLabel(groupBy))))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (SettlementGroupBy groupParam in Enum.GetValues(typeof(SettlementGroupBy)))
                {
                    list.Add(new FloatMenuOption(TranslateSettlementGroupLabel(groupParam), () =>
                    {
                        groupBy = groupParam;

                        RecacheSettlements();
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

                        RecacheSettlements();
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }
            if (Widgets.ButtonText(new Rect(10, 600, 300, 20), "SettlementEditorWindow_FixedFaction".Translate(fixedFactionOnSpawn == null ? "NoText".Translate().RawText : fixedFactionOnSpawn.Name)))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                list.Add(new FloatMenuOption("NoText".Translate(), () =>
                {
                    fixedFactionOnSpawn = null;
                }));

                foreach (var faction in avaliableFactions)
                {
                    list.Add(new FloatMenuOption(faction.Name, () =>
                    {
                        fixedFactionOnSpawn = faction;
                    }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }

            Widgets.DrawLineVertical(335, 5, inRect.height - 10);

            if (selectedSettlement != null)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(new Rect(340, 0, 660, 20), Translator.Translate("SettlementEditorWindow_SettlementInfoTitle"));
                Text.Anchor = TextAnchor.UpperLeft;

                if (Widgets.ButtonText(new Rect(350, 40, 600, 20), "SettlementEditorWindow_Def".Translate(tmpFaction.Name)))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();

                    foreach (var faction in avaliableFactions)
                    {
                        list.Add(new FloatMenuOption(faction.Name, () =>
                        {
                            tmpFaction = faction;
                        }));
                    }

                    Find.WindowStack.Add(new FloatMenu(list));
                }
                Widgets.DrawTextureFitted(new Rect(955, 35, 30, 30), tmpFaction.def.FactionIcon, 1.0f);

                Widgets.Label(new Rect(350, 78, 150, 30), Translator.Translate("SettlementEditorWindow_SettlementNameField"));
                tmpSettlementName = Widgets.TextField(new Rect(495, 75, 485, 30), tmpSettlementName);

                if (Widgets.ButtonText(new Rect(340, 600, 660, 20), Translator.Translate("SettlementEditorWindow_SaveSettlement")))
                {
                    SaveSettlement();
                }
            }
        }

        private string TranslateSettlementGroupLabel(SettlementGroupBy groupBy)
        {
            return $"SettlementGroupBy_{groupBy}".Translate();
        }

        private void DeleteAllSettlements()
        {
            settlementEditor.DeleteAllSettlements();

            RecacheSettlements();

            Messages.Message("SettlementEditorWindow_AllSettlementsDeleted".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void SaveSettlement()
        {
            selectedSettlement.Name = tmpSettlementName;
            selectedSettlement.SetFaction(tmpFaction);

            Messages.Message("SettlementEditorWindow_SaveSettlementInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        public override void WindowOnGUI()
        {
            if(movedSettlement != null)
            {
                GenUI.DrawMouseAttachment(movedSettlement.ExpandingIcon);
                return;
            }    

            if (createSettlementClick)
            {
                GenUI.DrawMouseAttachment(mouseSettlementTexture);
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
                    if (createSettlementClick)
                    {
                        if (!Find.WorldObjects.AnySettlementAt(clickTile))
                        {
                            createSettlementClick = false;
                            closeOnCancel = true;

                            AddNewSettlement(clickTile, true);
                        }
                    }
                    else
                    {
                        var tileSettlement = Find.WorldObjects.SettlementAt(clickTile);

                        if (tileSettlement != null && tileSettlement != selectedSettlement)
                        {
                            SelectNewSettlement(tileSettlement);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(settlementEditor.DragAndDropKey))
            {
                int mouseTile = GenWorld.MouseTile();
                if(mouseTile >= 0)
                {
                    var settlementAt = Find.WorldObjects.SettlementAt(mouseTile);
                    if(movedSettlement == null && settlementAt != null)
                    {
                        closeOnCancel = false;
                        createSettlementClick = false;

                        movedSettlement = settlementAt;
                    }else if(movedSettlement != null && settlementAt == null)
                    {
                        movedSettlement.Tile = mouseTile;

                        closeOnCancel = true;
                        movedSettlement = null;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (createSettlementClick)
                {
                    createSettlementClick = false;
                }
                if(movedSettlement != null)
                {
                    movedSettlement = null;
                }

                closeOnCancel = true;
            }
        }

        private void AddNewSettlement(int tile, bool select = false)
        {
            Settlement settlement = settlementEditor.AddNewSettlement(tile, fixedFactionOnSpawn ?? avaliableFactions.RandomElement());

            if(select)
            {
                SelectNewSettlement(settlement);
            }

            RecacheSettlements();
        }

        private void SelectNewSettlement(Settlement settlement)
        {
            selectedSettlement = settlement;

            tmpSettlementName = settlement.Name;

            tmpFaction = settlement.Faction;
        }

        private void DeleteSettlement(Settlement settlement)
        {
            if (selectedSettlement == null)
                return;

            Find.WorldObjects.Remove(settlement);

            RecacheSettlements();
        }

    }
}
