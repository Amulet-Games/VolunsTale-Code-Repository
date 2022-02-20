using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class DamagePreviewer : MonoBehaviour
    {
        [Header("Pop up Config.")]
        public float previewerShowingMoveSpeed;
        public float previewerHidingMoveSpeed;
        public float previewerStartPosY;
        public float previewerMiddleStopPosY;
        public float previewerLowestPosY;

        [Header("Fade Config.")]
        public float previewerFadeSpeed;

        [Header("Show Rate Config.")]
        public float _showRate;
        [ReadOnlyInspector] public float _showTimer;
        [ReadOnlyInspector] public bool _isCountingDownShowTimer;
        [ReadOnlyInspector] public bool _isShowingPreviewerInProcess;
        [ReadOnlyInspector] public bool _isHidingPreviewerInProcess;

        [Header("Current Damage Icon Type.")]
        [ReadOnlyInspector] public DamageTakenPhysicalTypeEnum _currentDamagePreviewIconType;

        [Header("Drag and Drop Refs.")]
        public RectTransform previewRect;
        public TMP_Text damageText;

        [Space(10)]
        [ReadOnlyInspector] public GameObject _cur_ActivePreviewIconObj;
        public GameObject _strike_PreviewIconObj;
        public GameObject _slash_PreviewIconObj;
        public GameObject _thrust_PreviewIconObj;
        public GameObject _execute_PreviewIconObj;
        public GameObject _aoe_PreviewIconObj;
        public GameObject _fall_PreviewIconObj;

        [Space(10)]
        public CanvasGroup previewerGroup;
        public Canvas previewerCanvas;

        [Header("Manager Refs.")]
        [ReadOnlyInspector] public StateManager states;

        #region Tween Id.
        int previewerMoveTweenId;
        int previewerShowTweenId;
        Vector3 previewOriginalAnchoredPos;
        StringBuilder _damageTextBuilder;
        #endregion

        #region Late Tick.
        public void LateTick()
        {
            MonitorPreviewerTimer();
        }

        #region Timer Methods.
        void MonitorPreviewerTimer()
        {
            if (_isCountingDownShowTimer)
            {
                _showTimer += states._delta;
                if (_showTimer > _showRate)
                {
                    _isCountingDownShowTimer = false;
                    _showTimer = 0;

                    HidePreviewer();
                }
            }
        }

        void HidePreviewer()
        {
            FadeOutPreviewer();
            MoveOutPreviewer();

            _isHidingPreviewerInProcess = true;
        }

        void ResetPreviewerTimer()
        {
            _showTimer = 0;
        }
        #endregion

        #endregion

        #region Register Previewer.
        public void RegisterDamagePreviewer()
        {
            EditDamageText();
            EditDamagePreviewIcon();

            MonitorPreviewShownStatus();

            void EditDamageText()
            {
                _damageTextBuilder.Clear();
                _damageTextBuilder.Append("-").Append((int)states._previousGetHitDamage);
                damageText.text = _damageTextBuilder.ToString();
            }

            void EditDamagePreviewIcon()
            {
                if (_currentDamagePreviewIconType != states._damageTakenPhysicalType)
                {
                    ChangeDamagePreviewIcon();
                    _currentDamagePreviewIconType = states._damageTakenPhysicalType;
                }

                void ChangeDamagePreviewIcon()
                {
                    _cur_ActivePreviewIconObj.SetActive(false);

                    switch (states._damageTakenPhysicalType)
                    {
                        case DamageTakenPhysicalTypeEnum.Strike:
                            _strike_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _strike_PreviewIconObj;
                            break;
                        case DamageTakenPhysicalTypeEnum.Slash:
                            _slash_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _slash_PreviewIconObj;
                            break;
                        case DamageTakenPhysicalTypeEnum.Thrust:
                            _thrust_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _thrust_PreviewIconObj;
                            break;
                        case DamageTakenPhysicalTypeEnum.AOE:
                            _aoe_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _aoe_PreviewIconObj;
                            break;
                        case DamageTakenPhysicalTypeEnum.Execution:
                            _execute_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _execute_PreviewIconObj;
                            break;
                        case DamageTakenPhysicalTypeEnum.Falling:
                            _fall_PreviewIconObj.SetActive(true);
                            _cur_ActivePreviewIconObj = _fall_PreviewIconObj;
                            break;
                    }
                }
            }
            
            void MonitorPreviewShownStatus()
            {
                if (_isHidingPreviewerInProcess)
                {
                    ResetHideProcess();
                    ShowPreviewer();
                }
                else
                {
                    if (_isCountingDownShowTimer)
                    {
                        ResetPreviewerTimer();
                    }
                    else
                    {
                        if (!_isShowingPreviewerInProcess)
                        {
                            ShowPreviewer();
                        }
                    }
                }
            }

            void ResetHideProcess()
            {
                LeanTween.cancel(previewerShowTweenId);
                LeanTween.cancel(previewerMoveTweenId);

                previewerGroup.alpha = 0;
                previewRect.anchoredPosition = previewOriginalAnchoredPos;

                _isHidingPreviewerInProcess = false;
            }

            void ShowPreviewer()
            {
                FadeInPreviewer();
                MoveInPreviewer();
               
                _isShowingPreviewerInProcess = true;
            }
        }
        #endregion

        #region Fade In / Out Previewer.
        void FadeInPreviewer()
        {
            EnablePreviewer();
            LeanTween.alphaCanvas(previewerGroup, 1, previewerFadeSpeed).setEaseLinear().setOnComplete(OnCompleteShowPreviewer);

            void OnCompleteShowPreviewer()
            {
                _isShowingPreviewerInProcess = false;
                _isCountingDownShowTimer = true;
            }
        }

        void FadeOutPreviewer()
        {
            previewerShowTweenId = LeanTween.alphaCanvas(previewerGroup, 0, previewerFadeSpeed).setEaseLinear().setOnComplete(OnCompleteHidePreviewer).id;

            void OnCompleteHidePreviewer()
            {
                DisablePreviewer();
                _isHidingPreviewerInProcess = false;
            }
        }

        void EnablePreviewer()
        {
            previewerCanvas.enabled = true;
        }

        void DisablePreviewer()
        {
            previewerCanvas.enabled = false;
        }
        #endregion

        #region Move In / Out Previewer.
        void MoveInPreviewer()
        {
            MovePreviewerToLowestPos();

            void MovePreviewerToLowestPos()
            {
                LeanTween.moveY(previewRect, previewerLowestPosY, previewerShowingMoveSpeed).setEaseOutCubic().setOnComplete(MovePreviewerToMiddlePos);
            }

            void MovePreviewerToMiddlePos()
            {
                LeanTween.moveY(previewRect, previewerMiddleStopPosY, previewerShowingMoveSpeed).setEaseOutCubic();
            }
        }

        void MoveOutPreviewer()
        {
            MovePreviewerBackToOrigin();

            void MovePreviewerBackToOrigin()
            {
                previewerMoveTweenId = LeanTween.moveY(previewRect, previewerStartPosY, previewerHidingMoveSpeed).setEaseOutCirc().id;
            }
        }
        #endregion

        #region On Quit Game.
        public void OnQuitGame()
        {
            LeanTween.cancel(previewerShowTweenId);
            LeanTween.cancel(previewerMoveTweenId);

            previewerGroup.alpha = 0;
            previewerCanvas.enabled = false;
            previewRect.anchoredPosition = previewOriginalAnchoredPos;

            _isHidingPreviewerInProcess = false;
            _isShowingPreviewerInProcess = false;
            _isCountingDownShowTimer = false;
            _showTimer = 0;

            _cur_ActivePreviewIconObj.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup(StateManager _states)
        {
            states = _states;

            SetupGetPreviewAnchoredPos();
            SetupCurrentPreviewIconObj();
            SetupDamagePreviewIconType();
            SetupDamageTextBuilder();

            void SetupGetPreviewAnchoredPos()
            {
                previewOriginalAnchoredPos = previewRect.anchoredPosition;
            }

            void SetupCurrentPreviewIconObj()
            {
                _cur_ActivePreviewIconObj = _strike_PreviewIconObj;
            }

            void SetupDamagePreviewIconType()
            {
                _currentDamagePreviewIconType = DamageTakenPhysicalTypeEnum.Init;
            }

            void SetupDamageTextBuilder()
            {
                _damageTextBuilder = _states._inp.damagePreviewer_strBuilder;
            }
        }
        #endregion
    }
}