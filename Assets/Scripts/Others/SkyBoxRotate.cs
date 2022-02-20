using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SkyBoxRotate : MonoBehaviour
    {
        public float RotateSpeed;

        private void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation",RotateSpeed * Time.time);
        }
    }
}