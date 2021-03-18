using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using WorldEdit_2_0.MainEditor;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.WorldObjects.Other;
using WorldEdit_2_0.MainEditor.WorldObjects.Settlements;
using WorldEdit_RimCity.WorldObjects.Cities;

namespace WorldEdit_RimCity
{
    public class WorldEditRimCity : Mod
    {
        private List<string> rimCitySettlementDefs = new List<string>
        {
            "City_Abandoned",
            "City_Citadel",
            "City_Compromised",
            "City_Faction",
            "City_Ghost"
        };
        private WorldEditor worldEditor => WorldEditor.WorldEditorInstance;

        public WorldEditRimCity(ModContentPack content) : base(content)
        {
            RegisterSettlementWorldObjectDefs();
        }

        private void RegisterSettlementWorldObjectDefs()
        {
            SettlementEditor settlementEditor = worldEditor.GetEditor<SettlementEditor>();
            for (int i = 0; i < rimCitySettlementDefs.Count; i++)
            {
                settlementEditor.RegisterSettlementWorldObjectDefName(rimCitySettlementDefs[i]);
            }
        }
    }
}
