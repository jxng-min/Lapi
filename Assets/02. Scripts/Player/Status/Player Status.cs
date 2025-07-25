using System;
using UnityEngine;
using UserService;

public class PlayerStatus : MonoBehaviour, IStatus
{
    private IUserService m_user_service;
    private PlayerCtrl m_controller;

    public Action<float, float> OnUpdatedHP;
    public Action<float, float> OnUpdatedMP;

    private bool m_is_dead;

    public float MaxHP => m_controller.DefaultStatus.HP
                                + m_user_service.Status.Level * m_controller.GrowthStatus.HP;

    public float MaxMP => m_controller.DefaultStatus.MP
                                + m_user_service.Status.Level * m_controller.GrowthStatus.MP;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    private void Start()
    {
        Initialize();   
    }

    public void Inject(IUserService user_service)
    {
        m_user_service = user_service;
    }

    public void Initialize()
    {
        UpdateHP(0);
        UpdateMP(0);
    }

    public void UpdateHP(float amount)
    {
        m_user_service.Status.HP += amount;
        m_user_service.Status.HP = Mathf.Clamp(m_user_service.Status.HP, 0f, MaxHP);

        if (m_user_service.Status.HP <= 0f)
        {
            Death();
        }

        OnUpdatedHP?.Invoke(m_user_service.Status.HP, MaxHP);
    }

    public void UpdateMP(float amount)
    {
        m_user_service.Status.MP += amount;
        m_user_service.Status.MP = Mathf.Clamp(m_user_service.Status.MP, 0f, MaxMP);

        OnUpdatedMP?.Invoke(m_user_service.Status.MP, MaxMP);
    }

    public void Death()
    {
        if (m_is_dead)
        {
            return;
        }

        m_is_dead = true;
    }
}
