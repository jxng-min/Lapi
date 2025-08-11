public class ItemInteractionHandler : IItemInteractionHandler
{
    private SlotPointerHandler m_slot_pointer_handler;
    private SlotDragHandler m_slot_drag_handler;
    private SlotDropHandler m_slot_drop_handler;

    private SlotType m_slot_type;
    private int m_offset;

    public ItemInteractionHandler(SlotPointerHandler pointer_handler,
                                  SlotDragHandler drag_handler,
                                  SlotDropHandler drop_handler)
    {
        m_slot_pointer_handler = pointer_handler;
        m_slot_drag_handler = drag_handler;
        m_slot_drop_handler = drop_handler;
    }

    public void OnPointerEnter(SlotType slot_type, int offset)
    {
        m_slot_type = slot_type;
        m_offset = offset;

        m_slot_pointer_handler.OnPointerEnter(slot_type, offset);
    }

    public void OnPointerExit()
    {
        m_slot_pointer_handler.OnPointerExit();
    }

    public void OnPointerClick(SlotType slot_type, int offset)
    {
        m_slot_pointer_handler.OnPointerClick(slot_type, offset);
    }

    public void OnBeginDrag(System.Numerics.Vector2 mouse_position, DragMode drag_mode)
    {
        m_slot_drag_handler.OnBeginDrag(mouse_position, drag_mode, m_slot_type, m_offset);
    }

    public void OnDrag(System.Numerics.Vector2 mouse_position)
    {
        m_slot_drag_handler.OnDrag(mouse_position);
    }

    public void OnEndDrag()
    {
        m_slot_drag_handler.OnEndDrag();
    }

    public void OnDrop(SlotType slot_type, int offset, bool masking)
    {
        m_slot_drop_handler.OnDrop(slot_type, offset, masking);
    }
}