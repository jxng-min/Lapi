using EquipmentService;
using UnityEngine;
using UserService;

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
    [Header("무기 목록")]
    [SerializeField] private AttackUI[] m_weapons;

    public void Install()
    {
        DIContainer.Register<PlayerCtrl>(m_controller);

        DIContainer.Register<IAttack>(m_attack);
        DIContainer.Register<IMovement>(m_movement);
        DIContainer.Register<IStatus>(m_status);

        DIContainer.Register<DefaultStatus>(m_default_status);
        DIContainer.Register<GrowthStatus>(m_growth_status);

        Inject();
    }

    private void Inject()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();
        player_ctrl.EquipmentEffect = new EquipmentEffect();

        var movement = DIContainer.Resolve<IMovement>();

        var attack = DIContainer.Resolve<IAttack>();
        (attack as PlayerAttack).Inject(ServiceLocator.Get<IUserService>(),
                                        ServiceLocator.Get<IEquipmentService>(),
                                        m_weapons);

        var status = DIContainer.Resolve<IStatus>();
        (status as PlayerStatus).Inject(ServiceLocator.Get<IUserService>());

        var default_status = DIContainer.Resolve<DefaultStatus>();
        var growth_status = DIContainer.Resolve<GrowthStatus>();

        player_ctrl.Inject(movement, attack, status, default_status, growth_status);

        (status as PlayerStatus).Initialize();
    }
}
