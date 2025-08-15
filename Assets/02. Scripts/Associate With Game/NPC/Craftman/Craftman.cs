using System;
using UnityEngine;

public class Craftman : NPC, IDisposable
{
    [Space(50f)]
    [Header("제작소 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("제작소에서 제작 가능한 레시피의 목록")]
    [SerializeField] protected ItemReceipe[] m_craft_list;

    private WorkshopPresenter m_workshop_presenter;

    public void Inject(WorkshopPresenter workshop_presenter)
    {
        m_workshop_presenter = workshop_presenter;

    }

    public override void Interaction()
    {
        if (m_dialogue_presenter.IsOpen)
        {
            return;
        }

        Rotation();
        OpenDialogue();
    }

    protected override void OpenDialogue()
    {
        if (IsExistQuest(out var quest))
        {
            DialogueAboutQuest(quest);
        }
        else
        {
            OnCompletedDialogue -= InjectToWorkshop;
            OnCompletedDialogue += InjectToWorkshop;

            OnCompletedDialogue -= OpenWorkshop;
            OnCompletedDialogue += OpenWorkshop;
            Dialogue(m_dialogue_id);
        }
    }

    private void InjectToWorkshop()
    {
        m_workshop_presenter.Inject(m_craft_list);
    }

    private void OpenWorkshop()
    {
        m_ui_manager.OpenUI(m_workshop_presenter);
    }

    public void Dispose()
    {
        OnCompletedDialogue -= InjectToWorkshop;
        OnCompletedDialogue -= OpenWorkshop;
    }
}
