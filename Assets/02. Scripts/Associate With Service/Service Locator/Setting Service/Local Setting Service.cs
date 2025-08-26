using System.IO;
using UnityEngine;

namespace SettingService
{
    #region Serialization
    [System.Serializable]
    public class SettingData
    {
        public bool m_bgm_active;
        public float m_bgm_rate;
        public bool m_sfx_active;
        public float m_sfx_rate;

        public SettingData()
        {
            m_bgm_active = true;
            m_bgm_rate = 0.5f;

            m_sfx_active = true;
            m_sfx_rate = 0.5f;
        } 
    }
    #endregion Serialization

    public class LocalSettingService : ISettingService, ISaveable
    {
        private SettingData m_setting_data;
        public SettingData Data
        {
            get => m_setting_data;
            set => m_setting_data = value;
        }

        public LocalSettingService()
        {
            m_setting_data = new SettingData();

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var directory_path = Path.Combine(Application.persistentDataPath, "Setting");

            if (!Directory.Exists(directory_path))
            {
                Directory.CreateDirectory(directory_path);
            }
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Setting", $"SettingData{offset}.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                m_setting_data = JsonUtility.FromJson<SettingData>(json_data);
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Setting", $"SettingData{offset}.json");

            var json_data = JsonUtility.ToJson(m_setting_data, true);
            File.WriteAllText(local_data_path, json_data);
        }
    }
}