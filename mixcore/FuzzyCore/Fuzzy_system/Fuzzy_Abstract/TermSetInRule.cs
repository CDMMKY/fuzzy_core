using System;
using System.Collections.Generic;

namespace FuzzySystem.FuzzyAbstract
{
    public sealed class TermSetInRule<T> : IList<T> where T : Term
    {
        List<T> dataListInRule = new List<T>();
        TermSetGlobal<T> TermSetGlobalChecker;


        public void AddTermSetGlobal(TermSetGlobal<T> TermSetGlobalSource)
        {
            TermSetGlobalChecker = TermSetGlobalSource;
        }

        public void RemoveTermSetGlobal(TermSetGlobal<T> TermSetGlobalSource)
        {
            TermSetGlobalChecker = null;
        }


        public int IndexOf(T item)
        {
            return dataListInRule.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            dataListInRule.Insert(index, item);
            if ((TermSetGlobalChecker !=null)&& !TermSetGlobalChecker.Contains(item))
            {
                TermSetGlobalChecker.Add(item);
            }
        }

        public void RemoveAt(int index)
        {
            dataListInRule.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return dataListInRule[index];
            }
            set
            {
                if ((TermSetGlobalChecker !=null)&& !TermSetGlobalChecker.Contains(value))
                {
                    TermSetGlobalChecker.Add(value);

                }
                dataListInRule[index] = value;

            }
        }

        public void Add(T item)
        {
            dataListInRule.Add(item);
            if ((TermSetGlobalChecker !=null)&& !TermSetGlobalChecker.Contains(item))
            {
                TermSetGlobalChecker.Add(item);
            }
        }

        public void Clear()
        {
            dataListInRule.Clear();
        }

        public bool Contains(T item)
        {
            return dataListInRule.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            dataListInRule.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dataListInRule.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {

            return dataListInRule.Remove(item);
        }

        public List<T> ToList()
        {
            return dataListInRule;
        }

        public int FindIndex(Predicate<T> match)
        {
            return dataListInRule.FindIndex(match);

        }
        public List<T> FindAll(Predicate<T> match)
        {
            return dataListInRule.FindAll(match);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return dataListInRule.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dataListInRule.GetEnumerator();
        }
    }

}



