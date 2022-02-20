using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class NavigatableGroupManager : MonoBehaviour
    {
        INavigatable[] navigatables;

        INavigatable _current;
        INavigatable currentNavigatable
        {
            get { return _current; }
            set
            {
                if (_current != null)
                {
                    _current.OnDeselect();
                }

                _current = value;
                _current.OnSelect();
            }
        }

        private void Start()
        {
            navigatables = GetComponentsInChildren<INavigatable>();
            currentNavigatable = navigatables[0];
        }

        public Vector3 debugDirection;
        public bool debugMove;

        private void Update()
        {
            if(debugMove)
            {
                debugMove = false;
                Navigate(debugDirection);
            }
        }

        public void Navigate(Vector3 targetDirection)
        {
            Transform c = currentNavigatable.GetTransform();
            INavigatable result = GetResult(c.position, targetDirection, c);
            

            if(result != null)
            {
                currentNavigatable = result;
            }
            else
            {
                result = GetResult(c.position + Vector3.up * 2000, targetDirection, c);
                if(result != null)
                {
                    currentNavigatable = result;
                }
            }
        }

        public INavigatable GetResult(Vector3 origin, Vector3 targetDirection, Transform originTransform)
        {
            INavigatable result = null;
            float minDis = float.MaxValue;

            for (int i = 0; i < navigatables.Length; i++)
            {
                Transform t = navigatables[i].GetTransform();
                if (t == originTransform)
                    continue;

                Vector3 dir = t.position - origin;
                StaticHelper.NormalizedVector3(dir);

                float angle = Vector3.SignedAngle(targetDirection, dir, Vector3.up);
                if (angle < 45)
                {
                    float tempDis = Vector3.Distance(t.position, origin);
                    if (tempDis < minDis)
                    {
                        minDis = tempDis;
                        result = navigatables[i];
                    }
                }
            }

            return result;
        }
    }

    public interface INavigatable
    {
        void OnSelect();
        void OnDeselect();
        Transform GetTransform();
    }
}