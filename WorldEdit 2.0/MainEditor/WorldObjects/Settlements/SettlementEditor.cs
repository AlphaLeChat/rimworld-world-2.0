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

namespace WorldEdit_2_0.MainEditor.WorldObjects.Settlements
{
    public class SettlementEditor : Editor
    {
        protected override Type WindowType => typeof(SettlementEditorWindow);

        protected override KeyCode DefaultKeyCode => KeyCode.F6;

        public override string EditorName => "WE_Settings_SettlementEditorKey".Translate();

        public KeyCode DragAndDropKey => dragAndDropKey;
        private KeyCode dragAndDropKey;

        public void DeleteAllSettlements()
        {
            List<Settlement> allSettlements = new List<Settlement>(Find.WorldObjects.Settlements.Where(stl =>
                                            stl.Faction != Faction.OfAncients && stl.Faction != Faction.OfInsects &&
                                            stl.Faction != Faction.OfMechanoids && stl.Faction != Faction.OfAncientsHostile &&
                                            stl.Faction != Faction.OfPlayer));
            foreach (var settlement in allSettlements)
            {
                Find.WorldObjects.Remove(settlement);
            }
        }

        public Settlement AddNewSettlement(int tile, Faction faction)
        {
            WorldObject obj = (WorldObject)Activator.CreateInstance(WorldObjectDefOf.Settlement.worldObjectClass);
            obj.def = WorldObjectDefOf.Settlement;
            obj.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
            obj.creationGameTicks = Find.TickManager.TicksGame;
            obj.PostMake();

            Settlement settlement = (Settlement)obj;

            settlement.SetFaction(faction);
            settlement.Tile = tile;
            settlement.Name = "New settlement " + obj.ID;

            Find.WorldObjects.Add(settlement);

            return settlement;
        }

        public override void DrawSettings(Rect inRect, Listing_Standard listing_Standard)
        {
            base.DrawSettings(inRect, listing_Standard);

            if (listing_Standard.ButtonText($"{EditorName} {"SettlementEditor_DragAndDropKey".Translate()}: {dragAndDropKey}"))
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
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref dragAndDropKey, "dragAndDropKey", KeyCode.Mouse2);
        }
    }
}
