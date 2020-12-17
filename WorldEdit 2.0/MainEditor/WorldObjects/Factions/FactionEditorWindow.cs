using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Factions
{
    public class FactionEditorWindow : EditWindow
    {
        private FactionEditor factionEditor;

        public override Vector2 InitialSize => new Vector2(920, 650);

        private List<FactionDef> avaliableFactionsDefs;
        private List<Faction> spawnedFactions;

        private Vector2 scrollPositionFactionList = Vector2.zero;
        private Vector2 scrollPositionDefsList = Vector2.zero;
        private Vector2 scrollPositionRelation = Vector2.zero;

        private Faction selectedFaction;

        private FactionManager rimFactionManager;

        //temp
        private FactionDef tmpSelectedFactionDef;
        private string tmpFactionName;
        private bool tmpIsDefeated;
        private string tmpLeaderName;

        private List<FactionRelation> newFactionRelation;
        private string[] newFactionGoodwillBuff;

        public FactionEditorWindow(FactionEditor editor)
        {
            factionEditor = editor;
            resizeable = false;

            rimFactionManager = Find.FactionManager;

            avaliableFactionsDefs = DefDatabase<FactionDef>.AllDefs.Where(f => !f.isPlayer &&
            rimFactionManager.OfMechanoids?.def != f && rimFactionManager.OfInsects?.def != f &&
            rimFactionManager.OfAncientsHostile?.def != f && rimFactionManager.OfAncients?.def != f).ToList();
        }

        public override void PostOpen()
        {
            base.PostOpen();

            RecacheFactions();
        }

        private void RecacheFactions()
        {
            spawnedFactions = rimFactionManager.AllFactionsListForReading.Where(fac => avaliableFactionsDefs.Contains(fac.def)).ToList();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(10, 0, 320, 20), Translator.Translate("FactionEditorWindow_FactionCreatorTitle"));
            Text.Anchor = TextAnchor.UpperLeft;

            int factionListSize = rimFactionManager.AllFactionsListForReading.Count * 25;
            Rect scrollRectFact = new Rect(0, 25, 320, 550);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, factionListSize);
            Widgets.BeginScrollView(scrollRectFact, ref scrollPositionFactionList, scrollVertRectFact);

            int yButtonPos = 5;
            if (Widgets.ButtonText(new Rect(0, yButtonPos, 300, 20), Translator.Translate("NoText")))
            {
                selectedFaction = null;
            }
            yButtonPos += 25;
            foreach (var spawnedFaction in spawnedFactions)
            {
                var spawnedFactionButtonRect = new Rect(0, yButtonPos, 300, 20);

                if (Widgets.ButtonText(spawnedFactionButtonRect, spawnedFaction.Name))
                {
                    SelectNewFaction(spawnedFaction);
                }

                if (selectedFaction == spawnedFaction)
                {
                    Widgets.DrawBox(spawnedFactionButtonRect, 2);
                }

                yButtonPos += 22;
            }
            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(0, 590, 300, 20), Translator.Translate("FactionEditorWindow_AddNewFaction")))
            {
                CreateFaction();
            }
            if (Widgets.ButtonText(new Rect(0, 610, 300, 20), Translator.Translate("FactionEditorWindow_DeleteSelectedFaction")))
            {
                DeleteFaction(selectedFaction);
            }

            Widgets.DrawLineVertical(330, 5, inRect.height - 10);

            //right side
            if (selectedFaction != null)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(new Rect(340, 0, 550, 23), Translator.Translate("FactionEditorWindow_FactionSettingsTitle"));
                Text.Anchor = TextAnchor.UpperLeft;

                if (Widgets.ButtonText(new Rect(340, 30, 520, 20), "FactionEditorWindow_Def".Translate(tmpSelectedFactionDef.LabelCap)))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();

                    foreach (var factionDef in avaliableFactionsDefs)
                    {
                        list.Add(new FloatMenuOption(factionDef.LabelCap, () =>
                        {
                            tmpSelectedFactionDef = factionDef;
                        }));
                    }

                    Find.WindowStack.Add(new FloatMenu(list));
                }
                Widgets.DrawTextureFitted(new Rect(870, 21, 30, 30), tmpSelectedFactionDef.FactionIcon, 1.0f);

                Widgets.Label(new Rect(340, 55, 150, 32), Translator.Translate("FactionEditorWindow_Name"));
                tmpFactionName = Widgets.TextField(new Rect(500, 55, 390, 30), tmpFactionName);

                Widgets.Label(new Rect(340, 90, 150, 30), Translator.Translate("FactionEditorWindow_Defeated"));
                if (tmpIsDefeated)
                {
                    if (Widgets.ButtonText(new Rect(495, 90, 390, 30), Translator.Translate("FactionEditorWindow_DefeatedYes")))
                    {
                        tmpIsDefeated = false;
                    }
                }
                else
                {
                    if (Widgets.ButtonText(new Rect(495, 90, 390, 30), Translator.Translate("FactionEditorWindow_DefeatedNo")))
                    {
                        tmpIsDefeated = true;
                    }
                }

                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(new Rect(340, 125, 550, 30), Translator.Translate("FactionEditorWindow_FactionRelative"));
                Text.Anchor = TextAnchor.UpperLeft;

                int y = 15;
                int boxY = 5;
                Rect scrollRectRel = new Rect(340, 165, 550, 360);
                Rect scrollVertRectRel = new Rect(0, 0, scrollRectRel.x, newFactionRelation.Count * 140);
                Widgets.DrawBox(new Rect(340, 165, 550, 360));
                Widgets.BeginScrollView(scrollRectRel, ref scrollPositionRelation, scrollVertRectRel);
                for (int i = 0; i < newFactionRelation.Count; i++)
                {
                    FactionRelation rel = newFactionRelation[i];

                    Widgets.DrawBox(new Rect(2, boxY, 530, 130));

                    Widgets.Label(new Rect(5, y, 490, 30), "FactionEditorWindow_FactionRelativeName".Translate(rel.other.Name));

                    y += 35;
                    Widgets.Label(new Rect(5, y, 142, 30), Translator.Translate("FactionEditorWindow_FactionGoodness"));
                    Widgets.TextFieldNumeric(new Rect(150, y, 375, 30), ref rel.goodwill, ref newFactionGoodwillBuff[i], -10000000000f);

                    y += 35;
                    switch (rel.kind)
                    {
                        case FactionRelationKind.Ally:
                            {
                                if (Widgets.ButtonText(new Rect(5, y, 520, 30), rel.kind.GetLabel()))
                                {
                                    rel.kind = FactionRelationKind.Neutral;
                                }
                                break;
                            }
                        case FactionRelationKind.Neutral:
                            {
                                if (Widgets.ButtonText(new Rect(5, y, 520, 30), rel.kind.GetLabel()))
                                {
                                    rel.kind = FactionRelationKind.Hostile;
                                }
                                break;
                            }
                        case FactionRelationKind.Hostile:
                            {
                                if (Widgets.ButtonText(new Rect(5, y, 520, 30), rel.kind.GetLabel()))
                                {
                                    rel.kind = FactionRelationKind.Ally;
                                }
                                break;
                            }
                    }

                    boxY += 140;
                    y = boxY + 10;
                }
                Widgets.EndScrollView();

                if (selectedFaction.leader != null)
                {
                    Widgets.Label(new Rect(340, 545, 120, 30), Translator.Translate("FactionEditorWindow_FactionLeaderName"));
                    tmpLeaderName = Widgets.TextField(new Rect(465, 540, 425, 30), tmpLeaderName);
                }

                if (Widgets.ButtonText(new Rect(340, 600, 520, 20), Translator.Translate("FactionEditorWindow_SaveFaction")))
                {
                    SaveFaction();
                }
            }
        }

        private void CreateFaction()
        {
            Faction faction = factionEditor.GenerateFaction(avaliableFactionsDefs.RandomElement());

            foreach (Faction item in Find.FactionManager.AllFactions)
            {
                faction.TryMakeInitialRelationsWith(item);
            }

            FieldInfo relations = typeof(Faction).GetField("relations", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            newFactionRelation = relations.GetValue(faction) as List<FactionRelation>;
            newFactionGoodwillBuff = new string[newFactionRelation.Count];

            for (int i = 0; i < newFactionRelation.Count; i++)
                newFactionGoodwillBuff[i] = newFactionRelation[i].goodwill.ToString();

            if (faction.TryGenerateNewLeader())
            {
                faction.leader.Name = new NameSingle("New leader");
            }

            faction.loadID = Find.UniqueIDsManager.GetNextFactionID();

            Find.FactionManager.Add(faction);

            RecacheFactions();
        }

        private void DeleteFaction(Faction faction)
        {
            if (faction == null || !avaliableFactionsDefs.Contains(faction.def))
                return;

            factionEditor.DeleteFaction(faction);

            selectedFaction = null;

            Messages.Message("FactionEditorWindow_FactionRemoved".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void SaveFaction()
        {
            selectedFaction.def = tmpSelectedFactionDef;
            selectedFaction.Name = tmpFactionName;
            selectedFaction.defeated = tmpIsDefeated;

            if (selectedFaction.leader != null)
            {
                selectedFaction.leader.Name = new NameSingle(tmpLeaderName);
            }

            Messages.Message("FactionEditorWindow_SaveFactionInfo".Translate(), MessageTypeDefOf.NeutralEvent, false);
        }

        private void SelectNewFaction(Faction faction)
        {
            selectedFaction = faction;

            tmpSelectedFactionDef = selectedFaction.def;
            tmpFactionName = selectedFaction.Name;
            tmpIsDefeated = selectedFaction.defeated;

            if(selectedFaction.leader != null)
                tmpLeaderName = selectedFaction.leader.Name.ToStringFull;

            FieldInfo relations = typeof(Faction).GetField("relations", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            newFactionRelation = relations.GetValue(faction) as List<FactionRelation>;
            newFactionGoodwillBuff = new string[newFactionRelation.Count];

            for (int i = 0; i < newFactionRelation.Count; i++)
                newFactionGoodwillBuff[i] = newFactionRelation[i].goodwill.ToString();
        }
    }
}
