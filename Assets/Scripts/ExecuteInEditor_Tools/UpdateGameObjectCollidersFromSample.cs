using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SA
{
    [ExecuteInEditMode]
    public class UpdateGameObjectCollidersFromSample : MonoBehaviour
    {
        /*
        public List<GameObject> objectsHaveColliders;
        public List<GameObject> objectsNeedsColliders;
        private Collider[] collidersFromOriginals;

        public bool copy;

        private void Update()
        {
            if(copy)
            {
                Debug.Log("is Copied");

                // For Every Objects have Colliders
                for (int i = 0; i < objectsHaveColliders.Count; i++)
                {
                    // Get Colliders From current object
                    collidersFromOriginals = objectsHaveColliders[i].GetComponents<Collider>();
                    if(collidersFromOriginals.Length > 0)
                    {
                        // If Colliders are not null, loop through every colliders in this array j
                        for (int j = 0; j < collidersFromOriginals.Length; j++)
                        {
                            UnityEditorInternal.ComponentUtility.CopyComponent(collidersFromOriginals[j]);
                            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(objectsNeedsColliders[i]);
                        }

                        // Clear the Collider's array
                        Array.Clear(collidersFromOriginals, 0, collidersFromOriginals.Length - 1);
                    }
                }

                copy = false;
                objectsHaveColliders.Clear();
                objectsNeedsColliders.Clear();
            }
        }
        */
    }
}