using System;
using InventoryService;

namespace SkillService
{
    public interface ISkillService
    {
        event Action<int> OnUpdatedPoint;
        event Action<int, ItemData> OnUpdatedSlot;

        public int SkillPoint { get; }

        void InitializePoint();
        void InitializeSlot(int offset);
        void SetSkill(int offset, ItemCode code, int level);
        void UpdatePoint(int amount);
        void UpdateSkill(int offset, int count, int point = -1);
        int GetSkillLevel(ItemCode code);
        ItemData GetSkill(int offset);
        int GetOffset(ItemCode code);
    }
}