using UnityEngine;
namespace ProxyBasics
{
    public class SetStateAction : ActionBase
    {
        [NonNullCheck]
        public StateMachine StateMachine;

        public string state
        {
            get { return m_State; }
            set { m_State = value; }
        }

        [SerializeField, StateMachineState("StateMachine")]
        protected string m_State = "State";

        public override void Execute(GameObject instigator = null)
        {
            if(StateMachine != null)
                StateMachine.SetState(m_State);
        }
    }
}

