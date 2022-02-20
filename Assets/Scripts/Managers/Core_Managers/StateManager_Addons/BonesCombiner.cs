using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BonesCombiner
    {
        public Dictionary<int, Transform> _RootBoneDictionary = new Dictionary<int, Transform>();
        private Transform[] _boneTransforms = new Transform[67];

        private Transform _transform;

        public BonesCombiner(GameObject _rootObj)
        {
            _transform = _rootObj.transform;
            TraverseHierarchy(_transform);
        }

        public Transform AddLimb(GameObject _bonedObj)
        {
            Transform limb = ProcessBonedObject(_bonedObj.GetComponentInChildren<SkinnedMeshRenderer>());
            limb.SetParent(_transform);
            return _transform;
        }

        private Transform ProcessBonedObject(SkinnedMeshRenderer _renderer)
        {
            Transform bonedObject = new GameObject().transform;

            SkinnedMeshRenderer meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

            Transform[] bones = _renderer.bones;
            for (int i = 0; i < _renderer.bones.Length; i++)
            {
                _boneTransforms[i] = _RootBoneDictionary[bones[i].name.GetHashCode()];
            }

            meshRenderer.bones = _boneTransforms;
            meshRenderer.sharedMesh = _renderer.sharedMesh;
            meshRenderer.sharedMaterials = _renderer.sharedMaterials;

            return bonedObject;
        }

        private void TraverseHierarchy(Transform _transform)
        {
            for (int i = 0; i < _transform.childCount; i++)
            {
                Transform childTransform = _transform.GetChild(i);
                _RootBoneDictionary.Add(childTransform.name.GetHashCode(), childTransform);
                TraverseHierarchy(childTransform);
            }
        }
    }
}