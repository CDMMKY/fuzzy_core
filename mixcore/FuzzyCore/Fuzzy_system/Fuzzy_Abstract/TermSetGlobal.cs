using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract
{
    public sealed class TermSetGlobal<T> : IList<T> where T : Term
    {

        List<T> dataList = new List<T>();
        List<TermSetInRule<T>> Dependency = new List<TermSetInRule<T>>();


        public void Trim()
        {
            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                if (Dependency.Where(x => x.Contains(dataList[i])).Count() == 0)

                    dataList.RemoveAt(i);
            }
        }


        public void AddDependencyRule(TermSetInRule<T> DependencySource)
        {
            Dependency.Add(DependencySource);
        }

        public void RemoveDependencyRule(TermSetInRule<T> DependencySource)
        {
            Dependency.Remove(DependencySource);
        }


        public int IndexOf(T item)
        {
            return dataList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            dataList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            T oldValue = dataList[index];
            Dependency.Where(x => x.Contains(oldValue)).AsParallel().ForAll(y => y.Remove(oldValue));
            dataList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return dataList[index];
            }
            set
            {

                T oldValue = dataList[index];
                Dependency.Where(x => x.Contains(oldValue)).AsParallel().ForAll(y => { y.Remove(oldValue); y.Add(value); });
                dataList[index] = value;

            }
        }

        public void Add(T item)
        {
            dataList.Add(item);
        }

        public void Clear()
        {
            Dependency.AsParallel().ForAll(y => y.Clear());
            Dependency.Clear();
            dataList.Clear();
        }

        public bool Contains(T item)
        {
            return dataList.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            dataList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                
                return dataList.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        { lock (Dependency) { 
            Dependency.Where(x => x.Contains(item)).AsParallel().ForAll(y => y.Remove(item));
            }   
            bool res = false;
            lock (dataList) { res = dataList.Remove(item); }
            return res;
        }

        public List<T> ToList()
        {
            return dataList;
        }

        public int FindIndex(Predicate<T> match)
        {
            return dataList.FindIndex(match);

        }
        public List<T> FindAll(Predicate<T> match)
        {
            return dataList.FindAll(match);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return dataList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dataList.GetEnumerator();
        }
    }




}
