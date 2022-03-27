using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor
{
    public class Dialog_Confirm : Window
    {
        public override Vector2 InitialSize => new Vector2(200, 120);

        private Action<bool> callback;

        public Dialog_Confirm(Action<bool> callback)
        {
            resizeable = false;
            forcePause = true;
            doCloseX = false;

            this.callback = callback;
        }
        public override void DoWindowContents(Rect inRect)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, inRect.width, 25), "Dialog_Confirm.Title".Translate());
            Text.Anchor = TextAnchor.UpperLeft;

            Rect buttonRect = new Rect(0, 30, inRect.width, 25);
            if (Widgets.ButtonText(buttonRect, "Dialog_Confirm.Ok".Translate()))
            {
                callback(true);
                Close();
            }

            buttonRect.y += 30;
            if (Widgets.ButtonText(buttonRect, "Dialog_Confirm.Decline".Translate()))
            {
                callback(false);
                Close();
            }
        }
    }
}
