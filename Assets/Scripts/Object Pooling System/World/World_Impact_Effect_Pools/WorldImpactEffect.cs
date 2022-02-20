using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* GameObject needs to be DEACTIVATED in Prefab.


    public abstract class WorldImpactEffect : MonoBehaviour
    {
        [Header("Particle System.")]
        public ParticleSystem worldImpactFx;

        [Header("Refs.")]
        [ReadOnlyInspector] public GameManager gameManager;
        [ReadOnlyInspector] public Transform mTransform;

        /// Callback.
        public void OnParticleSystemStopped()
        {
            ReturnEffectToPool();
        }

        /// Public.
        public void SceneObjectsHit_SpawnEffect(Vector3 _spawnPoint)
        {
            mTransform.SetParent(null);
            mTransform.position = _spawnPoint;

            gameObject.SetActive(true);
            worldImpactFx.Play();
        }

        public void BlockingHit_SpawnEffect()
        {
            gameObject.SetActive(true);
            worldImpactFx.Play();
        }

        public void SpawnInParent(Transform _parent)
        {
            mTransform.SetParent(_parent);
            mTransform.localPosition = gameManager.vector3Zero;

            gameObject.SetActive(true);
            worldImpactFx.Play();
        }

        public void ReturnToBackpack()
        {
            gameObject.SetActive(false);
            mTransform.parent = gameManager._WI_Effect_Bp;
        }

        /// Player
        public void Player_ParryHit_SpawnEffect(Vector3 _spawnPoint)
        {
            mTransform.SetParent(null);
            mTransform.position = _spawnPoint;

            gameObject.SetActive(true);
            worldImpactFx.Play();
        }

        /// AI
        public void AI_ParryHit_SpawnEffect()
        {
            gameObject.SetActive(true);
            worldImpactFx.Play();
        }

        /// Setup.
        public void Setup()
        {
            SetupRefs();

            void SetupRefs()
            {
                gameManager = GameManager.singleton;
                mTransform = transform;
            }
        }

        /// Abstract.
        public abstract void ReturnEffectToPool();
    }
}