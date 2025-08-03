using KeyService;
using TMPro;
using UnityEngine;

public class ShortcutSlotView : MonoBehaviour, IShortcutSlotView
{
    [Header("매핑 문자열")]
    [SerializeField] private string m_key_name;

    [Header("단축키 텍스트")]
    [SerializeField] private TMP_Text m_shortcut_key_text;

    private ShortcutSlotPresenter m_presenter;
    private IKeyService m_key_service;

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode(m_key_name)))
        {
            m_presenter.UseShortcut();
        }
    }

    public void Inject(ShortcutSlotPresenter presenter)
    {
        m_presenter = presenter;
        m_key_service = ServiceLocator.Get<IKeyService>();
    }

    public void UpdateUI(KeyCode code, string name)
    {
        if (m_key_name == name)
        {
            m_shortcut_key_text.text = ((char)code).ToString().ToUpper();
        }
    }
}
