using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers.IntervalTree
{
    internal interface IIntervalTree<TKey> : IEnumerable<RangePair<TKey>>
    {
        int Count { get; }

        bool Contains(TKey value);

        bool Contains(TKey from, TKey to);

        void Add(TKey from, TKey to);

        void Clear();
    }
}
