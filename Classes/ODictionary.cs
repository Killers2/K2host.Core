/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace K2host.Core.Classes
{

    public class ODictionary<I, K, V> : IEnumerable<V>, IDisposable
    {

        I[] pPairIndexes;
        K[] pPairKeys;
        V[] pPairValues;

        public I[] Indexes
        {
            get { return pPairIndexes; }
        }

        public K[] Keys
        {
            get { return pPairKeys; }
        }

        public V[] Values
        {
            get { return pPairValues; }
        }

        public V this[I index]
        {
            get
            {
                for (int i = 0; i < pPairIndexes.Length; i++)
                {
                    if (pPairIndexes[i].Equals(index))
                        return pPairValues[i];
                }
                throw new Exception("This key was not found.");
            }
            set
            {
                for (int i = 0; i < pPairIndexes.Length; i++)
                {
                    if (pPairIndexes[i].Equals(index))
                        pPairValues[i] = value;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                for (int i = 0; i < pPairKeys.Length; i++)
                {
                    if (pPairKeys[i].Equals(key))
                        return pPairValues[i];
                }
                throw new Exception("This key was not found.");
            }
            set
            {
                for (int i = 0; i < pPairKeys.Length; i++)
                {
                    if (pPairKeys[i].Equals(key))
                        pPairValues[i] = value;
                }
            }
        }

        public K this[V val]
        {
            get
            {
                for (int i = 0; i < pPairValues.Length; i++)
                {
                    if (pPairValues[i].Equals(val))
                        return pPairKeys[i];
                }
                throw new Exception("This key was not found.");
            }
            set
            {
                for (int i = 0; i < pPairValues.Length; i++)
                {
                    if (pPairValues[i].Equals(val))
                        pPairKeys[i] = value;
                }
            }
        }

        public I this[V val, K key]
        {
            get
            {
                for (int i = 0; i < pPairValues.Length; i++)
                {
                    if (pPairValues[i].Equals(val))
                        return pPairIndexes[i];
                }
                throw new Exception("This key was not found.");
            }
            set
            {
                for (int i = 0; i < pPairValues.Length; i++)
                {
                    if (pPairValues[i].Equals(val))
                        pPairIndexes[i] = value;
                }
            }
        }

        public V this[int pos]
        {
            get
            {

                if (pos > pPairValues.Length)
                    throw new Exception("This key was not found.");

                return pPairValues[pos];

            }
            set
            {

                if (pos > pPairValues.Length)
                    throw new Exception("This key was not found.");

                pPairValues[pos] = value;

            }
        }

        public ODictionary()
        {
            pPairIndexes    = Array.Empty<I>();
            pPairKeys       = Array.Empty<K>();
            pPairValues     = Array.Empty<V>();
        }

        public void Add(I index, K key, V value)
        {

            I[] temp0 = new I[pPairIndexes.Length];
            Array.Copy(pPairIndexes, temp0, pPairIndexes.Length);

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            pPairIndexes = null;

            pPairIndexes = new I[temp0.Length + 1];
            Array.Copy(temp0, pPairIndexes, temp0.Length);

            Array.Clear(temp0, 0, temp0.Length);

            K[] temp1 = new K[pPairKeys.Length];
            Array.Copy(pPairKeys, temp1, pPairKeys.Length);

            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            pPairKeys = null;

            pPairKeys = new K[temp1.Length + 1];
            Array.Copy(temp1, pPairKeys, temp1.Length);

            Array.Clear(temp1, 0, temp1.Length);

            V[] temp2 = new V[pPairValues.Length];
            Array.Copy(pPairValues, temp2, pPairValues.Length);

            Array.Clear(pPairValues, 0, pPairValues.Length);
            pPairValues = null;

            pPairValues = new V[temp2.Length + 1];
            Array.Copy(temp2, pPairValues, temp2.Length);

            Array.Clear(temp2, 0, temp2.Length);

            pPairIndexes[^1]    = index;
            pPairKeys[^1]       = key;
            pPairValues[^1]     = value;

        }

        public void AddRange(ODictionary<I, K, V> e)
        {

            List<I> til = pPairIndexes.ToList();
            List<K> tik = pPairKeys.ToList();
            List<V> tiv = pPairValues.ToList();


            til.AddRange(e.Indexes);
            tik.AddRange(e.Keys);
            tiv.AddRange(e.Values);

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            Array.Clear(pPairValues, 0, pPairValues.Length);

            pPairIndexes = null;
            pPairKeys = null;
            pPairValues = null;

            pPairIndexes = til.ToArray();
            pPairKeys = tik.ToArray();
            pPairValues = tiv.ToArray();

            til.Clear();
            tik.Clear();
            tiv.Clear();

        }

        public void Remove(V val)
        {

            int indexToRemove = -1;

            for (int i = 0; i < pPairValues.Length; i++)
                if (pPairValues[i].Equals(val))
                {
                    indexToRemove = i;
                    break;
                }

            if (indexToRemove == -1)
                throw new Exception("This value does not exsit.");

            RemoveWithIndex(indexToRemove);
        }

        public void Remove(I index)
        {

            int indexToRemove = -1;

            for (int i = 0; i < pPairIndexes.Length; i++)
                if (pPairIndexes[i].Equals(index))
                {
                    indexToRemove = i;
                    break;
                }

            if (indexToRemove == -1)
                throw new Exception("This index does not exsit.");


            RemoveWithIndex(indexToRemove);
        }

        public void Remove(K key)
        {

            int indexToRemove = -1;

            for (int i = 0; i < pPairKeys.Length; i++)
                if (pPairKeys[i].Equals(key))
                {
                    indexToRemove = i;
                    break;
                }

            if (indexToRemove == -1)
                throw new Exception("This key does not exsit.");


            RemoveWithIndex(indexToRemove);

        }

        public void RemoveAt(int index)
        {

            if (index > (pPairKeys.Length - 1))
                throw new Exception("This index was out of range.");

            RemoveWithIndex(index);

        }

        private void RemoveWithIndex(int indexToRemove)
        {


            I[] temp0 = new I[pPairIndexes.Length - 1];
            Array.Copy(pPairIndexes, 0, temp0, 0, indexToRemove);

            if (indexToRemove < (pPairIndexes.Length - 1))
                Array.Copy(pPairIndexes, (indexToRemove + 1), temp0, indexToRemove, (pPairIndexes.Length - indexToRemove) - 1);

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            pPairIndexes = null;

            pPairIndexes = new I[temp0.Length];
            Array.Copy(temp0, pPairIndexes, temp0.Length);

            Array.Clear(temp0, 0, temp0.Length);


            K[] temp1 = new K[pPairKeys.Length - 1];
            Array.Copy(pPairKeys, 0, temp1, 0, indexToRemove);

            if (indexToRemove < (pPairKeys.Length - 1))
                Array.Copy(pPairKeys, (indexToRemove + 1), temp1, indexToRemove, (pPairKeys.Length - indexToRemove) - 1);

            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            pPairKeys = null;

            pPairKeys = new K[temp1.Length];
            Array.Copy(temp1, pPairKeys, temp1.Length);

            Array.Clear(temp1, 0, temp1.Length);


            V[] temp2 = new V[pPairValues.Length - 1];
            Array.Copy(pPairValues, 0, temp2, 0, indexToRemove);

            if (indexToRemove < (pPairValues.Length - 1))
                Array.Copy(pPairValues, (indexToRemove + 1), temp2, indexToRemove, (pPairValues.Length - indexToRemove) - 1);

            Array.Clear(pPairValues, 0, pPairValues.Length);
            pPairValues = null;

            pPairValues = new V[temp2.Length];
            Array.Copy(temp2, pPairValues, temp2.Length);

            Array.Clear(temp2, 0, temp2.Length);

        }

        public void Clear()
        {

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            pPairIndexes = null;

            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            pPairKeys = null;

            Array.Clear(pPairValues, 0, pPairValues.Length);
            pPairValues = null;

        }

        public bool ContainsKey(K key)
        {

            for (int i = 0; i < pPairKeys.Length; i++)
                if (pPairKeys[i].Equals(key))
                    return true;

            return false;

        }

        public bool ContainsValue(V value)
        {

            for (int i = 0; i < pPairValues.Length; i++)
                if (pPairValues[i].Equals(value))
                    return true;

            return false;

        }

        public bool ContainsIndex(I index)
        {

            for (int i = 0; i < pPairIndexes.Length; i++)
                if (pPairIndexes[i].Equals(index))
                    return true;

            return false;

        }

        public K Retrive(I index)
        {
            for (int i = 0; i < pPairIndexes.Length; i++)
                if (pPairIndexes[i].Equals(index))
                    return pPairKeys[i];

            throw new Exception("This key was not found.");
        }

        public I Retrive(V val)
        {
            for (int i = 0; i < pPairValues.Length; i++)
                if (pPairValues[i].Equals(val))
                    return Indexes[i];

            throw new Exception("This key was not found.");
        }

        public I Retrive(K key)
        {
            for (int i = 0; i < pPairKeys.Length; i++)
                if (pPairKeys[i].Equals(key))
                    return Indexes[i];

            throw new Exception("This key was not found.");
        }

        public I First()
        {

            return Indexes[0];

        }

        public I Last()
        {

            return Indexes[^1];

        }

        public IEnumerator<V> GetEnumerator()
        {
            return (IEnumerator<V>)pPairValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)pPairValues).GetEnumerator();
        }

        #region Deconstuctor

        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
                if (disposing)
                {
                    Clear();

                }
            IsDisposed = true;
        }

        #endregion

    }

}
