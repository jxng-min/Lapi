using UnityEngine;

namespace KeyService
{
    public interface IKeyService
    {
        void Reset();
        KeyCode GetKeyCode(string key_name);
        bool Check(KeyCode key, KeyCode current_key);
        void Register(KeyCode key, string key_name);
    }
}