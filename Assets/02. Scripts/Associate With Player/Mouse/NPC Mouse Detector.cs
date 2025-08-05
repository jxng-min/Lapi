using UnityEngine;

public class NPCMouseDetector : MouseDetector
{
    private NPC m_npc;
    private NameTagPresenter m_name_tag_presenter;

    private void Awake()
    {
        m_npc = GetComponent<NPC>();
    }

    public void Inject(NameTagPresenter presenter)
    {
        m_name_tag_presenter = presenter;
    }

    protected override void OnMouseEnter()
    {
        SetCursor(CursorMode.CAN_TALK);

        var target_position = (Vector2)transform.position + Vector2.up * 3f;

        m_name_tag_presenter.OpenUI(m_npc.Code, new System.Numerics.Vector2(target_position.x, target_position.y));
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();

        m_name_tag_presenter.CloseUI();
    }

    protected virtual void OnMouseDown()
    {
        m_npc.Interaction();
    }
}