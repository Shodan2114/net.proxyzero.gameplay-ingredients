using NaughtyAttributes;
using UnityEngine;
namespace ProxyBasics
{
    public class OnMessageEvent : MonoBehaviour
    {
        public string MessageName = "Message";
        [ReorderableList]public Callable[] OnMessageRecieved;

        void OnEnable() {
            Messager.RegisterMessage(MessageName, Execute);
        }

        void OnDisable() {
            Messager.RemoveMessage(MessageName, Execute);
        }

        void Execute(GameObject instigator)
        {
            try
            {
                Callable.Call(OnMessageRecieved, instigator);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(string.Format("OnMessageEvent : Exception Caught while catching message '{0}' on Object '{1}'", MessageName, gameObject.name));
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}

