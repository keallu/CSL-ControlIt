﻿using HarmonyLib;
using System.Reflection;

namespace ControlIt
{
    public static class Patcher
    {
        private const string HarmonyId = "com.github.keallu.csl.controlit";

        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched)
            {
                return;
            }

            patched = true;
            Harmony harmony = new Harmony(HarmonyId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void UnpatchAll()
        {
            if (!patched)
            {
                return;
            }

            Harmony harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            patched = false;
        }
    }
}
