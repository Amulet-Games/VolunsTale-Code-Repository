using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeConsumableEffect : MonoBehaviour
    {
        [Header("Refs.")]
        [ReadOnlyInspector] SavableInventory _inventory;
        [ReadOnlyInspector] ParticleSystem _particleSystem;

        [HideInInspector] Vector3 vector3Zero;

        public void Init(SavableInventory _inventory)
        {
            this._inventory = _inventory;

            GetReferences();
            InitNonSerilalized();
            ParentUnderBackpack();
        }

        #region OutSide Call.
        public void PlayConsumableEffect()
        {
            ParentUnderPlayer();
            _particleSystem.Play();
        }

        public void PrepareConsumableEffect()
        {
            _inventory._currentConsumableEffect = this;
        }
        #endregion

        #region Callback.
        public void OnParticleSystemStopped()
        {
            ParentUnderBackpack();
            //OffEffectClearRefs();
        }

        //void OffEffectClearRefs()
        //{
        //    _inventory._states._currentConsumableEffect = null;
        //}
        #endregion
        
        #region Parent Transform.
        void ParentUnderBackpack()
        {
            transform.parent = _inventory.INV_ItemsEffectsBackpackTransform;
            transform.localPosition = vector3Zero;
            transform.localEulerAngles = vector3Zero;
            gameObject.SetActive(false);
        }

        void ParentUnderPlayer()
        {
            transform.parent = _inventory._states.mTransform;
            transform.localPosition = vector3Zero;
            transform.localEulerAngles = vector3Zero;
            gameObject.SetActive(true);
        }
        #endregion

        #region Init.
        void InitNonSerilalized()
        {
            vector3Zero = Vector3.zero;
        }

        void GetReferences()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        #endregion
    }
}