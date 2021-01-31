using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TestApi
{
    public class HediffCompProperties_Test : HediffCompProperties
    {
        public float minSeverity;

        public int needTicks;

        public HediffDef hediffDef;

        public BodyPartDef bodyPartDef

        public HediffCompProperties_Test()
        {
            compClass = typeof(HediffComp_Test);
        }
    }
}
}
