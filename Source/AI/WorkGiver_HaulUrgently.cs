using System.Collections.Generic;
using System.Linq;
using AllowTool.Comps;
using JetBrains.Annotations;
using RimWorld;
using Verse;
using Verse.AI;

namespace AllowTool
{
    // Generates hauling jobs for things designated for urgent hauling
    [UsedImplicitly]
    public class WorkGiver_HaulUrgently : WorkGiver_Scanner
    {
        public delegate Job TryGetJobOnThing(Pawn pawn, Thing t, bool forced);

        // give a vanilla haul job- it works just fine for our needs
        public static TryGetJobOnThing JobOnThingDelegate = (pawn, t, forced) => HaulAIUtility.HaulToStorageJob(pawn, t);

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobOnThingDelegate(pawn, t, forced);
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            // look over all saved things
            return pawn.Map.GetComponent<MapComp_AllowTool>().designatedThings.Where(thing =>
                thing?.def != null && (thing.def.alwaysHaulable || thing.def.EverHaulable) && !thing.IsInValidBestStorage() &&
                HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, thing, false));
        }
    }
}