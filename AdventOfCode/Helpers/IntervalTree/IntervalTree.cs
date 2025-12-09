using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers.IntervalTree
{
    public class IntervalTree<TKey> : IIntervalTree<TKey>
    {
        private IntervalTreeNode<TKey> root;
        private List<RangePair<TKey>> items;
        private readonly IComparer<TKey> comparer;
        private bool isInSync;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TKey? Max
        {
            get
            {
                if (!this.isInSync)
                {
                    this.Rebuild();
                }

                return this.root.Max;
            }
        }

        public TKey? Min
        {
            get
            {
                if (!this.isInSync)
                {
                    this.Rebuild();
                }

                return this.root.Min;
            }
        }

        /// <summary>
        /// Initializes an empty tree.
        /// </summary>
        public IntervalTree() : this(Comparer<TKey>.Default) { }

        /// <summary>
        /// Initializes an empty tree.
        /// </summary>
        public IntervalTree(IComparer<TKey> comparer)
        {
            this.comparer = comparer ?? Comparer<TKey>.Default;
            isInSync = true;
            root = new IntervalTreeNode<TKey>(this.comparer);
            items = [];
        }

        public int Count => this.items.Count;

        public void Add(TKey from, TKey to)
        {
            if (this.comparer.Compare(from, to) > 0)
            {
                throw new ArgumentException("The 'from' value must be less than or equal to the 'to' value.");
            }

            this.isInSync = false;
            items.Add(new RangePair<TKey>(from, to));
        }

        public void Clear()
        {
            this.root = new IntervalTreeNode<TKey>(comparer);
            this.items = [];
            this.isInSync = true;
        }

        public bool Contains(TKey value)
        {
            if (!this.isInSync)
            {
                this.Rebuild();
            }

            return this.root.Contains(value);
        }

        public bool Contains(TKey from, TKey to)
        {
            if (!this.isInSync)
            {
                this.Rebuild();
            }

            return this.root.Contains(from, to);
        }

        public IEnumerator<RangePair<TKey>> GetEnumerator()
        {
            if (!this.isInSync)
            {
                this.Rebuild();
            }

            return this.items.GetEnumerator();
        }

        private void Rebuild()
        {
            if (this.isInSync)
            {
                return;
            }

            if (items.Count > 0)
            {
                this.root = new IntervalTreeNode<TKey>(this.items, this.comparer);
            }
            else
            {
                this.root = new IntervalTreeNode<TKey>(this.comparer);
            }

            this.isInSync = true;
        }
    }
}
