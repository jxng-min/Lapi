using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DialogueService
{
    #region Serialization
    [System.Serializable]
    public class ContextData
    {
        public NPCCode NPC;
        public string Context;
    }

    [System.Serializable]
    public class DialogueData
    {
        public int ID;
        public ContextData[] Contexts;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public DialogueData[] Data;
    }
    #endregion Serialization

    public class LocalDialogueService : IDialogueService
    {
        private Dictionary<int, DialogueData> m_dialogue_data_dict;

        public LocalDialogueService()
        {
            m_dialogue_data_dict = new();

            Load();
        }

        public DialogueData GetDialogue(int dialogue_id)
        {
            return m_dialogue_data_dict.TryGetValue(dialogue_id, out var data) ? data : null;
        }

        public void Load()
        {
            var local_data_path = Path.Combine(Application.streamingAssetsPath, "DialogueData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var dialogue_data in wrapped_data.Data)
                {
                    m_dialogue_data_dict.TryAdd(dialogue_data.ID, dialogue_data);
                }

#if UNITY_EDITOR
                Debug.Log("<color=cyan>성공적으로 Dialogue 데이터를 로드하였습니다.</color>");
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