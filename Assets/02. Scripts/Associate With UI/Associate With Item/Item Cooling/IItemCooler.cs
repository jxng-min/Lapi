public interface IItemCooler
{
    void Push(ItemCode code, float cooltime);
    void Pop(ItemCode code);
    
    float GetCool(ItemCode code);
}