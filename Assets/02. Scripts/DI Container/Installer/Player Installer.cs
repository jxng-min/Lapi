using UnityEngine;

public class PlayerInstaller : MonoBehaviour, IInstaller
{
    [Header("플레이어 컨트롤러 컴포넌트")]
    [SerializeField] private PlayerCtrl m_controller;

    [Header("플레이어의 이동 컴포넌트")]
    [SerializeField] private PlayerMovement m_movement;

    [Header("플레이어의 공격 컴포넌트")]
    [SerializeField] private PlayerAttack m_attack;

    [Header("플레이어의 상태 컴포넌트")]
    [SerializeField] private PlayerStatus m_status;

    public void Install()
    {
        DIContainer.Register<PlayerCtrl>(m_controller);
        DIContainer.Register<IAttack>(m_attack);
        DIContainer.Register<IMovement>(m_movement);
        DIContainer.Register<IStatus>(m_status);
    }
}
