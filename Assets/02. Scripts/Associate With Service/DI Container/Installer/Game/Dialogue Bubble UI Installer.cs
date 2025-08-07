using DialogueService;
using UnityEngine;

public class DialogueBubbleUIInstaller : MonoBehaviour, IInstaller
{
    [Header("대화 뷰")]
    [SerializeField] private DialogueView m_diaglogue_view;

    public void Install()
    {
        var dialogue_presenter = new DialoguePresenter(m_diaglogue_view,
                                                       ServiceLocator.Get<IDialogueService>());
        DIContainer.Register<DialoguePresenter>(dialogue_presenter);
    }
}
