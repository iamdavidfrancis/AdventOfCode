using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class RangeEx
    {
        public static RangeEnumerator GetEnumerator(this Range range)
        {
            if (range.Start.IsFromEnd || range.End.IsFromEnd)
            {
                throw new ArgumentException(nameof(range));
            }

            return new RangeEnumerator(range.Start.Value, range.End.Value);
        }

        public struct RangeEnumerator : IEnumerator<int>
        {
            private readonly int _end;
            private int _current;

            public RangeEnumerator(int start, int end)
            {
                _current = start - 1;
                _end = end + 1; // Include end number in range
            }

            public int Current => _current;
            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext() => ++_current < _end;

            public void Dispose() { }
            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
