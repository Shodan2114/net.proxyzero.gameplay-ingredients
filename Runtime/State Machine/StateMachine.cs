using UnityEngine;
using NaughtyAttributes;
using System.Linq;


namespace ProxyBasics
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State remainInState;
        [StateMachineState] public string DefaultState;
        [ReorderableList, NonNullCheck] public State[] States = new State[0];

        public State CurrentState { get { return currentState; }}
        State currentState;
        State lastState;

        [Button("Create New State")]
        private void AddNewState()
        {
            var newState = new GameObject($"State {States.Length}");
            var state = newState.AddComponent<State>();
            newState.transform.parent = transform;
            newState.transform.localPosition = Vector3.zero;
            newState.transform.localRotation = Quaternion.identity;
            newState.transform.localScale = Vector3.one;
            States = States.Concat(new State[] { state }).ToArray();

            if (currentState == null)
                currentState = state;
        }

        [Button("Reset State Objects")]
        private void UpdateFromState()
        {
            foreach(var state in States)
            {
                state.gameObject.SetActive(state == States.FirstOrDefault(o => o.StateName == DefaultState));
            }
        }

        [Button("Create/Update SetStateAction Components")]
        private void UpdateSetStateActionComponents()
        {
            var components = this.GetComponents<SetStateAction>();
            foreach (var state in States)
            {
                if (!components.Any(o => o.state == state.StateName))
                {
                    var action = gameObject.AddComponent<SetStateAction>();
                    action.state = state.StateName;
                    action.StateMachine = this;
                }
            }

            var todelete = GetComponents<SetStateAction>().Where(a => !States.Any(s => s.StateName == a.state)).ToArray();
            for (int i = 0; i < todelete.Length; i++)
            {
                DestroyImmediate(todelete[i]);
            }

            components = this.GetComponents<SetStateAction>();
            foreach(var action in components)
            {
                action.Name = $"Set State {action.state}";
            }
        }

        [Button("Update Transitions")]
        private void UpdateTransitions()
        {
            foreach (var state in States)
            {
                state.GetTransitions();
            }
        }

        void Awake()
        {
            foreach (var state in States)
            {
                if(state.gameObject.activeSelf)
                    state.gameObject.SetActive(false);
            }

            SetState(DefaultState);
        }

        public void SetState(string stateName)
        {
            Debug.Log($"SetState on: {gameObject.name} with state Name: {stateName}");
            if(stateName == "STAY") return;
            Debug.Log("Set State entered with stateName: "+ stateName + " on GameObject: "+transform.name);
            State newState = States.FirstOrDefault(o => o.StateName == stateName);

            if(newState != null)
            {
                if (currentState != null)
                {
                    // Call Exit Actions
                    Callable.Call(currentState.OnStateExit, gameObject);
                    // Then finally disable old state
                    currentState.gameObject.SetActive(false);
                }

                // Switch Active new state
                newState.gameObject.SetActive(true);

                //Set last State
                lastState = currentState;

                // Then Set new current state
                currentState = newState;

                // Finally, call State enter
                Callable.Call(currentState.OnStateEnter, gameObject);
            }
            else
                Debug.LogWarning($"{gameObject.name} : Trying to set unknown state {stateName}", gameObject);
        }

        public void SetState(State state)
        {
            if(state == remainInState) return;
            Debug.Log($"SetState on: {gameObject.name} with state Name: {state}");

            if(state != null)
            {
                if (currentState != null)
                {
                    // Call Exit Actions
                    Callable.Call(currentState.OnStateExit, gameObject);
                    // Then finally disable old state
                    currentState.gameObject.SetActive(false);
                }

                // Switch Active new state
                state.gameObject.SetActive(true);

                //Set last State
                lastState = currentState;

                // Then Set new current state
                currentState = state;

                // Finally, call State enter
                Callable.Call(currentState.OnStateEnter, gameObject);
            }
            else
                Debug.LogWarning($"{gameObject.name} : Trying to set unknown state {state}", gameObject);
        }

        public virtual void SetLastState()
        {
            if(lastState != null)
            {
                if (currentState != null)
                {
                    // Call Exit Actions
                    Callable.Call(currentState.OnStateExit, gameObject);
                    // Then finally disable old state
                    currentState.gameObject.SetActive(false);
                }

                // Switch Active new state
                lastState.gameObject.SetActive(true);

                // Then Set new current state
                currentState = lastState;

                // Finally, call State enter
                Callable.Call(currentState.OnStateEnter, gameObject);
            }
        }

        void Update()
        {
            if (currentState.UseUpdate && currentState != null 
                && currentState.OnStateUpdate != null 
                && currentState.OnStateUpdate.Length > 0)
            {
                Callable.Call(currentState.OnStateUpdate, this.gameObject);
            }
            if(currentState != null)
            {
                currentState.CheckTransitions(this);
            }  
        }

        void FixedUpdate() {
            if (currentState.UseFixedUpdate && currentState != null 
                && currentState.OnStateFixedUpdate != null 
                && currentState.OnStateFixedUpdate.Length > 0)
            {
                Callable.Call(currentState.OnStateFixedUpdate, this.gameObject);
            }
        }

        void LateUpdate() {
            if(currentState.UseLateUpdate && currentState != null
                && currentState.OnStateLateUpdate != null && currentState.OnStateLateUpdate.Length > 0)
                {
                    Callable.Call(currentState.OnStateLateUpdate, this.gameObject);
                }
            
        }
    }
}

