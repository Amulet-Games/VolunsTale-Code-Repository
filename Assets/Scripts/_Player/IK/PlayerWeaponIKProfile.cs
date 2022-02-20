using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Weapon IK Profile")]
    public class PlayerWeaponIKProfile : ScriptableObject
    {
        [Header("1H IK LookAt Positions.")]
        public bool _1hDefaultHeadOnly;
        public Vector3 _1hDefaultLookAtPos;
        public bool _1hWalkingIKHeadOnly;
        public Vector3 _1hWalkingLookAtPos;
        public bool _1hRunningIKHeadOnly;
        public Vector3 _1hRunningLookAtPos;

        [Header("1H L&RH Avatar IK Goals.")]
        public PlayerIKGoal _1h_Lh_DefaultGoal;
        public PlayerIKGoal _1h_Lh_RunningGoal;
        public PlayerIKGoal _1h_Lh_WalkingGoal;
        public PlayerIKGoal _1h_Rh_DefaultGoal;
        public PlayerIKGoal _1h_Rh_RunningGoal;
        public PlayerIKGoal _1h_Rh_WalkingGoal;

        [Header("2H IK LookAt Positions.")]
        public bool _2hDefaultHeadOnly;
        public Vector3 _2hDefaultLookAtPos;
        public bool _2hWalkingIKHeadOnly;
        public Vector3 _2hWalkingLookAtPos;
        public bool _2hRunningIKHeadOnly;
        public Vector3 _2hRunningLookAtPos;

        [Header("2H L&RH Avatar IK Goals.")]
        public PlayerIKGoal _2h_Lh_DefaultGoal;
        public PlayerIKGoal _2h_Lh_RunningGoal;
        public PlayerIKGoal _2h_Lh_WalkingGoal;
        public PlayerIKGoal _2h_Rh_DefaultGoal;
        public PlayerIKGoal _2h_Rh_RunningGoal;
        public PlayerIKGoal _2h_Rh_WalkingGoal;
        
        [Header("2H Defense IK LookAt Positions.")]
        public bool _2hDefenseDefaultHeadOnly;
        public Vector3 _2hDefenseDefaultLookAtPos;
        public bool _2hDefenseWalkingHeadOnly;
        public Vector3 _2hDefenseWalkingLookAtPos;
        public bool _2hDefenseRunningHeadOnly;
        public Vector3 _2hDefenseRunningLookAtPos;

        [Header("2H Defense L&RH Avatar IK Goals.")]
        public PlayerIKGoal _light2Defense_Lh_DefaultGoal;
        public PlayerIKGoal _light2Defense_Lh_RunningGoal;
        public PlayerIKGoal _light2Defense_Lh_WalkingGoal;
        public PlayerIKGoal _light2Defense_Rh_DefaultGoal;
        public PlayerIKGoal _light2Defense_Rh_RunningGoal;
        public PlayerIKGoal _light2Defense_Rh_WalkingGoal;

        [Header("Oppose 1 Defense Profiles.")]
        public Weapon_Oppose1_Defense_Profile _axe_oppose1_defense_profile;
        public Weapon_Oppose1_Defense_Profile _shield_oppose1_defense_profile;

        [Header("1H Surrround IK Upper Body Max Angle Thershold.")]
        public float _surroundIKUpperBodyMaxAngle_1H = 80;
    }

    [Serializable]
    public class Weapon_Oppose1_Defense_Profile
    {
        [Header("1H Defense IK LookAt Positions.")]
        public bool _1hDefenseDefaultHeadOnly;
        public Vector3 _1hDefenseDefaultLookAtPos;
        public bool _1hDefenseWalkingHeadOnly;
        public Vector3 _1hDefenseWalkingLookAtPos;
        public bool _1hDefenseRunningHeadOnly;
        public Vector3 _1hDefenseRunningLookAtPos;

        [Header("1H Defense LH Avatar IK Goal.")]
        public PlayerIKGoal _oppose1DefenseDefaultGoal;
        public PlayerIKGoal _oppose1DefenseRunningGoal;
        public PlayerIKGoal _oppose1DefenseWalkingGoal;
    }
}