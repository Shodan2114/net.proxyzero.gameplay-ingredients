
using UnityEngine;
namespace ProxyBasics
{
    public class StateMachineStateAttribute : PropertyAttribute
    {
        public readonly string PropertyName;

        public StateMachineStateAttribute(string propertyName = "")
        {
            PropertyName = propertyName;
        }
    }
}


