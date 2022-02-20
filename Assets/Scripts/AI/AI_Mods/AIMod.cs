using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AIMod
    {
        public void RandomizeWithAddonValue(float addOnAmount, float standardAmount, ref float finalizedAmount)
        {
            bool isAdding = false;
            bool netual = false;

            int randomResult = Random.Range(1, 4);
            if (randomResult == 1)
            {
                isAdding = true;
            }
            else if (randomResult == 2)
            {
                netual = true;
            }

            float randomizedAmount = Random.Range(0, addOnAmount + 0.1f);
            if (isAdding)
            {
                finalizedAmount = standardAmount + randomizedAmount;
            }
            else
            {
                if (netual)
                {
                    finalizedAmount = standardAmount;
                }
                else
                {
                    finalizedAmount = standardAmount - randomizedAmount;
                }
            }
        }
    }
}