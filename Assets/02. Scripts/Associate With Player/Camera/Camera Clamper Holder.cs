using UnityEngine;

public class CameraClamperHolder : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    private Coroutine m_wrap_coroutine;


    public PlayerCtrl Player => m_player_ctrl;
    public Coroutine WarpCoroutine
    {
        get => m_wrap_coroutine;
        set => m_wrap_coroutine = value;
    }
}
