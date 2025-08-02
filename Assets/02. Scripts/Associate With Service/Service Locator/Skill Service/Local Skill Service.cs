using System;
using System.IO;
using InventoryService;
using UnityEngine;

namespace SkillService
{
    #region Serialization
    [System.Serializable]
    public class SkillData
    {
        public int Point;
        public ItemData[] Skills;

        public SkillData()
        {
            Point = 0;
            Skills = new ItemData[3]
            {
                new(ItemCode.DASH, 1),
                new(ItemCode.FIRE_BALL, 1),
                new(ItemCode.THUNDER, 1),
            };
        }

        public SkillData(int point, ItemData[] skills)
        {
            Point = point;
            Skills = skills;
        }
    }
    #endregion Serialization

    public class LocalSkillService : ISaveable, ISkillService
    {
        private int m_skill_point;
        private ItemData[] m_skills;

        public event Action<int> OnUpdatedPoint;
        public event Action<int, ItemData> OnUpdatedSlot;

        public int SkillPoint => m_skill_point;

        public LocalSkillService()
        {
            var skill_data = new SkillData();

            m_skill_point = skill_data.Point;
            m_skills = skill_data.Skills;

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Skill");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Skill 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        public void InitializePoint()
        {
            OnUpdatedPoint?.Invoke(m_skill_point);
        }

        public void InitializeSlot(int offset)
        {
            OnUpdatedSlot?.Invoke(offset, m_skills[offset]);
        }

        public void SetSkill(int offset, ItemCode code, int level)
        {
            m_skills[offset].Code = code;
            m_skills[offset].Count = level;

            OnUpdatedSlot?.Invoke(offset, m_skills[offset]);
        }

        public void UpdatePoint(int amount)
        {
            m_skill_point += amount;

            OnUpdatedPoint?.Invoke(m_skill_point);

            for (int i = 0; i < m_skills.Length; i++)
            {
                OnUpdatedSlot?.Invoke(i, m_skills[i]);
            }
        }

        public void UpdateSkill(int offset, int count, int point = -1)
        {
            m_skills[offset].Count += count;
            UpdatePoint(point);

            OnUpdatedSlot?.Invoke(offset, m_skills[offset]);
        }

        public ItemData GetSkill(int offset)
        {
            return m_skills[offset];
        }

        public int GetSkillLevel(ItemCode code)
        {
            foreach (var skill in m_skills)
            {
                if (skill.Code == code)
                {
                    return skill.Count;
                }
            }

            return 0;
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Skill", $"SkillData{offset}.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var skill_data = JsonUtility.FromJson<SkillData>(json_data);

                m_skill_point = skill_data.Point;
                m_skills = skill_data.Skills;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Skill", $"SkillData{offset}.json");

            var skill_data = new SkillData(m_skill_point, m_skills);
            var json_data = JsonUtility.ToJson(skill_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}