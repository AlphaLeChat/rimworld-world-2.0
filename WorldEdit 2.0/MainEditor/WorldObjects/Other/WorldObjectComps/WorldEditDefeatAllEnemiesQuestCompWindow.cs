using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WorldEdit_2_0.MainEditor.Models;
using WorldEdit_2_0.MainEditor.Utils;
using WorldEdit_2_0.MainEditor.WorldObjects.Other.Objects;

namespace WorldEdit_2_0.MainEditor.WorldObjects.Other.WorldObjectComps
{
    public class WorldEditDefeatAllEnemiesQuestCompWindow : WorldEditCompWindow
    {
        private DefeatAllEnemiesQuestComp defeatAllEnemiesQuestComp;

        public override Vector2 InitialSize => new Vector2(701, 640);

        private string relationsImprovementString;
        private Faction setFaction;

        private List<Thing> rewardsList;
        private int rewardsSize = 0;

        public WorldEditDefeatAllEnemiesQuestCompWindow(WorldObjectComp worldObjectComp, WorldEditWorldObjectComp worldEditComp) : base(worldObjectComp, worldEditComp)
        {
            defeatAllEnemiesQuestComp = (DefeatAllEnemiesQuestComp)worldObjectComp;

            relationsImprovementString = defeatAllEnemiesQuestComp.relationsImprovement.ToString();
            setFaction = defeatAllEnemiesQuestComp.requestingFaction ?? Find.FactionManager.AllFactionsVisible.First();
            rewardsList = defeatAllEnemiesQuestComp.rewards.ToList();

            rewardsSize = rewardsList.Count * 45;
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawHeader(ref inRect);

            Widgets.Label(new Rect(0, inRect.y, 210, 20), Translator.Translate("WorldEditDefeatAllEnemiesQuestCompWindow_Faction"));
            if(Widgets.ButtonText(new Rect(205, inRect.y, 460, 20), setFaction.Name))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach(var faction in Find.FactionManager.AllFactionsVisible)
                {
                    list.Add(new FloatMenuOption(faction.Name, () => { setFaction = faction; }));
                }

                Find.WindowStack.Add(new FloatMenu(list));
            }


            inRect.y += 25;

            Widgets.Label(new Rect(0, inRect.y, 230, 20), "WorldEditDefeatAllEnemiesQuestCompWindow_Relations".Translate());
            relationsImprovementString = Widgets.TextField(new Rect(235, inRect.y, 430, 20), relationsImprovementString);

            inRect.y += 25;

            ThingsMenu.DrawEditableThingsList(ref inRect, rewardsList, ref rewardsSize);

            DrawBottom(ref inRect);
        }

        protected override bool AcceptChanges()
        {
            if(!int.TryParse(relationsImprovementString, out int relationsImprovement))
            {
                relationsImprovement = 0;
                Messages.Message("WorldEditDefeatAllEnemiesQuestCompWindow_EnterCorrectRelations".Translate(), MessageTypeDefOf.NeutralEvent, false);
            }

            defeatAllEnemiesQuestComp.StartQuest(setFaction, relationsImprovement, rewardsList);

            return true;
        }
    }
}
