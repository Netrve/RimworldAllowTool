using AllowTool.Comps;
using HarmonyLib;
using Verse;

namespace AllowTool.Patches
{
    [HarmonyPatch(typeof(DesignationManager), nameof(DesignationManager.RemoveAllDesignationsOn))]
    internal static class DesignationManagerRAD_Patch
    {
        [HarmonyPrefix]
        public static void RemoveAllDesignationsOn(DesignationManager __instance, Thing t)
        {
            if (__instance.map.designationManager.DesignationOn(t, AllowToolDefOf.HaulUrgentlyDesignation) == null) return;
            
            __instance.map.GetComponent<MapComp_AllowTool>().designatedThings.Remove(t);
        }
    }

    [HarmonyPatch(typeof(DesignationManager), nameof(DesignationManager.RemoveDesignation))]
    internal static class DesignationManagerRD_Patch
    {
        [HarmonyPostfix]
        public static void RemoveDesignation(DesignationManager __instance, Designation des)
        {
            if (des.def != AllowToolDefOf.HaulUrgentlyDesignation) return;
            
            __instance.map.GetComponent<MapComp_AllowTool>().designatedThings.Remove(des.target.Thing);
        }
    }
}