using System.Numerics;
using DialogueService;

public class DialoguePresenter
{
    private readonly IDialogueView m_view;
    private readonly IDialogueService m_dialogue_service;

    private NPC m_npc;
    private DialogueData m_dialogue_data;
    private int m_context_index;

    private bool m_is_open;

    public bool IsOpen => m_is_open;

    public DialoguePresenter(IDialogueView view,
                             IDialogueService dialogue_service)
    {
        m_view = view;
        m_dialogue_service = dialogue_service;

        m_view.Inject(this);
    }

    public void OpenUI(NPC npc, int dialogue_id, Vector2 position)
    {
        m_is_open = true;

        m_npc = npc;
        m_dialogue_data = m_dialogue_service.GetDialogue(dialogue_id);
        m_context_index = 0;

        m_view.OpenUI(position);
        SetUI();
    }

    public void UpdateUI()
    {
        if (!m_is_open)
        {
            return;
        }

        if (m_context_index < m_dialogue_data.Contexts.Length - 1)
        {
            m_context_index++;
            SetUI();
        }
        else
        {
            m_is_open = false;

            m_view.CloseUI();
            m_npc.OnCompletedDialogue?.Invoke();
        }
    }

    private void SetUI()
    {
        m_view.UpdateUI(m_dialogue_data.Contexts[m_context_index].Context);
    }
}