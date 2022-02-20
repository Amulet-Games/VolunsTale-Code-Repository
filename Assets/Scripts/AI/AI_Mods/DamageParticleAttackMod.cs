using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class DamageParticleAttackMod : AIMod
    {
        [HideInInspector] public bool showDpAttackMod;

        /// Anim Hash.
        [NonSerialized] int e_dp_area_attack_1_hash;
        [NonSerialized] int e_dp_area_attack_2_hash;
        [NonSerialized] int e_dp_area_attack_3_hash;
        [NonSerialized] int e_dp_proj_attack_1_hash;
        [NonSerialized] int e_dp_proj_attack_2_hash;
        [NonSerialized] int e_dp_proj_attack_3_hash;

        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before execute another damage particle attack.")]
        public float stdDpAttackWaitRate;

        [SerializeField]
        [Tooltip("If this is true, \"stdDpAttackWaitRate\" will change each time after perilous attack is executed.")]
        private bool isRandomized = false;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdDpAttackWaitRate\".")]
        private float dPAttackRandomizeAmount = 0;

        [ReadOnlyInspector]
        public bool usedDpAttack;

        [ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait to peform damage particle attack again.")]
        public float finalizedDpAttackWaitTime;

        /// Area Damage Particles.
        public int _dp_area_attack_1_singleton_Id;
        public int _dp_area_attack_2_singleton_Id;
        public int _dp_area_attack_3_singleton_Id;
        public int _dp_proj_attack_1_singleton_Id;
        public int _dp_proj_attack_2_singleton_Id;
        public int _dp_proj_attack_3_singleton_Id;

        [NonSerialized]
        public float _delta;
        [NonSerialized]
        public AIManager _ai;

        /// INIT.
        
        public void DamagedParticleAttackModInit(AIManager _ai)
        {
            this._ai = _ai;

            InitAnimHash();
        }

        void InitAnimHash()
        {
            HashManager _hashManager = _ai.hashManager;
            e_dp_area_attack_1_hash = _hashManager.e_dp_area_attack_1_hash;
            e_dp_area_attack_2_hash = _hashManager.e_dp_area_attack_2_hash;
            e_dp_area_attack_3_hash = _hashManager.e_dp_area_attack_3_hash;
            e_dp_proj_attack_1_hash = _hashManager.e_dp_proj_attack_1_hash;
            e_dp_proj_attack_2_hash = _hashManager.e_dp_proj_attack_2_hash;
            e_dp_proj_attack_3_hash = _hashManager.e_dp_proj_attack_3_hash;
        }
        
        public void DpAttackGoesAggroReset()
        {
            RandomizeDpAttackRate();
        }

        public void DpAttackExitAggroReset()
        {
            usedDpAttack = false;
        }

        /// TICK.
        
        public void DamageParticleAttackTimeCount()
        {
            if (usedDpAttack)
            {
                finalizedDpAttackWaitTime -= _delta;
                if (finalizedDpAttackWaitTime <= 0)
                {
                    finalizedDpAttackWaitTime = 0;
                    usedDpAttack = false;
                }
            }
        }

        public void HandleDpAttack(DpAttackAnimStateEnum _targetAnimHash)
        {
            switch (_targetAnimHash)
            {
                case DpAttackAnimStateEnum.e_dp_area_attack_1:
                    _ai.PlayDpAttackAnim(e_dp_area_attack_1_hash, _ai.aISessionManager.GetSinglesAreaDP_ById(_dp_area_attack_1_singleton_Id));
                    break;

                case DpAttackAnimStateEnum.e_dp_area_attack_2:
                    _ai.PlayDpAttackAnim(e_dp_area_attack_2_hash, _ai.aISessionManager.GetSinglesAreaDP_ById(_dp_area_attack_2_singleton_Id));
                    break;

                case DpAttackAnimStateEnum.e_dp_area_attack_3:
                    _ai.PlayDpAttackAnim(e_dp_area_attack_3_hash, _ai.aISessionManager.GetSinglesAreaDP_ById(_dp_area_attack_3_singleton_Id));
                    break;

                case DpAttackAnimStateEnum.e_dp_proj_attack_1:
                    _ai.PlayDpAttackAnim(e_dp_proj_attack_1_hash, _ai.aISessionManager.GetSingletonProjDP_ById(_dp_proj_attack_1_singleton_Id));
                    break;

                case DpAttackAnimStateEnum.e_dp_proj_attack_2:
                    _ai.PlayDpAttackAnim(e_dp_proj_attack_2_hash, _ai.aISessionManager.GetSingletonProjDP_ById(_dp_proj_attack_2_singleton_Id));
                    break;

                case DpAttackAnimStateEnum.e_dp_proj_attack_3:
                    _ai.PlayDpAttackAnim(e_dp_proj_attack_3_hash, _ai.aISessionManager.GetSingletonProjDP_ById(_dp_proj_attack_3_singleton_Id));
                    break;
            }

            SetUsedDpAttackToTrue();
        }
        
        void SetUsedDpAttackToTrue()
        {
            usedDpAttack = true;
            RandomizeDpAttackRate();
        }

        void RandomizeDpAttackRate()
        {
            if (isRandomized)
            {
                RandomizeWithAddonValue(dPAttackRandomizeAmount, stdDpAttackWaitRate, ref finalizedDpAttackWaitTime);
            }
            else
            {
                finalizedDpAttackWaitTime = stdDpAttackWaitRate;
            }
        }
        
        public enum DpAttackAnimStateEnum
        {
            e_dp_area_attack_1,
            e_dp_area_attack_2,
            e_dp_area_attack_3,
            e_dp_proj_attack_1,
            e_dp_proj_attack_2,
            e_dp_proj_attack_3
        }
    }
}