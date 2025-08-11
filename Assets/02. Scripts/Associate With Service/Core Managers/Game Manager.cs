using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    private GameEventType m_current_event;

    public GameEventType Event => m_current_event;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.LOGIN, Login);
        GameEventBus.Subscribe(GameEventType.LOADING, Loading);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.LOGIN, Login);
        GameEventBus.Unsubscribe(GameEventType.LOADING, Loading);        
    }

    public void Login()
    {
        m_current_event = GameEventType.LOGIN;
        m_cursor_db.SetCursor(CursorMode.DEFAULT);
    }

    public void Loading()
    {
        m_current_event = GameEventType.LOADING;
        m_cursor_db.SetCursor(CursorMode.WAITING);
    }

    public void Playing()
    {
        m_current_event = GameEventType.PLAYING;
    }

    public void Interacting()
    {
        m_current_event = GameEventType.INTERACTING;
    }

    public void Events()
    {
        m_current_event = GameEventType.EVENT;
    }

    public void Setting()
    {
        m_current_event = GameEventType.SETTING;
        m_cursor_db.SetCursor(CursorMode.DEFAULT);
    }
}
