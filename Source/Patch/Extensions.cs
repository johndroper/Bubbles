﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles.Patch
{
    internal static class Extensions
    {
        public static string Italic(this string self) => "<i>" + self + "</i>";
        public static string Bold(this string self) => "<b>" + self + "</b>";

        public static int LastIndex(this IList self) => self.Count - 1;

        public static Color WithAlpha(this Color self, float alpha) => new Color(self.r, self.g, self.b, alpha);

        public static int? ToInt(this string self) => int.TryParse(self, out var result) ? result : (int?) null;
        public static bool? ToBool(this string self) => bool.TryParse(self, out var result) ? result : (bool?) null;

        public static string ToDecimalString(this int self, int remainder) => $"{self.ToString().Bold()}.{remainder.ToString("D2").Italic()}";
        public static string ToPercentageString(this float self) => self.ToPercentageInt() + "%";
        public static int ToPercentageInt(this float self) => Mathf.RoundToInt(self * 100f);
        public static float ToPercentageFloat(this int self) => self / 100f;
        public static int WrapTo(this int self, int max) => ((self % max) + max) % max;

        public static GUIStyle SetTo(this GUIStyle self, int? size = null, TextAnchor? alignment = null, bool? wrap = null) => new GUIStyle(self) { fontSize = size ?? self.fontSize, alignment = alignment ?? self.alignment, wordWrap = wrap ?? self.wordWrap };
        public static GUIStyle ResizedBy(this GUIStyle self, int size = 0) => new GUIStyle(self) { fontSize = self.fontSize + size };

        public static Rect Fix(this Rect self) => new Rect(Mathf.Round(self.x), Mathf.Round(self.y), Mathf.Round(self.width), Mathf.Round(self.height));
        public static Rect AdjustedBy(this Rect self, float x, float y, float width, float height) => new Rect(self.x + x, self.y + y, self.width + width, self.height + height);
        public static Rect[] GetHGrid(this Rect self, float padding, params float[] widths)
        {
            var unfixedCount = 0;
            var currentX = self.x;
            var fixedWidths = 0f;

            var rects = new List<Rect> { self };

            for (var index = 0; index < widths.Length; index++)
            {
                var width = widths[index];
                if (width >= 0f) { fixedWidths += width; }
                else { unfixedCount++; }

                if (index != widths.LastIndex()) { fixedWidths += padding; }
            }

            var unfixedWidth = unfixedCount > 0 ? (self.width - fixedWidths) / unfixedCount : 0f;

            foreach (var width in widths)
            {
                float newWidth;

                if (width >= 0f)
                {
                    newWidth = width;
                    rects.Add(new Rect(currentX, self.y, newWidth, self.height).Fix());
                }
                else
                {
                    newWidth = unfixedWidth;
                    rects.Add(new Rect(currentX, self.y, newWidth, self.height).Fix());
                }

                currentX += newWidth + padding;
            }

            return rects.ToArray();
        }
        public static Rect[] GetVGrid(this Rect self, float padding, params float[] heights)
        {
            var unfixedCount = 0;
            var currentY = self.y;
            var fixedHeights = 0f;

            var rects = new List<Rect> { self };

            for (var index = 0; index < heights.Length; index++)
            {
                var height = heights[index];
                if (height >= 0f) { fixedHeights += height; }
                else { unfixedCount++; }

                if (index != heights.LastIndex()) { fixedHeights += padding; }
            }

            var unfixedHeight = unfixedCount > 0 ? (self.height - fixedHeights) / unfixedCount : 0f;

            foreach (var height in heights)
            {
                float newHeight;

                if (height >= 0f)
                {
                    newHeight = height;
                    rects.Add(new Rect(self.x, currentY, self.width, newHeight).Fix());
                }
                else
                {
                    newHeight = unfixedHeight;
                    rects.Add(new Rect(self.x, currentY, self.width, newHeight).Fix());
                }

                currentY += newHeight + padding;
            }

            return rects.ToArray();
        }
    }
}
