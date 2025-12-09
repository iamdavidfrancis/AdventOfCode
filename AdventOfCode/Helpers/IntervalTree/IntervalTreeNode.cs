using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers.IntervalTree
{
    internal class IntervalTreeNode<TKey> : IComparer<RangePair<TKey>>
    {
        private readonly TKey? center;

        private readonly IComparer<TKey> comparer;
        private readonly RangePair<TKey>[]? items;
        private readonly IntervalTreeNode<TKey>? leftNode;
        private readonly IntervalTreeNode<TKey>? rightNode;

        public IntervalTreeNode(IComparer<TKey>? comparer)
        {
            this.comparer = comparer ?? Comparer<TKey>.Default;

            this.center = default;
            this.leftNode = null;
            this.rightNode = null;
            this.items = null;

            this.Min = default;
            this.Max = default;
        }

        public IntervalTreeNode(IList<RangePair<TKey>> items, IComparer<TKey>? comparer = null)
        {
            this.comparer = comparer ?? Comparer<TKey>.Default;

            var endPoints = new List<TKey>(items.Count * 2);
            
            // Find the median
            foreach (var item in items)
            {
                endPoints.Add(item.From);
                endPoints.Add(item.To);
            }

            endPoints.Sort(this.comparer);

            if (endPoints.Count > 0)
            {
                Min = endPoints[0];
                Max = endPoints[endPoints.Count - 1];
                this.center = endPoints[endPoints.Count / 2];
            }
            else
            {
                throw new ArgumentException("No ranges were added.");
            }

            var inner = new List<RangePair<TKey>>();
            var left = new List<RangePair<TKey>>();
            var right = new List<RangePair<TKey>>();

            // iterate over all items
            // if the range of an item is completely left of the center, add it to the left items
            // if it is on the right of the center, add it to the right items
            // otherwise (range overlaps the center), add the item to this node's items
            foreach (var item in items)
            {
                if (this.comparer.Compare(item.To, center) < 0)
                {
                    left.Add(item);
                }
                else if (this.comparer.Compare(item.From, center) > 0)
                {
                    right.Add(item);
                }
                else
                {
                    inner.Add(item);
                }
            }

            // sort the items, this way the query is faster later on
            if (inner.Count > 0)
            {
                if (inner.Count > 1)
                { 
                    inner.Sort(this);
                }

                this.items = [.. inner];
            }
            else
            {
                this.items = null;
            }

            // create left and right nodes, if there are any items
            if (left.Count > 0)
            {
                this.leftNode = new IntervalTreeNode<TKey>(left, this.comparer);
            }
            if (right.Count > 0)
            {
                this.rightNode = new IntervalTreeNode<TKey>(right, this.comparer);
            }
        }

        public TKey? Max { get; }

        public TKey? Min { get; }

        int IComparer<RangePair<TKey>>.Compare(RangePair<TKey> x, RangePair<TKey> y)
        {
            var fromComparison = this.comparer.Compare(x.From, y.From);

            if (fromComparison == 0)
            {
                return this.comparer.Compare(x.To, y.To);
            }

            return fromComparison;
        }

        public bool Contains(TKey value)
        {
            if (this.items != null)
            {
                foreach (var item in this.items)
                {
                    if (this.comparer.Compare(item.From, value) > 0)
                    {
                        break;
                    }
                    else if (this.comparer.Compare(value, item.From) >= 0
                        && this.comparer.Compare(value, item.To) <= 0)
                    {
                        return true;
                    }
                }
            }

            var centerComparison = this.comparer.Compare(value, center);

            if (this.leftNode != null && centerComparison < 0)
            {
                return this.leftNode.Contains(value);
            }
            else if (this.rightNode != null && centerComparison > 0)
            {
                return this.rightNode.Contains(value);
            }

            return false;
        }

        public bool Contains(TKey from, TKey to)
        {
            if (this.items != null)
            {
                foreach (var item in this.items)
                {
                    if (this.comparer.Compare(item.From, to) > 0)
                    {
                        break;
                    }
                    else if (this.comparer.Compare(to, item.From) >= 0
                        && this.comparer.Compare(from, item.To) <= 0)
                    {
                        return true;
                    }
                }
            }

            if (this.leftNode != null && this.comparer.Compare(to, center) < 0)
            {
                return this.leftNode.Contains(from, to);
            }
            else if (this.rightNode != null && this.comparer.Compare(to, center) > 0)
            {
                return this.rightNode.Contains(from, to);
            }

            return false;
        }
    }
}
