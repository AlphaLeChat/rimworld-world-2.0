using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Templates
{
    public class Page_SelectScenarioInGame : Page_SelectScenario
    {
        protected override bool CanDoNext()
        {
            FieldInfo scen = typeof(Page_SelectScenario).GetField("curScen", BindingFlags.NonPublic | BindingFlags.Instance);
            if (scen == null)
            {
                return false;
            }
            Scenario scenario = scen.GetValue(this) as Scenario;
            if (scenario == null)
            {
                return false;
            }
            Current.Game.Scenario = scenario;

            Close();

            return false;
        }
    }
}
