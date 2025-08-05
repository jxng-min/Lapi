using EquipmentService;
using UnityEngine;
using UserService;

public class PlayerAttack : MonoBehaviour, IAttack
{
    private PlayerCtrl m_controller;
    private IUserService m_user_service;
    private IEquipmentService m_equipment_service;

    private AttackUI[] m_weapons;
    private Weapon m_weapon;

    public float ATK => m_controller.DefaultStatus.ATK
                            + (m_user_service.Status.Level - 1) * m_controller.GrowthStatus.ATK
                            + m_controller.EquipmentEffect.ATK;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    #region Methods
    public void Inject(IUserService user_service, IEquipmentService equipment_service, AttackUI[] weapons)
    {
        m_user_service = user_service;
        m_equipment_service = equipment_service;

        m_weapons = weapons;

        m_equipment_service.OnUpdatedWeapon += SwapWeapon;
    }

    private void SwapWeapon(WeaponType type, float cooltime)
    {
        foreach (var weapon in m_weapons)
        {
            if (weapon.Type == type)
            {
                weapon.Interface.gameObject.SetActive(true);
                m_weapon = weapon.Interface;
                m_weapon.Initialize(cooltime);
            }
            else
            {
                weapon.Interface.gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        m_weapon.Use();
    }
    #endregion Methods
}