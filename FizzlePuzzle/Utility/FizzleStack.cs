using FizzlePuzzle.Extension;
using System.Collections.Generic;

namespace FizzlePuzzle.Utility
{
    internal class FizzleStack<E>
    {
        private readonly LinkedList<E> linkedList;
        private readonly long capacity;

        internal long Count => linkedList.Count;

        internal FizzleStack(long capacity)
        {
            this.capacity = capacity;
            this.linkedList = new LinkedList<E>();
        }

        internal void Push(E element)
        {
            if (capacity <= 0L)
            {
                return;
            }
            if (Count == capacity)
            {
                linkedList.RemoveFirst();
            }
            linkedList.AddLast(element);
        }

        internal void Pour(FizzleStack<E> stack2)
        {
            while (Count > 0L)
            {
                stack2.Push(Pop());
            }
        }

        internal E Pop()
        {
            if (Count == 0L)
            {
                throw new FizzleException("Stack Empty");
            }
            try
            {
                return linkedList.Last.Value;
            }
            finally
            {
                linkedList.RemoveLast();
            }
        }

        internal void Clear()
        {
            linkedList.Clear();
        }
    }
}
