using QuestDataService;
using QuestService;
using UnityEngine;

public class QuestUIInstaller : MonoBehaviour, IInstaller
{
    [Header("퀘스트 데이터베이스")]
    [SerializeField] private QuestDataBase m_quest_db;

    [Header("풀 퀘스트 뷰")]
    [SerializeField] private FullQuestView m_full_quest_view;

    [Header("컴팩트 퀘스트 뷰")]
    [SerializeField] private CompactQuestView m_compact_quest_view;

    public void Install()
    {
        DIContainer.Register<IQuestDataBase>(m_quest_db);

        var quest_service = ServiceLocator.Get<IQuestService>();
        quest_service.Inject(m_quest_db);

        var compact_quest_presenter = new CompactQuestPresenter(m_compact_quest_view,
                                                                ServiceLocator.Get<IQuestService>(),
                                                                ServiceLocator.Get<IQuestDataService>(),
                                                                m_quest_db);
        DIContainer.Register<CompactQuestPresenter>(compact_quest_presenter);
        DIContainer.Register<ICompactQuestView>(m_compact_quest_view);

        var full_quest_presenter = new FullQuestPresenter(m_full_quest_view,
                                                          ServiceLocator.Get<IQuestService>(),
                                                          ServiceLocator.Get<IQuestDataService>(),
                                                          compact_quest_presenter);
        DIContainer.Register<FullQuestPresenter>(full_quest_presenter);
        DIContainer.Register<IFullQuestSlotView>(m_full_quest_view);
    }
}
