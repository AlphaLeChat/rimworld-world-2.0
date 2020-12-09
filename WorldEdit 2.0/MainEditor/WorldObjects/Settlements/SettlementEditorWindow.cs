using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Settlements
{
    class SettlementEditorWindow : EditWindow
    {
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

            avaliableFactions = Find.FactionManager.AllFactions.Where(faction =>
                                        !faction.IsPlayer && faction != Faction.OfAncients && faction != Faction.OfAncientsHostile &&
                                        faction != Faction.OfInsects && faction != Faction.OfMechanoids).ToList();
        }

        public override void PostClose()
        {
            base.PostClose();

            createSettlementClick = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (createSettlementClick)
                return;

            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, 350, 30), Translator.Translate("SettlementEditorWindow_SettlementsList"));
            Text.Anchor = TextAnchor.UpperLeft;

            int size = Find.WorldObjects.Settlements.Count * 30;
            Rect scrollRectFact = new Rect(10, 50, 330, 490);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, size);
            Widgets.BeginScrollView(scrollRectFact, ref settlementsScrollPosition, scrollVertRectFact);
            int x = 0;
            foreach (var settlement in Find.WorldObjects.Settlements)
            {
                if (Widgets.ButtonText(new Rect(0, x, 305, 20), settlement.Name))
                {
                    SelectNewSettlement(settlement);
                }
                x += 22;
            }
            Widgets.EndScrollView();


            if (Widgets.ButtonText(new Rect(10, 520, 300, 20), Translator.Translate("SettlementEditorWindow_AddNewSettlement")))
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

            if (Widgets.ButtonText(new Rect(10, 540, 300, 20), Translator.Translate("SettlementEditorWindow_DeleteSelectedSettlement")))
            {
                DeleteSettlement(selectedSettlement);
            }

            if (Widgets.ButtonText(new Rect(10, 560, 300, 20), Translator.Translate("SettlementEditorWindow_DeleteAllSettlements")))
            {
                DeleteAllSettlements();
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

        private void DeleteAllSettlements()
        {
            List<Settlement> allSettlements = new List<Settlement>(Find.WorldObjects.Settlements);
            foreach(var settlement in allSettlements)
            {
                Find.WorldObjects.Remove(settlement);
            }

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
                            SelectNewSettlement(selectedSettlement);
                        }
                    }
                }
            }

            if (createSettlementClick && Input.GetKeyDown(KeyCode.Escape))
            {
                createSettlementClick = false;
                closeOnCancel = true;
            }
        }

        private void AddNewSettlement(int tile, bool select = false)
        {
            WorldObject obj = (WorldObject)Activator.CreateInstance(WorldObjectDefOf.Settlement.worldObjectClass);
            obj.def = WorldObjectDefOf.Settlement;
            obj.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
            obj.creationGameTicks = Find.TickManager.TicksGame;
            obj.PostMake();

            Settlement settlement = (Settlement)obj;
            
            if(fixedFactionOnSpawn == null)
                settlement.SetFaction(avaliableFactions.RandomElement());
            else
                settlement.SetFaction(fixedFactionOnSpawn);
            settlement.Tile = tile;
            settlement.Name = "New settlement " + obj.ID;

            Find.WorldObjects.Add(settlement);

            if(select)
            {
                SelectNewSettlement(settlement);
            }
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
        }

    }
}
