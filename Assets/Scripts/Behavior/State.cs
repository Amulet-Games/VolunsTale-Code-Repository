using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
    	public StateActions[] onFixed;
        public StateActions[] onUpdate;
        public StateActions[] onEnter;
        public StateActions[] onExit;

        public int idCount;
        public List<Transition> transitions = new List<Transition>();

        #region Controller States
		public void FixedTick(StateManager states)
		{
			ExecuteActions(states,onFixed);
		}

        public void Tick(StateManager states)
        {
            ExecuteActions(states, onUpdate);
            CheckTransitions(states);
        }
        
        public void CheckTransitions(StateManager states)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.CheckCondition(states))
                {
                    states.currentState = transitions[i].targetState;
                    return;
                }
            }
        }

        public void ExecuteActions(StateManager states, StateActions[] l)
        {
            int _stateActionsLength = l.Length;
            for (int i = 0; i < _stateActionsLength; i++)
            {
                l[i].Execute(states);
            }
        }
        #endregion

        #region AI States
        public void OnAIEnter(AIStateManager states)
        {
            int _stateActionsLength = onEnter.Length;
            for (int i = 0; i < _stateActionsLength; i++)
            {
                onEnter[i].AIExecute(states);
            }
        }

        public void AIFixedTick(AIStateManager states)
        {
            int _stateActionsLength = onFixed.Length;
            for (int i = 0; i < _stateActionsLength; i++)
            {
                onFixed[i].AIExecute(states);
            }
        }

        public void AITick(AIStateManager states)
        {
            int _stateActionsLength = onUpdate.Length;
            for (int i = 0; i < _stateActionsLength; i++)
            {
                onUpdate[i].AIExecute(states);
            }
            
            CheckAITransitions(states);
        }
        
        public void CheckAITransitions(AIStateManager states)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.CheckAICondition(states))
                {
                    states.currentState = transitions[i].targetState;
                    return;
                }
            }
        }
        #endregion

        public Transition AddTransition()
        {
            Transition retVal = new Transition();
            transitions.Add(retVal);
            retVal.id = idCount;
            idCount++;
            return retVal;
        }

        public Transition GetTransition(int id)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].id == id)
                    return transitions[i];
            }

            return null;
        }

		public void RemoveTransition(int id)
		{
			Transition t = GetTransition(id);
			if (t != null)
				transitions.Remove(t);
		}

    }
}
