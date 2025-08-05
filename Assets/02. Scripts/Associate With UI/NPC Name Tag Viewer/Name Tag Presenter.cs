using System.Numerics;
using NPCService;

public class NameTagPresenter
{
    private readonly INameTagView m_view;
    private readonly INPCService m_npc_service;

    public NameTagPresenter(INameTagView view, INPCService npc_service)
    {
        m_view = view;
        m_npc_service = npc_service;
    }

    public void OpenUI(NPCCode code, Vector2 position)
    {
        var name = m_npc_service.GetName(code);
        m_view.OpenUI(name, position);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }
}
