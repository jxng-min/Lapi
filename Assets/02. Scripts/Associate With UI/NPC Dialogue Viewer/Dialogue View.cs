using UnityEngine;

public class DialogueView : MonoBehaviour, IDialogueView
{
    [Header("UI 관련 컴포넌트")]
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("타이핑 이펙터")]
    [SerializeField] private TypingEffector m_typing_effector;

    private DialoguePresenter m_presenter;

    private void Update()
    {
        if (!m_typing_effector.IsEffecting && Input.GetKeyDown(KeyCode.Space))
        {
            m_presenter.UpdateUI();
        }   
    }

    public void Inject(DialoguePresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI(System.Numerics.Vector2 position)
    {
        transform.position = new Vector2(position.X, position.Y);
        
        m_canvas_group.alpha = 1f;
    }

    public void UpdateUI(string context)
    {
        m_typing_effector.SetContext(context);
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
    }
}
