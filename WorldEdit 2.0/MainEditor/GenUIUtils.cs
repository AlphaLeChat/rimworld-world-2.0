using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WorldEdit_2_0.MainEditor
{
    public static class GenUIUtils
    {
		public static void DrawMouseAttachment(string text)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.Label(mouseRect.AtZero(), text);
			}, doBackground: false, absorbInputAroundWindow: false, 0f);
		}
	}
}
