using UnityEngine;

public class PauseUIInstaller : MonoBehaviour, IInstaller
{
    [Header("일시정지 뷰")]
    [SerializeField] private PauseView m_pause_view;

    public void Install()
    {
        var pause_presenter = new PausePresenter(m_pause_view);
    }
}
