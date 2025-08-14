using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EXPService
{
    #region Serialization
    [System.Serializable]
    public struct EXPData
    {
        public int Level;
        public int EXP;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public EXPData[] List;
    }
    #endregion Serialization

    public class LocalEXPService : IEXPService
    {
        private Dictionary<int, int> m_exp_dict = new();

        public LocalEXPService()
        {
            Load();
        }

        public void Load()
        {
            var local_data_path = Path.Combine(Application.streamingAssetsPath, "EXPData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var exp_data in wrapped_data.List)
                {
                    m_exp_dict.TryAdd(exp_data.Level, exp_data.EXP);
                }

#if UNITY_EDITOR
                Debug.Log("<color=cyan>성공적으로 EXP 데이터를 로드하였습니다.</color>");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"{local_data_path}가 존재하지 않습니다.");
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        public int GetEXP(int current_level)
        {
            return m_exp_dict.TryGetValue(current_level + 1, out var exp) ? exp : 0;
        }
    }
}