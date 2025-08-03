using System;
using UnityEngine;

namespace KeyService
{
    public interface IKeyService
    {
        event Action<KeyCode, string> OnUpdatedKey;

        void Reset();
        KeyCode GetKeyCode(string key_name);
        bool Check(KeyCode key, KeyCode current_key);
        void Register(KeyCode key, string key_name);
    }
}