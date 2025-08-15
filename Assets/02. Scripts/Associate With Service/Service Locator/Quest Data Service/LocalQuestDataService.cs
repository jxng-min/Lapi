using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuestDataService
{
    #region Serialization
    [System.Serializable]
    public class QuestInfoData
    {
        public int ID;
        public string Name;
        public string Objective;
        public string Description;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public QuestInfoData[] Data;
    }
    #endregion Serialization

    public class LocalQuestDataService : IQuestDataService
    {
        private Dictionary<int, QuestInfoData> m_quest_dict;

        public LocalQuestDataService()
        {
            m_quest_dict = new();

            Load();
        }

        public void Load()
        {
            var local_data_path = Path.Combine(Application.streamingAssetsPath, "QuestData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var quest_data in wrapped_data.Data)
                {
                    m_quest_dict.TryAdd(quest_data.ID, quest_data);
                }

#if UNITY_EDITOR
                Debug.Log("<color=cyan>성공적으로 Quest 데이터를 로드하였습니다.</color>");
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

        public string GetName(int quest_id)
        {
            return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.Name : "";
        }

        public string GetObjective(int quest_id)
        {
            return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.Objective : "";
        }

        public string GetDescription(int quest_id)
        {
            return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.Description : "";
        }
    }
}