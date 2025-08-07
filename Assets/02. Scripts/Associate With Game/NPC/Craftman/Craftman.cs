using UnityEngine;

public class Craftman : NPC
{
    [Space(50f)]
    [Header("제작소 관련 컴포넌트")]
    [Header("제작소에서 제작 가능한 레시피의 목록")]
    [SerializeField] protected ItemReceipe[] m_craft_list;

    private WorkshopPresenter m_workshop_presenter;

    public void Inject(WorkshopPresenter workshop_presenter)
    {
        m_workshop_presenter = workshop_presenter;
        OnCompletedDialogue += m_workshop_presenter.OpenUI;
    }

    public override void Interaction()
    {
        if (m_dialogue_presenter.IsOpen)
        {
            return;
        }

        m_workshop_presenter.Inject(m_craft_list);

        Rotation();
        OpenDialogue();
    }
}
