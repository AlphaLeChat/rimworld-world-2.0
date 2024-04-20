using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using LudeonTK;

namespace WorldEdit_2_0.MainEditor.Templates.PawnEditor
{
    public class AddTraitMenu : EditWindow
    {
        public override Vector2 InitialSize => new Vector2(250, 150);
        private TraitDef trait = TraitDefOf.Abrasive;
        private Pawn pawn;
        private int degree = 0;
        private TraitDegreeData degreeData = null;

        public AddTraitMenu(Pawn pawn)
        {
            resizeable = false;

            this.pawn = pawn;

            degreeData = trait.degreeDatas.FirstOrDefault();
        }
        public override void DoWindowContents(Rect inRect)
        {
            Widgets.Label(new Rect(0, 15, 70, 20), Translator.Translate("TraitLabel"));
            if (Widgets.ButtonText(new Rect(80, 15, 160, 20), degreeData.label))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var trait in DefDatabase<TraitDef>.AllDefsListForReading)
                {
                    for (int i = 0; i < trait.degreeDatas.Count; i++)
                    {
                        TraitDegreeData deg = trait.degreeDatas[i];
                        list.Add(new FloatMenuOption(deg.label, delegate
                        {
                            this.trait = trait;
                            degree = deg.degree;
                            degreeData = deg;
                        }));
                    }
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (Widgets.ButtonText(new Rect(0, 110, 240, 20), Translator.Translate("AddNewTrait")))
            {
                AddTrait();
            }

        }

        private void AddTrait()
        {
            pawn.story.traits.GainTrait(new Trait(trait, degree));

            Close();
        }
    }
}
