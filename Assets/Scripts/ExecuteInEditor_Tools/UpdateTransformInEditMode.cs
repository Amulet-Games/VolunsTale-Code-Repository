using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [ExecuteInEditMode]
    public class UpdateTransformInEditMode : MonoBehaviour
    {
        public List<GameObject> objectCopyForm;
        public List<GameObject> objectCopyTo;
        public bool copy;

        // Update is called once per frame
        void Update()
        {
            if (copy)
            {
                for (int i = 0; i < objectCopyForm.Count; i++)
                {
                    objectCopyTo[i].transform.position = objectCopyForm[i].transform.position;
                    objectCopyTo[i].transform.rotation = objectCopyForm[i].transform.rotation;
                    objectCopyTo[i].transform.localScale = objectCopyForm[i].transform.localScale;
                }
                
                copy = false;
                objectCopyForm.Clear();
                objectCopyTo.Clear();
                Debug.Log("transform copied");
            }
        }
    }
}