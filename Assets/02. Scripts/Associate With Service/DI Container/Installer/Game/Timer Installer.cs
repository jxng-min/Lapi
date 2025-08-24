using UnityEngine;
using UserService;

public class TimerInstaller : MonoBehaviour, IInstaller
{
    [Header("게임 타이머")]
    [SerializeField] private Timer m_timer;

    public void Install()
    {
        m_timer.Inject(ServiceLocator.Get<IUserService>());
    }
}
