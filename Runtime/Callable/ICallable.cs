using UnityEngine;
namespace ProxyBasics
{
    public interface ICallable
    {
        void Execute(GameObject instigator = null);
    }
}

