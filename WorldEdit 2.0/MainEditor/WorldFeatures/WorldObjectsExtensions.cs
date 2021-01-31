using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor.WorldObjects;

namespace WorldEdit_2_0.MainEditor.WorldFeatures
{
    public static class WorldObjectsExtensions
    {
        public static string TranslateSortWorldObjectBy(this SortWorldObjectBy sortWorldObjectBy)
        {
            return $"SortWorldObjectBy_{sortWorldObjectBy}".Translate();
        }
    }
}
