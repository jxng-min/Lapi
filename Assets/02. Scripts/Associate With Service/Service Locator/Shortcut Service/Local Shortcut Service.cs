using System;
using System.IO;
using InventoryService;
using UnityEngine;

namespace ShortcutService
{
    #region Serialization
    [System.Serializable]
    public class ShortcutData
    {
        public ItemCode[] Shortcuts;

        public ShortcutData()
        {
            Shortcuts = new ItemCode[10];
        }

        public ShortcutData(ItemCode[] codes)
        {
            Shortcuts = codes;
        }
    }
    #endregion Serialization

    public class LocalShortcutService : ISaveable, IShortcutService
    {
        private IInventoryService m_inventory_service;
        private ItemCode[] m_shortcuts;

        public event Action<int, ItemData> OnUpdatedSlot;

        public LocalShortcutService()
        {
            m_inventory_service = ServiceLocator.Get<IInventoryService>();
            m_inventory_service.OnUpdatedSlot += UpdateSlot;

            var shortcut_data = new ShortcutData();
            m_shortcuts = shortcut_data.Shortcuts;

            CreateDictionary();
        }

        private void CreateDictionary()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Shortcut");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);

#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Shortcut 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        public ItemData GetItem(int offset)
        {
            if (m_shortcuts[offset] == ItemCode.NONE)
            {
                return new ItemData();
            }
            else
            {
                return new ItemData(m_shortcuts[offset], m_inventory_service.GetItemCount(m_shortcuts[offset]));
            }
        }

        public void UpdateSlot(int offset, ItemData item_data)
        {
            for (offset = 0; offset < 10; offset++)
            {
                if (m_shortcuts[offset] == item_data.Code)
                {
                    OnUpdatedSlot?.Invoke(offset, new ItemData(m_shortcuts[offset], m_inventory_service.GetItemCount(m_shortcuts[offset])));
                }
            }
        }

        public void SetItem(int offset, ItemCode code)
        {
            m_shortcuts[offset] = code;

            OnUpdatedSlot?.Invoke(offset, new ItemData(m_shortcuts[offset], m_inventory_service.GetItemCount(m_shortcuts[offset])));
        }

        public void Clear(int offset)
        {
            m_shortcuts[offset] = ItemCode.NONE;

            OnUpdatedSlot?.Invoke(offset, new ItemData());
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Shortcut", $"ShortcutData{offset}.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var shortcut_data = JsonUtility.FromJson<ShortcutData>(json_data);

                m_shortcuts = shortcut_data.Shortcuts;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Shortcut", $"ShortcutData{offset}.json");

            var shortcut_data = new ShortcutData(m_shortcuts);
            var json_data = JsonUtility.ToJson(shortcut_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}