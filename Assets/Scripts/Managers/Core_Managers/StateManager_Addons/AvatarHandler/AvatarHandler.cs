using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AvatarHandler : MonoBehaviour
    {
        [Header("Bake Tools (Drops).")]
        public Camera bakeCam;
        public RenderTexture bakeRt;

        [Header("Bake Lights (Drops).")]
        public GameObject camKeyLight;
        public GameObject camFillLight;
        public GameObject camBackLight;

        [Header("Bake Transforms (Drops).")]
        public AvatarBakeTransform _axe_1h_BakeTransform;
        public AvatarBakeTransform _axe_2h_BakeTransform;
        public AvatarBakeTransform _fist_1h_BakeTransform;
        public AvatarBakeTransform _fist_2h_BakeTransform;
        public AvatarBakeTransform _shield_1h_BakeTransform;
        public AvatarBakeTransform _shield_2h_BakeTransform;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public Transform _mTransform;
        [ReadOnlyInspector] public Transform _bakeCamTransform;
        
        [Header("Static")]
        public static AvatarHandler singleton;

        #region Private.
        RenderTexture mRt;
        #endregion
        
        #region Capture Avatar.
        public void RequestAvatarCapture()
        {
            PrepareRefsForCapture();
            CaptureAvatarToSave();
            ClearUpRefsAfterCapture();
        }

        void PrepareRefsForCapture()
        {
            AvatarBakeTransform _targetBakeTrans = null;

            ParentUnderPlayer();

            GetTargetBakeTransform();

            MoveCameraToCapturePoint();

            ActivateHandler();

            void GetTargetBakeTransform()
            {
                if (_states._isTwoHanding)
                {
                    #region Get Pose From Th Weapon.
                    switch (_states._savableInventory._twoHandingWeapon_referedItem.weaponType)
                    {
                        case P_Weapon_WeaponTypeEnum.Axe:
                            _targetBakeTrans = _axe_2h_BakeTransform;
                            break;
                        case P_Weapon_WeaponTypeEnum.Fist:
                            _targetBakeTrans = _fist_2h_BakeTransform;
                            break;
                        case P_Weapon_WeaponTypeEnum.Shield:
                            _targetBakeTrans = _shield_2h_BakeTransform;
                            break;
                    }
                    #endregion
                }
                else
                {
                    #region Get Pose From Rh Weapon.
                    switch (_states._savableInventory._rightHandWeapon_referedItem.weaponType)
                    {
                        case P_Weapon_WeaponTypeEnum.Axe:
                            _targetBakeTrans = _axe_1h_BakeTransform;
                            break;
                        case P_Weapon_WeaponTypeEnum.Fist:
                            _targetBakeTrans = _fist_1h_BakeTransform;
                            break;
                        case P_Weapon_WeaponTypeEnum.Shield:
                            _targetBakeTrans = _shield_1h_BakeTransform;
                            break;
                    }
                    #endregion
                }
            }

            void MoveCameraToCapturePoint()
            {
                _bakeCamTransform.localPosition = _targetBakeTrans.localPos;
                _bakeCamTransform.localEulerAngles = _targetBakeTrans.localEulers;
            }
        }

        void CaptureAvatarToSave()
        {
            Texture2D tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
            mRt.Release();
            RenderTexture.active = mRt;

            bakeCam.Render();
            tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
            tex.Apply();

            /// Save result as .png file.
            byte[] bytesPng = tex.EncodeToPNG();
            Serialization.Avatar_SaveToFile_NewGame(bytesPng);

            Destroy(tex);
        }

        void ClearUpRefsAfterCapture()
        {
            DeactivateHandler();

            UnParentHandler();
        }
        #endregion

        #region Parent / UnParent Handler.
        void ParentUnderPlayer()
        {
            _mTransform.parent = _states.mTransform;
            _mTransform.localPosition = _states.vector3Zero;
            _mTransform.localEulerAngles = _states.vector3Zero;
        }

        void UnParentHandler()
        {
            _mTransform.parent = null;
        }
        #endregion

        #region Activate / Deactivate Handler.
        void ActivateHandler()
        {
            gameObject.SetActive(true);
        }

        void DeactivateHandler()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Awake.
        private void Awake()
        {
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }

            Init();
        }

        void Init()
        {
            InitRefs();
            
            InitCreateCustomRT();

            InitSetBakeCamRT();

            DeactivateHandler();
        }

        void InitRefs()
        {
            _mTransform = transform;
            _bakeCamTransform = bakeCam.transform;

            bakeCam.gameObject.SetActive(true);
            camKeyLight.SetActive(true);
            camFillLight.SetActive(true);
            camBackLight.SetActive(true);
        }
        
        void InitCreateCustomRT()
        {
            mRt = new RenderTexture(bakeRt.width, bakeRt.height, bakeRt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            mRt.antiAliasing = bakeRt.antiAliasing;
        }

        void InitSetBakeCamRT()
        {
            bakeCam.targetTexture = mRt;
        }
        #endregion
    }
}