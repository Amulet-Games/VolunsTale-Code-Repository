using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class Nav_Button : MonoBehaviour, INavigatable
    {
        public Image bckImage;

        void Awake()
        {
            bckImage = GetComponent<Image>();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void OnDeselect()
        {
            bckImage.color = Color.white;
        }

        public void OnSelect()
        {
            bckImage.color = Color.red;
        }
    }
}