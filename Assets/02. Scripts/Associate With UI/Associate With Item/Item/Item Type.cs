[System.Flags]
public enum ItemType
{
    NONE = 0,

    Consumable = 1 << 0,
    Quest = 1 << 1,
    ETC = 1 << 2,

    Skill = 1 << 3,

    Equipment_Helmet = 1 << 4,
    Equipment_Armor = 1 << 5,
    Equipment_Weapon = 1 << 6,
    Equipment_Shield = 1 << 7,
}