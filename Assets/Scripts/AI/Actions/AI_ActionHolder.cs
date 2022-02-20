using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI_ActionHolder")]
    public class AI_ActionHolder : ScriptableObject
    {
        public AIAction[] actionsList;

        /// Privates.
        int actionListLength;
        int sameScoreActionsCount;
        int topScore;
        List<AIAction> sameScoreActions = new List<AIAction>();

        public AIAction FindTopScoreAction(AIManager ai)
        {
            if (ai.playerStates.isDead)
                return null;

            if (ai.debugActionHolder)
            {
                #region Debug Ver.
                actionListLength = actionsList.Length;
                sameScoreActionsCount = 0;
                topScore = 0;

                sameScoreActions.Clear();

                #region Debug.
                Debug.LogWarning("distanceToTarget = " + ai.distanceToTarget);
                #endregion

                for (int i = 0; i < actionListLength; i++)
                {
                    int currentTotalScore = actionsList[i].TotalScoreForeachAction(ai);

                    // If current total score is higher than previous total score.
                    if (currentTotalScore > topScore)
                    {
                        topScore = currentTotalScore;

                        // If current total score is higher than previous total score, because sameScoreActions's score is the same as previous total score.
                        // Actions share the same score with preivous total score don't matter anymore.
                        sameScoreActions.Clear();
                        sameScoreActions.Add(actionsList[i]);
                        sameScoreActionsCount = 1;
                    }
                    else if (currentTotalScore == topScore)
                    {
                        sameScoreActions.Add(actionsList[i]);
                        sameScoreActionsCount++;
                    }

                    #region Debug.
                    Debug.Log(ai.aIStates.GetErrorMessage(actionsList[i].name, " score : ", currentTotalScore.ToString()));
                    if (i == actionListLength - 1)
                    {
                        Debug.LogWarning("----------------------End of cycle----------------------");
                    }
                    #endregion
                }

                if (topScore == 0)
                    return null;

                // If same score ai action exists inside sameScoreAction array, which means there are actions share the same score.
                return sameScoreActions[Random.Range(0, sameScoreActionsCount)];
                #endregion
            }
            else
            {
                #region Normal.
                actionListLength = actionsList.Length;
                sameScoreActionsCount = 0;
                topScore = 0;

                sameScoreActions.Clear();
                
                for (int i = 0; i < actionListLength; i++)
                {
                    int currentTotalScore = actionsList[i].TotalScoreForeachAction(ai);

                    // If current total score is higher than previous total score.
                    if (currentTotalScore > topScore)
                    {
                        topScore = currentTotalScore;

                        // If current total score is higher than previous total score, because sameScoreActions's score is the same as previous total score.
                        // Actions share the same score with preivous total score don't matter anymore.
                        sameScoreActions.Clear();
                        sameScoreActions.Add(actionsList[i]);
                        sameScoreActionsCount = 1;
                    }
                    else if (currentTotalScore == topScore)
                    {
                        sameScoreActions.Add(actionsList[i]);
                        sameScoreActionsCount++;
                    }
                }

                if (topScore == 0)
                    return null;

                // Get a random action.
                return sameScoreActions[Random.Range(0, sameScoreActionsCount)];
                #endregion
            }
        }
    }
}