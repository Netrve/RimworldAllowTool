using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AllowTool.Comps
{
    [UsedImplicitly]
    public class MapComp_AllowTool : MapComponent
    {
        public readonly List<Thing> designatedThings = new List<Thing>();

        public MapComp_AllowTool(Map map) : base(map) 
        {
        }
    }
}