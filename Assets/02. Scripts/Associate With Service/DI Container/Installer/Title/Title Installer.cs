using UnityEngine;
using UnityEngine.UI;

public class TitleInstaller : MonoBehaviour, IInstaller
{
    [Header("시작 버튼")]
    [SerializeField] private Button m_start_button;

    [Header("종료 버튼")]
    [SerializeField] private Button m_exit_button;

    public void Install()
    {
        m_start_button.onClick.AddListener(OnClickedStart);
        m_exit_button.onClick.AddListener(OnClickedExit);
    }

    public void OnClickedStart()
    {
        LoadingManager.Instance.LoadScene("Game");
    }

    public void OnClickedExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
