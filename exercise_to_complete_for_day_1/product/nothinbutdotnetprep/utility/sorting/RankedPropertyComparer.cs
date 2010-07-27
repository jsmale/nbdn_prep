﻿using System;
using System.Collections.Generic;

namespace nothinbutdotnetprep.utility.sorting
{
    public class RankedPropertyComparer<ItemToCompare, PropertyType> : IComparer<ItemToCompare>
    {
        Func<ItemToCompare, PropertyType> property_accessor;
        IDictionary<PropertyType, int> rankings;

        public RankedPropertyComparer(Func<ItemToCompare, PropertyType> property_accessor, 
                                    IEnumerable<PropertyType> rank_order)
        {
            this.property_accessor = property_accessor;
            initialize_rankings(rank_order);
        }

        public IComparer<ItemToCompare> then_by<PropertyType2>(Func<ItemToCompare, PropertyType2> property_accessor2)
            where PropertyType2 : IComparable<PropertyType2>
        {
            var to_chain = new ComparablePropertyComparer<ItemToCompare, PropertyType2>(property_accessor2);
            return new ChainedComparer<ItemToCompare>(this, to_chain);
        }

        private void initialize_rankings(IEnumerable<PropertyType> rank_order) 
        {
            rankings = new Dictionary<PropertyType, int>();
            var rank = 0;
            foreach (var item in rank_order)
            {
                rankings.Add(item, rank++);
            }
        }

        public int Compare(ItemToCompare x, ItemToCompare y)
        {
            return rankings[property_accessor(x)].CompareTo(rankings[property_accessor(y)]);
        }
    }
}