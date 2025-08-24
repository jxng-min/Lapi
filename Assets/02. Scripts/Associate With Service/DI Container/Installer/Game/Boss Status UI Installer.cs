using UnityEngine;

public class BossStatusUIInstaller : MonoBehaviour, IInstaller
{
    [Header("보스 스테이터스 뷰")]
    [SerializeField] private BossStatusView m_boss_status_view;

    public void Install()
    {
        var boss_status_presenter = new BossStatusPresenter(m_boss_status_view);
        DIContainer.Register<BossStatusPresenter>(boss_status_presenter);
    }
}
