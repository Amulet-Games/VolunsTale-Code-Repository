using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_StickyUpdaterPool : MonoBehaviour
    {
        [SerializeField]
        private BFX_StickyUpdater prefab = null;

        [SerializeField]
        private Queue<BFX_StickyUpdater> objects = new Queue<BFX_StickyUpdater>();
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;
        
        public BFX_StickyUpdater Get()
        {
            if (_objectsCount == 0)
            {
                BFX_StickyUpdater newSticky = Instantiate(prefab);
                newSticky.Init();
                return newSticky;
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        public void PreWarm()
        {
            BFX_StickyUpdater newSticky = Instantiate(prefab);
            newSticky.Init();

            ReturnToPool(newSticky);
        }

        /// Return To Pool.
        public void ReturnToPool(BFX_StickyUpdater stickyToReturn)
        {
            objects.Enqueue(stickyToReturn);
            _objectsCount++;

            stickyToReturn.ReturnToBackpack();
        }
    }
}