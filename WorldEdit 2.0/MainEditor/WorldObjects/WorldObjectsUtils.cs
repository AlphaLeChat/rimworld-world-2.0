using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEdit_2_0.MainEditor.WorldObjects
{
    public static class WorldObjectsUtils
    {
        public static IOrderedEnumerable<T> SortWorldObjectsBy<T>(List<T> worldObjects, SortWorldObjectBy sortBy) where T : WorldObject
        {
            switch (sortBy)
            {
                case SortWorldObjectBy.ID:
                        return worldObjects.OrderBy(x => x.ID);
                case SortWorldObjectBy.ABC:
                        return worldObjects.OrderBy(x => x.LabelCap);
                default:
                    return worldObjects.OrderBy(x => x.LabelCap);
            }
        }

        public static IOrderedEnumerable<T> SortWorldObjectsBy<T>(IEnumerable<T> worldObjects, SortWorldObjectBy sortBy) where T : WorldObject
        {
            switch (sortBy)
            {
                case SortWorldObjectBy.ID:
                    return worldObjects.OrderBy(x => x.ID);
                case SortWorldObjectBy.ABC:
                    return worldObjects.OrderBy(x => x.LabelCap);
                default:
                    return worldObjects.OrderBy(x => x.LabelCap);
            }
        }

        public static IOrderedEnumerable<Faction> SortFactionBy(List<Faction> factions, SortWorldObjectBy sortBy)
        {
            switch (sortBy)
            {
                case SortWorldObjectBy.ID:
                    return factions.OrderBy(x => x.loadID);
                case SortWorldObjectBy.ABC:
                    return factions.OrderBy(x => x.Name);
                default:
                    return factions.OrderBy(x => x.Name);
            }
        }

        public static IOrderedEnumerable<Faction> SortFactionBy(IEnumerable<Faction> factions, SortWorldObjectBy sortBy)
        {
            switch (sortBy)
            {
                case SortWorldObjectBy.ID:
                    return factions.OrderBy(x => x.loadID);
                case SortWorldObjectBy.ABC:
                    return factions.OrderBy(x => x.Name);
                default:
                    return factions.OrderBy(x => x.Name);
            }
        }
    }
}
