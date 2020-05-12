using System;
using AllowTool.Comps;
using HugsLib.Utils;
using RimWorld;
using Verse;
using System.Threading;

namespace AllowTool
{
    /// <summary>
    ///     Designates things for urgent hauling.
    /// </summary>
    public class Designator_HaulUrgently : Designator_SelectableThings
    {
        public Designator_HaulUrgently()
        {
            UseDesignatorDef(AllowToolDefOf.HaulUrgentlyDesignator);
        }

        protected override DesignationDef Designation => AllowToolDefOf.HaulUrgentlyDesignation;

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();
            if (!HugsLibUtility.ShiftIsHeld) return;
            
            foreach (var colonist in Find.CurrentMap.mapPawns.FreeColonists)
                colonist.jobs.CheckForJobOverride();
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            return ThingIsRelevant(t) && !t.HasDesignation(AllowToolDefOf.HaulUrgentlyDesignation);
        }

        public override void DesignateThing(Thing thing)
        {
            if (thing.def.designateHaulable) // for things that require explicit hauling designation, such as rock chunks
                thing.ToggleDesignation(DesignationDefOf.Haul, true);
            thing.ToggleDesignation(AllowToolDefOf.HaulUrgentlyDesignation, true);
            // un-forbid for convenience
            thing.SetForbidden(false, false);
            
            Map.GetComponent<MapComp_AllowTool>().designatedThings.Add(thing);
            Log.Message($"[AllowTool] Added Thing from HaulUrgently list. Length of list: {Map.GetComponent<MapComp_AllowTool>().designatedThings.Count}");
        }

        private static bool ThingIsRelevant(Thing thing)
        {
            if (thing.def == null || thing.Position.Fogged(thing.Map)) return false;
            
            return (thing.def.alwaysHaulable || thing.def.EverHaulable) && !thing.IsInValidBestStorage();
        }
    }
}