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
    public class HealthMenu : EditWindow
    {
        private Vector2 scrollPosition = Vector2.zero;

        private bool highlight = true;

        private bool showAllHediffs = false;

        public const float TopPadding = 20f;

        private readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

        private readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding");

        public override Vector2 InitialSize => new Vector2(400, 520);
        private int heddiffCount = 0;
        private Pawn pawn;

        private BodyPartRecord bodyPart = null;
        private HediffStage hediffStage = null;
        private HediffDef hediffDef = null;
        private DamageDef damageType = null;

        private string buffAmount = string.Empty;
        private float damageAmount = 0;

        private string buffSever = string.Empty;
        private float sevAmount = 0;

        public HealthMenu(Pawn p)
        {
            pawn = p;
            resizeable = false;

            RecalculateHeight();

            bodyPart = pawn.health.hediffSet.GetNotMissingParts().RandomElement();
            hediffDef = HediffDefOf.Asthma;

            if (hediffDef.stages != null)
                hediffStage = hediffDef.stages.RandomElement();

            if (hediffStage != null)
            {
                buffSever = $"{hediffStage.minSeverity}";
                sevAmount = hediffStage.minSeverity;
            }

            damageType = DamageDefOf.Arrow;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Widgets.Label(new Rect(0, 5, 150, 20), Translator.Translate("HeddifsCurrent"));

            if (Widgets.ButtonText(new Rect(150, 5, 210, 20), Translator.Translate("FullHeal")))
            {
                FullHeal();
            }

            int defSize = heddiffCount;
            Rect scrollRectFact = new Rect(0, 30, 390, 200);
            Rect scrollVertRectFact = new Rect(0, 0, scrollRectFact.x, defSize);
            Widgets.BeginScrollView(scrollRectFact, ref scrollPosition, scrollVertRectFact);
            float curY = 0f;
            foreach (IGrouping<BodyPartRecord, Hediff> item in VisibleHediffGroupsInOrder(pawn, true))
            {
                DrawHediffRow(inRect, pawn, item, ref curY);
            }
            Widgets.EndScrollView();

            Widgets.Label(new Rect(0, 250, 120, 20), Translator.Translate("BodyPartInfo"));
            if (Widgets.ButtonText(new Rect(110, 250, 270, 20), bodyPart.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var p in pawn.health.hediffSet.GetNotMissingParts())
                {
                    list.Add(new FloatMenuOption(p.LabelCap, delegate
                    {
                        bodyPart = p;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            Widgets.DrawLineHorizontal(0, 280, 400);

            Widgets.Label(new Rect(0, 290, 120, 20), Translator.Translate("HeddifDefInfo"));
            if (Widgets.ButtonText(new Rect(110, 290, 270, 20), hediffDef.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var h in DefDatabase<HediffDef>.AllDefsListForReading)
                {
                    list.Add(new FloatMenuOption(h.LabelCap, delegate
                    {
                        hediffDef = h;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
            Widgets.Label(new Rect(0, 315, 120, 20), Translator.Translate("HeddifStageInfo"));
            if (Widgets.ButtonText(new Rect(110, 315, 270, 20), hediffStage.label))
            {
                if (hediffDef != null || hediffDef.stages.Count > 1)
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (var stage in hediffDef.stages)
                    {
                        list.Add(new FloatMenuOption(stage.label, delegate
                        {
                            buffSever = $"{stage.minSeverity}";
                            sevAmount = stage.minSeverity;
                            hediffStage = stage;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                }
            }

            Widgets.Label(new Rect(0, 340, 120, 20), Translator.Translate("SeverityInfo"));
            Widgets.TextFieldNumeric(new Rect(130, 340, 240, 20), ref sevAmount, ref buffSever, 0, 1);

            if (Widgets.ButtonText(new Rect(0, 370, 390, 20), Translator.Translate("AddNewHediffLabel")))
            {
                AddNewHediff();
            }

            Widgets.DrawLineHorizontal(0, 405, 400);

            Widgets.Label(new Rect(0, 420, 120, 20), Translator.Translate("DamageType"));
            if (Widgets.ButtonText(new Rect(110, 420, 270, 20), damageType.LabelCap))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var dmg in DefDatabase<DamageDef>.AllDefsListForReading)
                {
                    list.Add(new FloatMenuOption(dmg.LabelCap, delegate
                    {
                        damageType = dmg;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            Widgets.Label(new Rect(0, 445, 120, 20), Translator.Translate("DamageAmount"));
            Widgets.TextFieldNumeric(new Rect(130, 445, 240, 20), ref damageAmount, ref buffAmount, 0);

            if (Widgets.ButtonText(new Rect(0, 470, 390, 20), Translator.Translate("AddDamageLabel")))
            {
                AddDamage();
            }
        }

        private void FullHeal()
        {
            foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet.GetNotMissingParts())
            {
                pawn.health.RestorePart(notMissingPart);
            }

            RecalculateHeight();
        }

        private void AddDamage()
        {
            DamageInfo info = new DamageInfo(damageType, damageAmount, hitPart: bodyPart);

            pawn.TakeDamage(info);

            RecalculateHeight();
        }

        private void AddNewHediff()
        {
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPart);

            hediff.Severity = sevAmount;

            pawn.health.AddHediff(hediff);

            RecalculateHeight();
        }

        private void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
        {
            float num = rect.width * 0.375f;
            float width = rect.width - num - 20f;
            BodyPartRecord part = diffs.First().Part;
            float a = (part != null) ? Text.CalcHeight(part.LabelCap, num) : Text.CalcHeight("WholeBody".Translate(), num);
            float num2 = 0f;
            float num3 = curY;
            float num4 = 0f;
            foreach (IGrouping<int, Hediff> item in from x in diffs
                                                    group x by x.UIGroupKey)
            {
                int num5 = item.Count();
                string text = item.First().LabelCap;
                if (num5 != 1)
                {
                    text = text + " x" + num5.ToString();
                }
                num4 += Text.CalcHeight(text, width);
            }
            num2 = num4;
            Rect rect2 = new Rect(0f, curY, rect.width, Mathf.Max(a, num2));
            DoRightRowHighlight(rect2);
            if (part != null)
            {
                GUI.color = HealthUtility.GetPartConditionLabel(pawn, part).Second;
                Widgets.Label(new Rect(0f, curY, num, 100f), part.LabelCap);
            }
            else
            {
                GUI.color = HealthUtility.DarkRedColor;
                Widgets.Label(new Rect(0f, curY, num, 100f), "WholeBody".Translate());
            }
            GUI.color = Color.white;
            foreach (IGrouping<int, Hediff> item2 in from x in diffs
                                                     group x by x.UIGroupKey)
            {
                int num6 = 0;
                Hediff hediff = null;
                Texture2D texture2D = null;
                TextureAndColor textureAndColor = null;
                float num7 = 0f;
                foreach (Hediff item3 in item2)
                {
                    if (num6 == 0)
                    {
                        hediff = item3;
                    }
                    textureAndColor = item3.StateIcon;
                    if (item3.Bleeding)
                    {
                        texture2D = BleedingIcon;
                    }
                    num7 += item3.BleedRate;
                    num6++;
                }
                string text2 = hediff.LabelCap;
                if (num6 != 1)
                {
                    text2 = text2 + " x" + num6.ToStringCached();
                }
                GUI.color = hediff.LabelColor;
                float num8 = Text.CalcHeight(text2, width);
                Rect rect3 = new Rect(num, curY, width, num8);
                Widgets.Label(rect3, text2);
                GUI.color = Color.white;
                Rect rect4 = new Rect(rect2.xMax - 20f, curY, 20f, 20f);
                if ((bool)texture2D)
                {
                    Rect position = rect4.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(num7, 1f)));
                    GUI.DrawTexture(position, texture2D);
                    rect4.x -= rect4.width;
                }
                if (textureAndColor.HasValue)
                {
                    GUI.color = textureAndColor.Color;
                    GUI.DrawTexture(rect4, textureAndColor.Texture);
                    GUI.color = Color.white;
                    rect4.x -= rect4.width;
                }
                curY += num8;

                if (Widgets.ButtonText(new Rect(rect3.x + 205f, rect3.y, 20, 20), "X"))
                {
                    pawn.health.RemoveHediff(hediff);
                    RecalculateHeight();
                }
            }
            GUI.color = Color.white;
            curY = num3 + Mathf.Max(a, num2);
        }

        private void RecalculateHeight() => heddiffCount = VisibleHediffGroupsInOrder(pawn, true).Count() * 40;

        private IEnumerable<IGrouping<BodyPartRecord, Hediff>> VisibleHediffGroupsInOrder(Pawn pawn, bool showBloodLoss)
        {
            foreach (IGrouping<BodyPartRecord, Hediff> item in from x in VisibleHediffs(pawn, showBloodLoss)
                                                               group x by x.Part into x
                                                               orderby GetListPriority(x.First().Part) descending
                                                               select x)
            {
                yield return item;
            }
        }

        private float GetListPriority(BodyPartRecord rec)
        {
            if (rec == null)
            {
                return 9999999f;
            }
            return (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
        }

        private IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
        {
            if (!showAllHediffs)
            {
                List<Hediff_MissingPart> mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
                for (int i = 0; i < mpca.Count; i++)
                {
                    yield return mpca[i];
                }
                IEnumerable<Hediff> visibleDiffs = pawn.health.hediffSet.hediffs.Where(delegate (Hediff d)
                {
                    if (d is Hediff_MissingPart)
                    {
                        return false;
                    }
                    if (!d.Visible)
                    {
                        return false;
                    }
                    return (!showBloodLoss && d.def == HediffDefOf.BloodLoss) ? false : true;
                });
                foreach (Hediff item in visibleDiffs)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    yield return hediff;
                }
            }
        }

        private void DoRightRowHighlight(Rect rowRect)
        {
            if (highlight)
            {
                GUI.color = StaticHighlightColor;
                GUI.DrawTexture(rowRect, TexUI.HighlightTex);
            }
            highlight = !highlight;
            if (Mouse.IsOver(rowRect))
            {
                GUI.color = HighlightColor;
                GUI.DrawTexture(rowRect, TexUI.HighlightTex);
            }
        }
    }
}
