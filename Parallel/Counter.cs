using System;

namespace Parallel
{
    class Counter: IComparable<Counter>
    {
        public int Value { get; set; }
        public static Random r = new Random();

        public Counter()
        {
            Value = r.Next();
        }       

        public int CompareTo(Counter other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
