using UnityEngine;

public class MouseDetectorInstaller : MonoBehaviour, IInstaller
{
    [Header("NPC의 부모 트랜스폼")]
    [SerializeField] private Transform m_npc_root;

    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    public void Install()
    {
        DIContainer.Register<ICursorDataBase>(m_cursor_db);
        
        var npcs = m_npc_root.GetComponentsInChildren<MouseDetector>();
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].Inject(m_cursor_db);
        }
    }
}
