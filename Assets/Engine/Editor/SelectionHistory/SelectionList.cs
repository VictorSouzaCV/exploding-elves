#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace ExplodingElves.Editor
{
    public class SelectionList : List<Object>
    {
        readonly int maxSize;
        int currentIndex;
        public bool HasPrevious => Count > 1 && currentIndex > 0;
        public bool HasNext => Count > 1 && currentIndex < Count-1;

        public SelectionList(int maxSize)
        {
            this.maxSize = maxSize;
        }

        public void Insert(Object insertion)
        {
            if (Count >= maxSize)
            {
                RemoveAt(0);
            }
            
            Add(insertion);
            currentIndex = Count - 1;
        }

        public Object SelectPrevious()
        {
            if (Count == 0)
            {
                return null;
            }
            
            currentIndex = Mathf.Clamp(currentIndex-1, 0, Count-1);
            return this[currentIndex];
        }
        
        public Object SelectNext()
        {
            if (Count == 0)
            {
                return null;
            }
            
            currentIndex = Mathf.Clamp(currentIndex+1, 0, Count-1);
            return this[currentIndex];
        }
    }
}
#endif
