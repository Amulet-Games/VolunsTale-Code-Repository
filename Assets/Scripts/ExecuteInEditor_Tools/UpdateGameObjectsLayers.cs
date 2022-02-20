using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [ExecuteInEditMode]
    public class UpdateGameObjectsLayers : MonoBehaviour
    {
        public List<GameObject> objectsToChangeLayers;
        public bool change;

        void Update()
        {
            if (change)
            {
                for (int i = 0; i < objectsToChangeLayers.Count; i++)
                {
                    if (objectsToChangeLayers[i] == null)
                        continue;

                    if (objectsToChangeLayers[i].name == "Walkable_Stepable")
                    {
                        GameObject _firstLayerGameObject = objectsToChangeLayers[i];
                        for (int j = 0; j < _firstLayerGameObject.transform.childCount; j++)
                        {
                            GameObject _secondLayerGameObject = _firstLayerGameObject.transform.GetChild(j).gameObject;
                            if (_secondLayerGameObject.layer != 17)
                            {
                                _secondLayerGameObject.layer = 17;
                                for (int z = 0; z < _secondLayerGameObject.transform.childCount; z++)
                                {
                                    _secondLayerGameObject.transform.GetChild(z).gameObject.layer = 17;
                                }
                            }
                        }
                    }
                    else if (objectsToChangeLayers[i].name == "UnWalkable")
                    {
                        GameObject _firstLayerGameObject = objectsToChangeLayers[i];
                        for (int j = 0; j < _firstLayerGameObject.transform.childCount; j++)
                        {
                            GameObject _secondLayerGameObject = _firstLayerGameObject.transform.GetChild(j).gameObject;
                            if (_secondLayerGameObject.layer != 18)
                            {
                                _secondLayerGameObject.layer = 18;
                                for (int z = 0; z < _secondLayerGameObject.transform.childCount; z++)
                                {
                                    _secondLayerGameObject.transform.GetChild(z).gameObject.layer = 18;
                                }
                            }
                        }
                    }
                    else if (objectsToChangeLayers[i].name == "Walkable_UnStepable")
                    {
                        GameObject _firstLayerGameObject = objectsToChangeLayers[i];
                        for (int j = 0; j < _firstLayerGameObject.transform.childCount; j++)
                        {
                            GameObject _secondLayerGameObject = _firstLayerGameObject.transform.GetChild(j).gameObject;
                            if (_secondLayerGameObject.layer != 19)
                            {
                                _secondLayerGameObject.layer = 19;
                                for (int z = 0; z < _secondLayerGameObject.transform.childCount; z++)
                                {
                                    _secondLayerGameObject.transform.GetChild(z).gameObject.layer = 19;
                                }
                            }
                        }
                    }
                }

                change = false;
                objectsToChangeLayers.Clear();
                Debug.Log("Static Changed");
            }
        }
    }
}