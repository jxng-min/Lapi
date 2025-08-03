using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KeyService
{
    #region Serialization
    [System.Serializable]
    public class KeyData
    {
        public string Name;
        public KeyCode Code;

        public KeyData(string name, KeyCode code)
        {
            Name = name;
            Code = code;
        }
    }

    public class DataWrapper
    {
        public KeyData[] Data;

        public DataWrapper(KeyData[] data)
        {
            Data = data;
        }
    }
    #endregion Serialization

    public class LocalKeyService : ISaveable, IKeyService
    {
        private Dictionary<string, KeyCode> m_key_dict;

        public LocalKeyService()
        {
            m_key_dict = new();

            CreateDirectory();
            Reset();
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Key");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);
            }
        }

        public void Reset()
        {
            m_key_dict.Clear();

            m_key_dict.Add("Inventory", KeyCode.I);
            m_key_dict.Add("Equipment", KeyCode.U);
            m_key_dict.Add("Skill", KeyCode.K);
            m_key_dict.Add("Quest", KeyCode.T);
            m_key_dict.Add("Binder", KeyCode.P);

            m_key_dict.Add("Shortcut0", KeyCode.Alpha1);
            m_key_dict.Add("Shortcut1", KeyCode.Alpha2);
            m_key_dict.Add("Shortcut2", KeyCode.Alpha3);
            m_key_dict.Add("Shortcut3", KeyCode.Alpha4);
            m_key_dict.Add("Shortcut4", KeyCode.Alpha5);

            m_key_dict.Add("Shortcut5", KeyCode.Z);
            m_key_dict.Add("Shortcut6", KeyCode.X);
            m_key_dict.Add("Shortcut7", KeyCode.C);
            m_key_dict.Add("Shortcut8", KeyCode.V);
            m_key_dict.Add("Shortcut9", KeyCode.B);
        }

        public bool Check(KeyCode key, KeyCode current_key)
        {
            if (current_key == key)
            {
                return true;
            }

            if (KeyCode.A <= key && key <= KeyCode.Z ||
                KeyCode.Alpha0 <= key && key <= KeyCode.Alpha9) { }
            else
            {
                return false;
            }

            if (key == KeyCode.W ||
                key == KeyCode.A ||
                key == KeyCode.S ||
                key == KeyCode.D)
            {
                return false;
            }

            foreach (var pair in m_key_dict)
            {
                if (key == pair.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public void Register(KeyCode key, string key_name)
        {
            m_key_dict[key_name] = key;
        }

        public KeyCode GetKeyCode(string key_name)
        {
            return m_key_dict.TryGetValue(key_name, out var code) ? code : KeyCode.None;
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Key", $"KeyData{offset}.json");

            if (File.Exists(local_data_path))
            {
                m_key_dict.Clear();

                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var key_data in wrapped_data.Data)
                {
                    m_key_dict.TryAdd(key_data.Name, key_data.Code);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Key", $"KeyData{offset}.json");

            var temp_list = new List<KeyData>();
            foreach (var pair in m_key_dict)
            {
                temp_list.Add(new(pair.Key, pair.Value));
            }

            var wrapped_data = new DataWrapper(temp_list.ToArray());
            var json_data = JsonUtility.ToJson(wrapped_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}