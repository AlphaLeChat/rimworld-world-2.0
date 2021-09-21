using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Models
{
    public class WorldTemplateGridElement : IExposable
    {
        public string dataDeflate;

        public byte[] data;

        public WorldTemplateGridElement()
        {

        }

        public WorldTemplateGridElement(byte[] data)
        {
            this.data = data;
        }

        public byte[] GetBytes()
        {
            if (data != null && data.Length > 0)
                return data;

            try
            {
                data = CompressUtility.Decompress(Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(dataDeflate)));
            }
            catch { }

            if(data != null)
            {
                return data;
            }

            try
            {
                data = Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(dataDeflate));
            }
            catch { }

            if(data == null)
            {
                Log.Error($"Cannot deserialize byte[] WorldTemplateGridElement");
            }

            return data;
        }

        public virtual void ExposeData()
        {
            DataExposeUtility.ByteArray(ref data, "data");
        }
    }
}
