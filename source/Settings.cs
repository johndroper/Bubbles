using System.Collections.Generic;
using System.Linq;
using Bubbles.Configuration;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bubbles
{
  public class Settings : ModSettings
  {
    public static bool Activated = true;

    public static readonly Setting<int> AutoHideSpeed = new Setting<int>(nameof(AutoHideSpeed), 5);

    public static readonly Setting<bool> DoNonPlayer = new Setting<bool>(nameof(DoNonPlayer), true);
    public static readonly Setting<bool> DoAnimals = new Setting<bool>(nameof(DoAnimals), true);
    public static readonly Setting<bool> DoDrafted = new Setting<bool>(nameof(DoDrafted), false);
    public static readonly Setting<bool> DoTextColors = new Setting<bool>(nameof(DoTextColors), false);

    public static readonly Setting<int> AltitudeBase = new Setting<int>(nameof(AltitudeBase), 11);
    public static readonly Setting<int> AltitudeMax = new Setting<int>(nameof(AltitudeMax), 40);
    public static readonly Setting<int> PawnMax = new Setting<int>(nameof(PawnMax), 3);

    public static readonly Setting<int> FontSize = new Setting<int>(nameof(FontSize), 12);
    public static readonly Setting<int> PaddingX = new Setting<int>(nameof(PaddingX), 10);
    public static readonly Setting<int> PaddingY = new Setting<int>(nameof(PaddingY), 4);
    public static readonly Setting<int> WidthMax = new Setting<int>(nameof(WidthMax), 250);

    public static readonly Setting<int> OffsetStart = new Setting<int>(nameof(OffsetStart), 15);
    public static readonly Setting<int> OffsetSpacing = new Setting<int>(nameof(OffsetSpacing), 2);
    public static readonly Setting<Rot4> OffsetDirection = new Setting<Rot4>(nameof(OffsetDirection), Rot4.North);

    public static readonly Setting<float> OpacityStart = new Setting<float>(nameof(OpacityStart), 0.9f);
    public static readonly Setting<float> OpacityHover = new Setting<float>(nameof(OpacityHover), 0.2f);

    public static readonly Setting<int> FadeStart = new Setting<int>(nameof(FadeStart), 500);
    public static readonly Setting<int> FadeLength = new Setting<int>(nameof(FadeLength), 250);

    public static readonly Setting<Color> Background = new Setting<Color>(nameof(Background), Color.white);
    public static readonly Setting<Color> Foreground = new Setting<Color>(nameof(Foreground), Color.black);
    public static readonly Setting<Color> SelectedBackground = new Setting<Color>(nameof(SelectedBackground), new Color(1f, 1f, 0.75f));
    public static readonly Setting<Color> SelectedForeground = new Setting<Color>(nameof(SelectedForeground), Color.black);

    private static IEnumerable<Setting> AllSettings => typeof(Settings).GetFields().Select(field => field.GetValue(null) as Setting).Where(setting => setting != null);

    public static void Reset() => AllSettings.Do(setting => setting.ToDefault());
    public override void ExposeData() => AllSettings.Do(setting => setting.Scribe());
  }
}