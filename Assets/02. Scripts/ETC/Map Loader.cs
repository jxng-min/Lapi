using UnityEngine;
using UserService;
using QuestService;

[RequireComponent(typeof(BoxCollider2D))]
public class MapLoader : MonoBehaviour
{
    private IUserService m_user_service;
    private IQuestService m_quest_service;
    
    private PlayerCtrl m_player_ctrl;

    [Header("이동할 맵의 이름")]
    [SerializeField] private string m_map_name;

    [Header("맵의 플레이어 초기화 좌표")]
    [SerializeField] private Vector3 m_player_position;

    [Header("맵의 카메라 초기화 좌표")]
    [SerializeField] private Vector3 m_camera_position;

    [Header("이동 조건 퀘스트")]
    [SerializeField] private Quest m_quest;

    [Header("되돌아갈 좌표")]
    [SerializeField] private Vector3 m_return_position;

    public void Inject(IUserService user_service,
                       IQuestService quest_service,
                       PlayerCtrl player_ctrl)
    {
        m_user_service = user_service;
        m_quest_service = quest_service;
        m_player_ctrl = player_ctrl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(m_quest_service.GetQuestState(m_quest.ID) == QuestState.CLEARED)
            {
                m_user_service.Position = m_player_position;
                m_user_service.Camera = m_camera_position;
                m_user_service.Map = m_map_name;

                LoadingManager.Instance.LoadScene(m_map_name);
            }
            else
            {
                m_player_ctrl.transform.position = m_return_position;
            }
        }
    }
}
