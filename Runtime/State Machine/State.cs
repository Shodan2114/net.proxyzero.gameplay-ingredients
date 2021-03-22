using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


namespace ProxyBasics
{
    public class State : MonoBehaviour
    {
        public string StateName {get { return gameObject.name; } }
        [ReorderableList] public Callable[] OnStateEnter;
        [ReorderableList] public Callable[] OnStateExit;
        [SerializeField] private List<Transition> transitions = new List<Transition>();
        public List<Transition> Transitions => transitions;
        [ReorderableList, ShowIf("useUpdate")] public Callable[] OnStateUpdate;
        [ReorderableList, ShowIf("useFixedUpdate")]public Callable[] OnStateFixedUpdate;
        [ShowIf("useLateUpdate")]public Callable[] OnStateLateUpdate;
        [SerializeField] private bool useUpdate;
        [SerializeField] private bool useFixedUpdate;
        [SerializeField] private bool useLateUpdate;
        public bool UseUpdate => useUpdate;
        public bool UseFixedUpdate => useFixedUpdate;
        public bool UseLateUpdate => useLateUpdate;

        public void CheckTransitions(StateMachine sm)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                bool decisionSucceeded = transitions[i].Decision.Decide();
                if(decisionSucceeded)
                {
                    sm.SetState(transitions[i].trueState);
                }
                else
                {
                    sm.SetState(transitions[i].falseState);
                }
            }
        }

        public void GetTransitions()
        {
            transitions = new List<Transition>();
            Transform t = transform.Find("Transitions");
            foreach (Transform transf in t)
            {
                Transition transition = transf.GetComponent<Transition>();
                transitions.Add(transition);
            }
        }

    }
}

