public enum ObjectType
{
    // UI 타입(0 ~ 199)
    DAMAGE_INDICATOR = 0, DAMAGE = 1, SHOP_SLOT = 2, WORKSHOP_SLOT = 3, ITEM_SLOT = 4, SFX = 5,

    // 아이템 타입(200 ~ )
    ITEM = 200, BRONZE_COIN = 201, SILVER_COIN = 202, GOLD_COIN = 203,

    // 몬스터 타입(1000 ~)
    MELEE_ENEMY = 1000, RANGED_ENEMY = 1001,

    // 스킬 타입(2000 ~)
    ARROW = 2000, DASH = 2001, FIRE_BALL = 2002, THUNDER = 2003,
}