using System;
using System.Collections.Generic;
using UnityEngine;
namespace ProxyBasics
{
    public static class Messager
    {
        public delegate void Message(GameObject instigator = null);
        private static Dictionary<string, List<Message>> registeredMessages;

        static Messager()
        {
            registeredMessages = new Dictionary<string, List<Message>>();
        }

        public static void RegisterMessage(string messageName, Message message)
        {
            if(!registeredMessages.ContainsKey(messageName))
            {
                registeredMessages.Add(messageName, new List<Message>());
            }
            if(!registeredMessages[messageName].Contains(message))
            {
                registeredMessages[messageName].Add(message);
            }
            else
            {
                Debug.LogWarning(string.Format("Messager : {0} entry already contains reference to message.", messageName));
            }
        }

        public static void RemoveMessage(string messageName, Message message)
        {
            var currentEvent = registeredMessages[messageName];
            if(currentEvent.Contains(message))
            {
                currentEvent.Remove(message);
            }

            if(currentEvent == null || currentEvent.Count == 0)
                registeredMessages.Remove(messageName);
        }

        public static void Send(string eventName, GameObject instigator = null)
        {
            if(registeredMessages.ContainsKey(eventName))
            {
                try
                {
                    var messages = registeredMessages[eventName].ToArray();
                    foreach (var message in messages)
                    {
                        if(message != null)
                            message.Invoke(instigator);
                    }
                }
                catch (Exception e)
                {
                    
                    Debug.Log(e);
                }
            }
        }
    }
}

