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
    public class WorldEditItemStashContentsCompWindow : WorldEditCompWindow
    {
        private ItemStashContentsComp itemStashContentsComp;

        public override Vector2 InitialSize => new Vector2(701, 600);

        private List<Thing> contents;
        private int contentsSize = 0;

        public WorldEditItemStashContentsCompWindow(WorldObjectComp worldObjectComp, WorldEditWorldObjectComp worldEditComp) : base(worldObjectComp, worldEditComp)
        {
            itemStashContentsComp = (ItemStashContentsComp)worldObjectComp;

            contents = itemStashContentsComp.contents.ToList();

            contentsSize = contents.Count * 45;
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawHeader(ref inRect);

            ThingsMenu.DrawEditableThingsList(ref inRect, contents, ref contentsSize);

            DrawBottom(ref inRect);
        }

        protected override bool AcceptChanges()
        {
            itemStashContentsComp.contents.TryAddRangeOrTransfer(contents);

            return true;
        }
    }
}
