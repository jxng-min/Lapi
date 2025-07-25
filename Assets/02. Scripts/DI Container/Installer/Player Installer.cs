using UnityEngine;

public class PlayerInstaller : MonoBehaviour, IInstaller
{
    [Header("플레이어 컨트롤러 컴포넌트")]
    [SerializeField] private PlayerCtrl m_controller;

    [Space(30f)]
    [Header("플레이어의 이동 컴포넌트")]
    [SerializeField] private PlayerMovement m_movement;

    [Header("플레이어의 공격 컴포넌트")]
    [SerializeField] private PlayerAttack m_attack;

    [Header("플레이어의 상태 컴포넌트")]
    [SerializeField] private PlayerStatus m_status;

    [Space(30f)]
    [Header("기본 능력치")]
    [SerializeField] private DefaultStatus m_default_status;

    [Header("성장 능력치")]
    [SerializeField] private GrowthStatus m_growth_status;

    [Space(30f)]
    [Header("기본 무기")]
    [SerializeField] private Weapon m_weapon;

    public void Install()
    {
        DIContainer.Register<PlayerCtrl>(m_controller);

        DIContainer.Register<IAttack>(m_attack);
        DIContainer.Register<IMovement>(m_movement);
        DIContainer.Register<IStatus>(m_status);

        DIContainer.Register<DefaultStatus>(m_default_status);
        DIContainer.Register<GrowthStatus>(m_growth_status);

        DIContainer.Register<Weapon>(m_weapon);
    }
}
