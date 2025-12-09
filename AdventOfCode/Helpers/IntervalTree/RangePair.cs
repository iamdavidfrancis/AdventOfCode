using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers.IntervalTree
{
    public readonly struct RangePair<TKey> : IEquatable<RangePair<TKey>>
    {
        public TKey From { get; }
        public TKey To { get; }

        public RangePair(TKey from, TKey to)
            : this()
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return $"[{From}, {To}]";
        }

        public bool Equals(RangePair<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(From, other.From)
                && EqualityComparer<TKey>.Default.Equals(To, other.To);
        }

        override public bool Equals(object? obj)
        {
            return obj is RangePair<TKey> other && Equals(other);
        }

        public static bool operator ==(RangePair<TKey> left, RangePair<TKey> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RangePair<TKey> left, RangePair<TKey> right)
        {
            return !(left == right);
        }
    }
}
