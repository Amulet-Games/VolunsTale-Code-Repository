using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_BFX_HandlerPool : MonoBehaviour
    {
        [Header("Id.")]
        public int _ai_Bfx_ID;

        [Header("Prefab (Drops).")]
        public AI_Poolable_BFX_Handler prefab = null;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;
        Queue<AI_Poolable_BFX_Handler> objects = new Queue<AI_Poolable_BFX_Handler>();

        public AI_Poolable_BFX_Handler Get()
        {
            if (_objectsCount == 0)
            {
                AI_Poolable_BFX_Handler newHandler = Instantiate(prefab);
                newHandler.Setup(this);
                return newHandler;
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        public void PreWarm()
        {
            AI_Poolable_BFX_Handler newHandler = Instantiate(prefab);
            newHandler.Setup(this);

            ReturnToPool(newHandler);
        }

        /// Return To Pool.
        public void ReturnToPool(AI_Poolable_BFX_Handler handlerToReturn)
        {
            objects.Enqueue(handlerToReturn);
            _objectsCount++;

            handlerToReturn.ReturnToBackpack();
        }
    }
}