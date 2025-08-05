using NPCService;
using UnityEngine;

public class NPCInstaller : MonoBehaviour, IInstaller
{
    [Header("네임 태그 뷰")]
    [SerializeField] private NameTagView m_name_tag_view;

    [Header("NPC 부모 트랜스폼")]
    [SerializeField] private Transform m_npc_root;

    public void Install()
    {
        var name_tag_presenter = new NameTagPresenter(m_name_tag_view,
                                                      ServiceLocator.Get<INPCService>());

        var npcs = m_npc_root.GetComponentsInChildren<NPCMouseDetector>();
        foreach (var npc in npcs)
        {
            npc.Inject(name_tag_presenter);
        }
    }
}
