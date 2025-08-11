using System.Numerics;

public interface IItemInteractionHandler
{
    void OnPointerEnter(SlotType slot_type, int offset);
    void OnPointerExit();

    void OnBeginDrag(Vector2 mouse_position, DragMode drag_mode);
    void OnDrag(Vector2 mouse_position);
    void OnEndDrag();
    void OnDrop(SlotType slot_type, int offset, bool masking);
    void OnPointerClick(SlotType slot_type, int offset);
}