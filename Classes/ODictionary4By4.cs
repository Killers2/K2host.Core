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

    public class ODictionary4By4<I, K, V, S> : IEnumerable<V>, IDisposable
    {

        I[] pPairIndexes;
        K[] pPairKeys;
        V[] pPairValues;
        S[] pPairSubs;

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

        public S[] Subs
        {
            get { return pPairSubs; }
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

        public ODictionary4By4()
        {
            pPairIndexes    = Array.Empty<I>();
            pPairKeys       = Array.Empty<K>();
            pPairValues     = Array.Empty<V>();
            pPairSubs       = Array.Empty<S>();
        }

        public void Add(I index, K key, V value, S sub)
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

            S[] temp3 = new S[pPairSubs.Length];
            Array.Copy(pPairSubs, temp3, pPairSubs.Length);

            Array.Clear(pPairSubs, 0, pPairSubs.Length);
            pPairSubs = null;

            pPairSubs = new S[temp3.Length + 1];
            Array.Copy(temp3, pPairSubs, temp3.Length);

            Array.Clear(temp3, 0, temp3.Length);


            pPairIndexes[^1]    = index;
            pPairKeys[^1]       = key;
            pPairValues[^1]     = value;
            pPairSubs[^1]       = sub;

        }

        public void AddRange(ODictionary4By4<I, K, V, S> e)
        {

            List<I> til = pPairIndexes.ToList();
            List<K> tik = pPairKeys.ToList();
            List<V> tiv = pPairValues.ToList();
            List<S> tis = pPairSubs.ToList();


            til.AddRange(e.Indexes);
            tik.AddRange(e.Keys);
            tiv.AddRange(e.Values);
            tis.AddRange(e.Subs);

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            Array.Clear(pPairValues, 0, pPairValues.Length);
            Array.Clear(pPairSubs, 0, pPairSubs.Length);

            pPairIndexes = null;
            pPairKeys = null;
            pPairValues = null;
            pPairSubs = null;

            pPairIndexes = til.ToArray();
            pPairKeys = tik.ToArray();
            pPairValues = tiv.ToArray();
            pPairSubs = tis.ToArray();

            til.Clear();
            tik.Clear();
            tiv.Clear();
            tis.Clear();

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

        public void Remove(S sub)
        {

            int indexToRemove = -1;

            for (int i = 0; i < pPairSubs.Length; i++)
                if (pPairSubs[i].Equals(sub))
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


            S[] temp3 = new S[pPairSubs.Length - 1];
            Array.Copy(pPairSubs, 0, temp3, 0, indexToRemove);

            if (indexToRemove < (pPairSubs.Length - 1))
                Array.Copy(pPairSubs, (indexToRemove + 1), temp3, indexToRemove, (pPairSubs.Length - indexToRemove) - 1);

            Array.Clear(pPairSubs, 0, pPairSubs.Length);
            pPairSubs = null;

            pPairSubs = new S[temp3.Length];
            Array.Copy(temp3, pPairSubs, temp3.Length);

            Array.Clear(temp3, 0, temp3.Length);

        }

        public void Clear()
        {

            Array.Clear(pPairIndexes, 0, pPairIndexes.Length);
            pPairIndexes = null;

            Array.Clear(pPairKeys, 0, pPairKeys.Length);
            pPairKeys = null;

            Array.Clear(pPairValues, 0, pPairValues.Length);
            pPairValues = null;
          
            Array.Clear(pPairSubs, 0, pPairSubs.Length);
            pPairSubs = null;

        }

        public bool ContainsKey(K key)
        {
            if (pPairKeys.Where(i => i.Equals(key)).Any())
                return true;

            return false;

        }

        public bool ContainsValue(V value)
        {

            if (pPairValues.Where(i => i.Equals(value)).Any())
                return true;

            return false;

        }

        public bool ContainsIndex(I index)
        {

            if (pPairIndexes.Where(i => i.Equals(index)).Any())
                return true;

            return false;

        }

        public bool ContainsSub(S sub)
        {

            if(pPairSubs.Where(i => i.Equals(sub)).Any())
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

        public I Retrive(S sub)
        {
            for (int i = 0; i < pPairSubs.Length; i++)
                if (pPairSubs[i].Equals(sub))
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
