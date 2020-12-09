using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor.Templates.PawnEditor
{
    public class RelationsMenu : EditWindow
    {
        public override Vector2 InitialSize => new Vector2(400, 315);

        private Vector2 scroll = Vector2.zero;

        private PawnRelationDef relationType = null;

        private Faction faction = null;

        private Pawn pawnToRelation = null;
        private Pawn parentPawn = null;

        private FactionManager rimfactionManager = Find.FactionManager;

        private List<Faction> getFactionList => Find.FactionManager.AllFactionsListForReading.Where(f =>
        rimfactionManager.OfMechanoids.def != f.def && rimfactionManager.OfInsects.def != f.def &&
        rimfactionManager.OfAncientsHostile.def != f.def && rimfactionManager.OfAncients.def != f.def).ToList();

        public RelationsMenu(Pawn p)
        {
            resizeable = false;

            relationType = DefDatabase<PawnRelationDef>.GetRandom();
            faction = getFactionList.RandomElement();

            pawnToRelation = (from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
                              where x.Faction == faction
                              select x).RandomElement();

            parentPawn = p;
        }

        public override void DoWindowContents(Rect inRect)
        {
            int size = parentPawn.relations.DirectRelations.Count * 25;
            Rect scrollRectFact = new Rect(0, 20, 385, 170);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, size);
            Widgets.BeginScrollView(scrollRectFact, ref scroll, scrollVertRectFact);
            int xP = 0;
            for (int i = 0; i < parentPawn.relations.DirectRelations.Count; i++)
            {
                DirectPawnRelation rel = parentPawn.relations.DirectRelations[i];

                Widgets.Label(new Rect(0, xP, 260, 20), $"{rel.otherPawn.Name.ToStringFull} - {rel.def.LabelCap}");
                if (Widgets.ButtonText(new Rect(270, xP, 110, 20), Translator.Translate("DeleteTargetRelation")))
                {
                    parentPawn.relations.RemoveDirectRelation(rel);
                }
                xP += 22;
            }
            Widgets.EndScrollView();

            Widgets.Label(new Rect(0, 190, 120, 20), Translator.Translate("RelationType"));
            if (Widgets.ButtonText(new Rect(110, 190, 270, 20), relationType.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var rDef in DefDatabase<PawnRelationDef>.AllDefsListForReading.Where(rel => !rel.implied))
                {
                    list.Add(new FloatMenuOption(rDef.LabelCap, delegate
                    {
                        relationType = rDef;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            Widgets.Label(new Rect(0, 220, 120, 20), Translator.Translate("FactionForSort"));
            if (Widgets.ButtonText(new Rect(110, 220, 270, 20), faction.Name))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var f in getFactionList)
                {
                    list.Add(new FloatMenuOption(f.Name, delegate
                    {
                        faction = f;
                        pawnToRelation = (from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
                                          where x.Faction == faction
                                          select x).FirstOrDefault();
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            Widgets.Label(new Rect(0, 250, 120, 20), Translator.Translate("PawnToSelect"));
            if (Widgets.ButtonText(new Rect(110, 250, 270, 20), pawnToRelation.Name.ToStringFull))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var p in from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
                                  where x.Faction == faction
                                  select x)
                {
                    list.Add(new FloatMenuOption(p.Name.ToStringFull, delegate
                    {
                        pawnToRelation = p;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (Widgets.ButtonText(new Rect(0, 280, 390, 20), Translator.Translate("AddNewRelationToPawn")))
            {
                AddNewRelationToPawn();
            }
        }

        private void AddNewRelationToPawn()
        {
            parentPawn.relations.AddDirectRelation(relationType, pawnToRelation);
        }
    }
}
