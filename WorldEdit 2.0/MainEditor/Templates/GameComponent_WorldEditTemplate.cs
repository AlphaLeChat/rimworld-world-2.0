using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WorldEdit_2_0.MainEditor.Templates
{
    public class GameComponent_WorldEditTemplate : GameComponent
    {
        private string templateName;
        public string TemplateName => templateName;

        private string description;
        public string Description => description;

        private string author;
        public string Author => author;

        private bool canSelectPawns;
        public bool CanSelectPawns => canSelectPawns;

        private List<Pawn> forceStartPawns = new List<Pawn>();
        public List<Pawn> ForceStartPawns => forceStartPawns;

        public bool IsValidTemplate => !string.IsNullOrEmpty(templateName) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(author);

        public GameComponent_WorldEditTemplate()
        {

        }

        public GameComponent_WorldEditTemplate(Game game)
        {

        }


        public GameComponent_WorldEditTemplate(string templateName, string description, string author, bool canSelectPawns, List<Pawn> forceStartPawns = null)
        {
            this.templateName = templateName;
            this.description = description;
            this.author = author;
            this.canSelectPawns = canSelectPawns;
            this.forceStartPawns = forceStartPawns;
        }

        public void SetTemplateName(string name) => templateName = name;
        public void SetAuthor(string author) => this.author = author;
        public void SetDescription(string description) => this.description = description;
        public void SetCanSelectPawns(bool canSelectPawns) => this.canSelectPawns = canSelectPawns;
        public void SetForceStartPawns(List<Pawn> pawnList)
        {
            if (pawnList != null) 
                forceStartPawns = pawnList;
        }

        public void ClearTemplate()
        {
            templateName = string.Empty;
            description = string.Empty;
            author = string.Empty;

            canSelectPawns = true;

            forceStartPawns = new List<Pawn>();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref templateName, "templateName");
            Scribe_Values.Look(ref description, "description");
            Scribe_Values.Look(ref author, "author");
            Scribe_Values.Look(ref canSelectPawns, "canSelectPawns");
            Scribe_Collections.Look(ref forceStartPawns, "forceStartPawns", LookMode.Deep);
        }
    }
}
