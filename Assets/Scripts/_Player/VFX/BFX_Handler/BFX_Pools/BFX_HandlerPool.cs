using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_HandlerPool : MonoBehaviour
    {
        [Header("Config.")]
        public int bfxId;
        public BFX_Handler prefab = null;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;
        Queue<BFX_Handler> objects = new Queue<BFX_Handler>();
        
        public BFX_Handler Get()
        {
            if (_objectsCount == 0)
            {
                BFX_Handler newHandler = Instantiate(prefab);
                newHandler.Init(this);
                return newHandler;
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        public void PreWarm()
        {
            BFX_Handler newHandler = Instantiate(prefab);
            newHandler.Init(this);

            ReturnToPool(newHandler);
        }

        /// Return To Pool.
        public void ReturnToPool(BFX_Handler handlerToReturn)
        {
            objects.Enqueue(handlerToReturn);
            _objectsCount++;

            handlerToReturn.ReturnToBackpack();
        }
    }
}