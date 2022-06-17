﻿using System.Reflection;
using HarmonyLib;
using Verse;

namespace Bubbles.Core
{
  internal static class Compatibility
  {
    private static readonly bool CameraPlusLoaded = ModsConfig.IsActive("brrainz.cameraplus");

    public static readonly bool HasSteamDeckUpdate = CheckHasSteamDeckUpdate();

    private static FastInvokeHandler _cameraPlusLerpRootSize;

    public static void Apply(ref float altitude) => ApplyCameraPlus(ref altitude);

    private static void ApplyCameraPlus(ref float scale)
    {
      if (!CameraPlusLoaded) { return; }

      if (_cameraPlusLerpRootSize == null) { _cameraPlusLerpRootSize = MethodInvoker.GetHandler(AccessTools.Method("CameraPlus.Tools:LerpRootSize")); }

      scale = (float) _cameraPlusLerpRootSize(null, scale);
    }

    private static bool CheckHasSteamDeckUpdate()
    {
      try { return typeof(Widgets).GetMethod("BeginGroup") != null && typeof(Widgets).GetMethod("EndGroup") != null; }
      catch (AmbiguousMatchException) { return true; }
    }
  }
}