using System;
using System.IO;
using UnityEngine;

namespace InventoryService
{
    #region Serialization
    [System.Serializable]
    public class ItemData
    {
        public ItemCode Code;
        public int Count;

        public ItemData()
        {
            Code = ItemCode.NONE;
            Count = 0;
        }

        public ItemData(ItemCode code, int count)
        {
            Code = code;
            Count = count;
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public int Gold;
        public ItemData[] Items;

        public InventoryData()
        {
            Gold = 0;
            Items = new ItemData[30];
        }

        public InventoryData(int gold, ItemData[] items)
        {
            Gold = gold;
            Items = items;
        }
    }
    #endregion Serialization

    public class LocalInventoryService : ISaveable, IInventoryService
    {
        private IItemDataBase m_item_db;

        private int m_money;
        private ItemData[] m_items;

        public event Action<int> OnUpdatedGold;
        public event Action<int, ItemData> OnUpdatedSlot;

        public LocalInventoryService()
        {
            m_money = 0;
            m_items = new ItemData[30];
            for (int i = 0; i < m_items.Length; i++)
            {
                if (i == 0)
                {
                    m_items[i] = new ItemData(ItemCode.NINA_ARMOR, 1);
                }
                else if (i == 3)
                {
                    m_items[i] = new ItemData(ItemCode.NINA_SWORD, 1);
                }
                else
                {
                    m_items[i] = new ItemData();
                }
            }

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var directory_path = Path.Combine(Application.persistentDataPath, "Inventory");

            if (!Directory.Exists(directory_path))
            {
                Directory.CreateDirectory(directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Inventory 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        public void Inject(IItemDataBase item_db)
        {
            m_item_db = item_db;
        }

        public void InitializeSlot(int offset)
        {
            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        public void InitializeGold()
        {
            UpdateGold(0);
        }

        public void UpdateGold(int amount)
        {
            m_money += amount;
            m_money = Mathf.Clamp(m_money, 0, int.MaxValue);

            OnUpdatedGold?.Invoke(m_money);
        }

        public void AddItem(ItemCode code, int count)
        {
            var item = m_item_db.GetItem(code);

            if (item.Stackable)
            {
                for (int i = 0; i < m_items.Length; i++)
                {
                    if (m_items[i].Code == code && m_items[i].Count + count <= 99)
                    {
                        m_items[i].Count += count;

                        OnUpdatedSlot?.Invoke(i, m_items[i]);
                        return;
                    }
                }
            }

            for (int i = 0; i < m_items.Length; i++)
            {
                if (m_items[i].Code == ItemCode.NONE)
                {
                    m_items[i].Code = code;
                    m_items[i].Count = count;

                    OnUpdatedSlot?.Invoke(i, m_items[i]);
                    return;
                }
            }
        }

        public void SetItem(int offset, ItemCode code, int count)
        {
            m_items[offset].Code = code;
            m_items[offset].Count = count;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        public bool UpdateItem(int offset, int count)
        {
            if (m_items[offset].Count + count <= 99)
            {
                m_items[offset].Count += count;
                OnUpdatedSlot?.Invoke(offset, m_items[offset]);

                return true;
            }

            return false;
        }

        public void Clear(int offset)
        {
            m_items[offset].Code = ItemCode.NONE;
            m_items[offset].Count = 0;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        public int GetItemCount(ItemCode code)
        {
            var total_count = 0;

            foreach (var slot in m_items)
            {
                if (slot.Code == code)
                {
                    total_count += slot.Count;
                }
            }

            return total_count;
        }

        public bool HasItem(ItemCode code)
        {
            foreach (var slot in m_items)
            {
                if (slot.Code == code)
                {
                    return true;
                }
            }

            return false;
        }

        public ItemData GetItem(int offset)
        {
            return m_items[offset];
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Inventory", $"InventoryData{offset}.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var inventory_data = JsonUtility.FromJson<InventoryData>(json_data);

                m_money = inventory_data.Gold;
                m_items = inventory_data.Items;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Inventory", $"InventoryData{offset}.json");

            var inventory_data = new InventoryData(m_money, m_items);
            var json_data = JsonUtility.ToJson(inventory_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}