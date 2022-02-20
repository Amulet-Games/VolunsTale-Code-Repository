using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepImpactFxHandler : MonoBehaviour
{
    [Header("Drag and Drop Ref.")]
    public ParticleSystem _footStepDustFx;
    public ParticleSystem _footStepSmokeFx;

    public void PlayStepImpact()
    {
        gameObject.SetActive(true);

        _footStepDustFx.Play();
        _footStepSmokeFx.Play();
    }

    #region Callback.
    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
