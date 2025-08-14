using QuestDataService;
using QuestService;
using UnityEngine;

public class QuestUIInstaller : MonoBehaviour, IInstaller
{
    [Header("퀘스트 데이터베이스")]
    [SerializeField] private QuestDataBase m_quest_db;

    [Header("풀 퀘스트 뷰")]
    [SerializeField] private FullQuestView m_full_quest_view;

    public void Install()
    {
        DIContainer.Register<IQuestDataBase>(m_quest_db);

        var quest_service = ServiceLocator.Get<IQuestService>();
        quest_service.Inject(m_quest_db);

        var full_quest_presenter = new FullQuestPresenter(m_full_quest_view,
                                                          ServiceLocator.Get<IQuestService>(),
                                                          ServiceLocator.Get<IQuestDataService>());
        DIContainer.Register<FullQuestPresenter>(full_quest_presenter);
    }
}
