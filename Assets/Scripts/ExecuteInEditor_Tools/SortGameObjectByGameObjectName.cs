using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [ExecuteInEditMode]
    public class SortGameObjectByGameObjectName : MonoBehaviour
    {
        public List<GameObject> objectsToAddInParent;
        private List<GameObject> parentGameObject = null;
        public bool Add;
        private bool firstTime;

        private void Update()
        {
            if(Add)
            {
                Debug.Log("isPressed");

                firstTime = true;

                for (int i = 0; i < objectsToAddInParent.Count; i++)
                {
                    string gameobjectName = objectsToAddInParent[i].name;

                    bool noResult = true;

                    if(firstTime)
                    {
                        firstTime = false;

                        GameObject newParentObject = new GameObject(gameobjectName);
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
                            if (parentGameObject[j].name == gameobjectName)
                            {
                                objectsToAddInParent[i].transform.parent = parentGameObject[j].transform;
                                noResult = false;
                            }
                        }

                        if(noResult)
                        {
                            GameObject newParentObject = new GameObject(gameobjectName);
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