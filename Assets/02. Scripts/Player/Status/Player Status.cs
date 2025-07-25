using System;
using System.Collections;
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
                                + (m_user_service.Status.Level - 1) * m_controller.GrowthStatus.HP;

    public float MaxMP => m_controller.DefaultStatus.MP
                                + (m_user_service.Status.Level - 1) * m_controller.GrowthStatus.MP;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    #region Methods
    public void Inject(IUserService user_service)
    {
        m_user_service = user_service;
    }

    public void Initialize()
    {
        UpdateHP(0f);
        UpdateMP(0f);
    }

    public void UpdateHP(float amount)
    {
        m_user_service.Status.HP += amount;
        m_user_service.Status.HP = Mathf.Clamp(m_user_service.Status.HP, 0f, MaxHP);

        if (m_user_service.Status.HP <= 0f)
        {
            Death();
        }
        else
        {
            if (amount < 0f)
            {
                Damage();
            }
        }

        OnUpdatedHP?.Invoke(m_user_service.Status.HP, MaxHP);
    }

    public void UpdateMP(float amount)
    {
        m_user_service.Status.MP += amount;
        m_user_service.Status.MP = Mathf.Clamp(m_user_service.Status.MP, 0f, MaxMP);

        OnUpdatedMP?.Invoke(m_user_service.Status.MP, MaxMP);
    }

    private void Damage()
    {
        StartCoroutine(SetInvincibility());
    }

    public void Death()
    {
        if (m_is_dead)
        {
            return;
        }

        m_is_dead = true;

        StartCoroutine(SetDeath());
    }

    private void SetAlpha(float alpha)
    {
        var color = m_controller.Renderer.color;
        color.a = alpha;
        m_controller.Renderer.color = color;
    }

    private IEnumerator SetInvincibility()
    {
        float elapsed_time = 0f;
        float target_time = 2f;

        SetAlpha(0.5f);
        gameObject.layer = LayerMask.NameToLayer("INVINCIBILITY");

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            yield return null;
        }

        SetAlpha(1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private IEnumerator SetDeath()
    {
        float elapsed_time = 0f;
        float target_time = 1f;

        gameObject.layer = LayerMask.NameToLayer("INVINCIBILITY");

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            var delta = 1f - (elapsed_time / target_time);
            var color = new Color(delta, delta, delta);
            m_controller.Renderer.color = color;

            yield return null;
        }

        m_controller.Renderer.color = Color.black;
        Destroy(gameObject);
    }
    #endregion Methods
}
