using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public class WorldTemplateFeatureGridElement : WorldTemplateGridElement
    {
        public List<WorldFeature> features = new List<WorldFeature>();

        public WorldTemplateFeatureGridElement()
        {

        }

        public WorldTemplateFeatureGridElement(byte[] data, List<WorldFeature> features)
        {
            this.data = data;
            this.features = features;
        }

        public WorldFeature GetFeatureWithID(int uniqueID)
        {
            for (int i = 0; i < features.Count; i++)
            {
                if (features[i].uniqueID == uniqueID)
                {
                    return features[i];
                }
            }
            return null;
        }

        public override void ExposeData()
        {
            DataExposeUtility.LookByteArray(ref data, "data");
            Scribe_Collections.Look(ref features, "features", LookMode.Deep);
        }
    }
}
