public interface ISaveable
{
    bool Load(int offset);
    void Save(int offset);
}