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

        public void DeleteAllSettlements()
        {
            List<Settlement> allSettlements = new List<Settlement>(Find.WorldObjects.Settlements);
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
    }
}
