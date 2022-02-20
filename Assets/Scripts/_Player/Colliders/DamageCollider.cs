using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public Collider _collider = null;
        [ReadOnlyInspector] public RuntimeWeapon _runtimeWeapon = null;

        [Header("Status.")]
        [ReadOnlyInspector] public bool _hasPlayedSceneObjImpactEffect;

        [HideInInspector] public int enemyLayer;
        [HideInInspector] public int enemyDamageColliderLayer;
        
        public void Init()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
            _collider.isTrigger = true;

            LayerManager _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.playerDamageColliderLayer;
            enemyLayer = _layerManager.enemyLayer;
            enemyDamageColliderLayer = _layerManager.enemyDamageColliderLayer;

            _hasPlayedSceneObjImpactEffect = false;
        }

        public void SetColliderStatusToTrue()
        {
            _collider.enabled = true;
            _hasPlayedSceneObjImpactEffect = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_runtimeWeapon._states._isParrying)
            {
                ParryOnTriggerEnter(other);
            }
            else
            {
                NonParryOnTriggerEnter(other);
            }
        }

        void ParryOnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == enemyDamageColliderLayer)
            {
                BaseEnemyDamageCollider e_dc = other.GetComponent<BaseEnemyDamageCollider>();
                if (e_dc)
                {
                    if (e_dc._ai._isParryable)
                    {
                        e_dc._ai._isInParryExecuteWindow = true;

                        _collider.enabled = false;

                        PlayParryImpactEffectOnPoint();
                    }
                }
            }

            void PlayParryImpactEffectOnPoint()
            {
                GameManager.singleton._parryImpactEffect.SceneObjectsHit_SpawnEffect(other.ClosestPoint(transform.position));
            }
        }

        void NonParryOnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == enemyLayer)
            {
                AIManager aiManager = other.GetComponent<AIManager>();
                if (!aiManager._isInvincible)
                {
                    aiManager.OnHit(this);

                    _collider.enabled = false;
                }
            }
            else
            {
                if (!_hasPlayedSceneObjImpactEffect)
                {
                    if (other.gameObject.CompareTag("Impactable"))
                    {
                        _hasPlayedSceneObjImpactEffect = true;
                        PlayImpactableEffectOnPoint();
                    }
                }
                
                void PlayImpactableEffectOnPoint()
                {
                    Vector3 _closestPoint = other.ClosestPoint(transform.position);     //Debug.Log("_closestPoint = " + _closestPoint);
                    Vector3 _spinePosition = _runtimeWeapon._states.spineTransform.position;

                    if (Physics.Raycast(_spinePosition, _closestPoint - _spinePosition, out RaycastHit hit, 2))
                    {
                        other.GetComponent<ImpactableSceneObject>().SpawnImpactEffectWhenHit(hit.point);
                    }
                }
            }
        }
    }
}