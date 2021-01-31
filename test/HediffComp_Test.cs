using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TestApi
{
    public class HediffComp_Test : HediffComp
    {
        public HediffCompProperties_Test Props => (HediffCompProperties_Test)props;

        private int giveTicks;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn.health.hediffSet.HasHediff(Props.hediffDef))
                return;

            if (parent.Severity < Props.minSeverity)
            {
                UpdateTicks();
                return;
            }

            if(Find.TickManager.TicksGame >= giveTicks)
            {
                AddHediff(Pawn, Props.hediffDef, 0.5f);
                UpdateTicks();
            }
        }

        private void UpdateTicks()
        {
            giveTicks = Find.TickManager.TicksGame + Props.needTicks;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            Scribe_Values.Look(ref giveTicks, "giveTicks");
        }

        public void AddHediff(Pawn pawn, HediffDef hediffDef, float severity)
        {
            BodyPartRecord bodyPartRecord = awn.RaceProps.body.AllParts.Where(x => x.def == 
                                                                              Props.part && (Pawn.health.hediffSet.PartIsMissing(x) || Pawn.health.hediffSet.hediffs.Any(x2 => x2.Part == x && !(x2 is Hediff_AddedPart))))
                                                                              .FirstOrDefault();

            AdjustSeverity(pawn, hediffDef, severity, bodyPartRecord);
        }

        public void AdjustSeverity(Pawn pawn, HediffDef hediffDef, float sevOffset, BodyPartRecord bodyPartRecord = null)
        {
            if (sevOffset != 0f)
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef);
                if (firstHediffOfDef != null)
                {
                    firstHediffOfDef.Severity += sevOffset;
                }
                else if (sevOffset > 0f)
                {
                    firstHediffOfDef = HediffMaker.MakeHediff(hdDef, pawn);
                    firstHediffOfDef.Severity = sevOffset;
                    pawn.health.AddHediff(firstHediffOfDef, bodyPartRecord);
                }
            }
        }

    }
}
