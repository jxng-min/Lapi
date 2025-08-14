using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NPCService
{
    #region Serialization
    [System.Serializable]
    public struct NPCData
    {
        public NPCCode Code;
        public string Name;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public NPCData[] Data;
    }
    #endregion Serialization

    public class LocalNPCService : INPCService
    {
        private Dictionary<NPCCode, string> m_name_dict;

        public LocalNPCService()
        {
            m_name_dict = new();
            Load();
        }

        public string GetName(NPCCode code)
        {
            return m_name_dict.TryGetValue(code, out var name) ? name : string.Empty;
        }

        public void Load()
        {
            var local_data_path = Path.Combine(Application.streamingAssetsPath, "NPCData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var npc_data in wrapped_data.Data)
                {
                    m_name_dict.TryAdd(npc_data.Code, npc_data.Name);
                }

#if UNITY_EDITOR
                Debug.Log("<color=cyan>성공적으로 NPC 데이터를 로드하였습니다.</color>");
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
    }
}