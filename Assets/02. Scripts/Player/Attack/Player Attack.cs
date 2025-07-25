using UnityEngine;
using UserService;

public class PlayerAttack : MonoBehaviour, IAttack
{
    private PlayerCtrl m_controller;
    private IUserService m_user_service;

    private Weapon m_weapon;

    public float ATK => m_controller.DefaultStatus.ATK
                            + m_user_service.Status.Level * m_controller.GrowthStatus.ATK;

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
    public void Inject(IUserService user_service, Weapon weapon)
    {
        m_user_service = user_service;
        m_weapon = weapon;
    }

    public void Attack()
    {
        m_weapon.Use();
    }
    #endregion Methods
}