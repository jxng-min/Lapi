using System;
using System.IO;
using InventoryService;
using UnityEngine;

namespace EquipmentService
{
    #region Serialization
    [System.Serializable]
    public class EquipmentData
    {
        public ItemData[] Equipments;

        public EquipmentData()
        {
            Equipments = new ItemData[4];
            for (int i = 0; i < 4; i++)
            {
                Equipments[i] = new ItemData();
            }
        }

        public EquipmentData(ItemData[] equipments)
        {
            Equipments = equipments;
        }
    }
    #endregion Serialization

    public class LocalEquipmentService : ISaveable, IEquipmentService
    {
        private IItemDataBase m_item_db;
        private ItemData[] m_equipments;
        private EquipmentEffect m_current_effect;

        public event Action<WeaponType> OnUpdatedWeapon;
        public event Action<int, ItemData> OnUpdatedSlot;
        public event Action<EquipmentEffect> OnUpdatedEffect;

        public LocalEquipmentService()
        {
            var equipment_data = new EquipmentData();
            m_equipments = equipment_data.Equipments;

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Equipment");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Equipment 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        public void Inject(IItemDataBase item_db)
        {
            m_item_db = item_db;
            Debug.Log("주입함");
        }

        public void InitializeSlot(int offset)
        {
            if (offset == 2)
            {
                var code = m_equipments[offset].Code;
                if (code == ItemCode.NONE)
                {
                    OnUpdatedWeapon?.Invoke(WeaponType.NONE);
                }
                else
                {
                    var item = m_item_db.GetItem(code) as WeaponItem;
                    OnUpdatedWeapon?.Invoke(item.WeaponType);
                }
            }

            OnUpdatedSlot?.Invoke(offset, m_equipments[offset]);
            Calculation();
        }

        public void AddItem(ItemCode code)
        {
            var item = m_item_db.GetItem(code);
            var offset = GetOffset(item.Type);

            SetItem(offset, code);
        }

        public void SetItem(int offset, ItemCode code)
        {
            m_equipments[offset].Code = code;
            m_equipments[offset].Count = 1;

            if (offset == 2)
            {
                if (code == ItemCode.NONE)
                {
                    OnUpdatedWeapon?.Invoke(WeaponType.NONE);
                }
                else
                {
                    var item = m_item_db.GetItem(code) as WeaponItem;
                    OnUpdatedWeapon?.Invoke(item.WeaponType);
                }
            }

            Calculation();

            OnUpdatedSlot?.Invoke(offset, m_equipments[offset]);
        }

        public ItemData GetItem(int offset)
        {
            return m_equipments[offset];
        }

        public int GetOffset(ItemType type)
        {
            switch (type)
            {
                case ItemType.Equipment_Helmet:
                    return 0;

                case ItemType.Equipment_Armor:
                    return 1;

                case ItemType.Equipment_Weapon:
                    return 2;

                case ItemType.Equipment_Shield:
                    return 3;
            }

            return -1;
        }

        public void Clear(int offset)
        {
            m_equipments[offset].Code = ItemCode.NONE;
            m_equipments[offset].Count = 0;

            if (offset == 2)
            {
                var code = m_equipments[offset].Code;
                if (code == ItemCode.NONE)
                {
                    OnUpdatedWeapon?.Invoke(WeaponType.NONE);
                }
                else
                {
                    var item = m_item_db.GetItem(code) as WeaponItem;
                    OnUpdatedWeapon?.Invoke(item.WeaponType);
                }
            }

            Calculation();

            OnUpdatedSlot?.Invoke(offset, m_equipments[offset]);
        }

        private void Calculation()
        {
            var calculated_effect = new EquipmentEffect();

            foreach (var slot in m_equipments)
            {
                if (slot.Code == ItemCode.NONE)
                {
                    continue;
                }

                Debug.Log(m_item_db);
                var item = m_item_db.GetItem(slot.Code) as EquipmentItem;
                calculated_effect += item.EquipmentEffect;
            }

            m_current_effect = calculated_effect;

            OnUpdatedEffect?.Invoke(m_current_effect);
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Equipment", $"EquipmentData{offset}.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var equipment_data = JsonUtility.FromJson<EquipmentData>(json_data);

                m_equipments = equipment_data.Equipments;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Equipment", $"EquipmentData{offset}.json");

            var equipment_data = new EquipmentData(m_equipments);
            var json_data = JsonUtility.ToJson(equipment_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}