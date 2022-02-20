using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA
{
    [ExecuteInEditMode]
    public class UpdateGameObjectStaticTags : MonoBehaviour
    {
        /*
        public List<GameObject> objectsToChangeStatic;
        public bool change;
        public bool add;

        // Update is called once per frame
        void Update()
        {
            if (change)
            {
                //StaticEditorFlags flags = StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic;
                //StaticEditorFlags flags = StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.ReflectionProbeStatic;
                //StaticEditorFlags flags = StaticEditorFlags.OccludeeStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.ReflectionProbeStatic;
                //StaticEditorFlags flags = StaticEditorFlags.ReflectionProbeStatic;
                StaticEditorFlags flags = StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.BatchingStatic;

                for (int i = 0; i < objectsToChangeStatic.Count; i++)
                {
                    if (objectsToChangeStatic[i] == null)
                        continue;

                    string objectIName = objectsToChangeStatic[i].gameObject.name;

                    GameObjectUtility.SetStaticEditorFlags(objectsToChangeStatic[i], flags);
                }

                change = false;
                objectsToChangeStatic.Clear();
                Debug.Log("Static Changed");
            }

            if(add)
            {
                for (int i = 0; i < objectsToChangeStatic.Count; i++)
                {
                    if (objectsToChangeStatic[i] == null)
                        continue;

                    //string objectIName = objectsToChangeStatic[i].gameObject.name;

                    //bool isEligible = (objectIName == "LOD_0" || objectIName == "LOD_1" || objectIName == "LOD_2") ? true : false;

                    //if (!isEligible)
                    //    continue;

                    StaticEditorFlags currentFlags =  GameObjectUtility.GetStaticEditorFlags(objectsToChangeStatic[i]);

                    currentFlags |= StaticEditorFlags.NavigationStatic;

                    GameObjectUtility.SetStaticEditorFlags(objectsToChangeStatic[i], currentFlags);
                }

                add = false;
                objectsToChangeStatic.Clear();
                Debug.Log("Static Add");
            }
        }
        */
    }
}