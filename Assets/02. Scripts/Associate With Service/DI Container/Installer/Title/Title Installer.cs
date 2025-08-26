using UnityEngine;
using UnityEngine.UI;

public class TitleInstaller : MonoBehaviour, IInstaller
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    [Header("시작 버튼")]
    [SerializeField] private Button m_start_button;

    [Header("종료 버튼")]
    [SerializeField] private Button m_exit_button;

    public void Install()
    {
        m_start_button.onClick.AddListener(OnClickedStart);
        m_exit_button.onClick.AddListener(OnClickedExit);

        GameEventBus.Publish(GameEventType.LOGIN);
        SoundManager.Instance.PlayBGM("Title");
    }

    public void OnClickedStart()
    {
        LoadingManager.Instance.LoadScene("Event");
        SoundManager.Instance.PlaySFX("Default");
    }

    public void OnClickedExit()
    {
        SoundManager.Instance.PlaySFX("Default");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
