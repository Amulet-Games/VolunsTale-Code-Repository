using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SA
{
    [ExecuteInEditMode]
    public class SortGameObjectByMaterialName : MonoBehaviour
    {
        public List<GameObject> objectsToAddInParent;
        public List<GameObject> parentGameObject;
        public bool Add;
        public bool firstTime;
        
        // Update is called once per frame
        void Update()
        {
            if(Add)
            {
                firstTime = true;

                for (int i = 0; i < objectsToAddInParent.Count; i++)
                {
                    MeshRenderer objectMeshRenderer = objectsToAddInParent[i].GetComponentInChildren<MeshRenderer>();
                    string currentMaterialName = objectMeshRenderer.sharedMaterial.name;

                    bool noResult = true;

                    if (firstTime)
                    {
                        firstTime = false;

                        GameObject newParentObject = new GameObject(currentMaterialName);
                        newParentObject.transform.position = Vector3.zero;
                        newParentObject.transform.rotation = Quaternion.identity;
                        newParentObject.transform.localScale = Vector3.one;
                        parentGameObject.Add(newParentObject);
                        objectsToAddInParent[i].transform.parent = newParentObject.transform;
                    }
                    else
                    {
                        for (int j = 0; j < parentGameObject.Count; j++)
                        {
                            if (parentGameObject[j].name == currentMaterialName)
                            {
                                objectsToAddInParent[i].transform.parent = parentGameObject[j].transform;
                                noResult = false;
                            }
                        }

                        if(noResult)
                        {
                            GameObject newParentObject = new GameObject(currentMaterialName);
                            newParentObject.transform.position = Vector3.zero;
                            newParentObject.transform.rotation = Quaternion.identity;
                            newParentObject.transform.localScale = Vector3.one;
                            parentGameObject.Add(newParentObject);
                            objectsToAddInParent[i].transform.parent = newParentObject.transform;
                        }
                    }
                }

                Debug.Log("isOver");
                Add = false;
                objectsToAddInParent.Clear();
                parentGameObject.Clear();
            }
        }
    }
}