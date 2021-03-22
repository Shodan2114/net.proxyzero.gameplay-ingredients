using UnityEngine;

namespace ProxyBasics
{
    public abstract class Callable : MonoBehaviour, ICallable
    {
        public string Name;

        public void Reset()
        {
            if(Name == string.Empty || Name == null)
                Name = GetDefaultName();
        }

        public abstract void Execute(GameObject instigator = null);
        public abstract new string ToString();

        public static void Call(Callable[] calls, GameObject instigator = null)
        {
            if (calls == null)
            {
                Debug.LogError("Cannot execute callable list: Null or Missing");
                return;
            }

            foreach (var call in calls)
            {

                if(call != null)
                    call.Execute(instigator);
                else
                    Debug.LogError($"Cannot execute Call: Null or Missing");
            }
        }

        public static void Call(Callable callable, GameObject instigator = null)
        {
            if(callable != null)
                callable.Execute(instigator);
            else
                Debug.LogError("Cannot execute call: Null or Missing");
        }

        [ContextMenu("Reset Callable Name")]
        private void MenuSetDefaultName()
        {
            Name = GetDefaultName();
        }
        
        public virtual string GetDefaultName()
        {
            return GetType().Name;
        }
    }
}

