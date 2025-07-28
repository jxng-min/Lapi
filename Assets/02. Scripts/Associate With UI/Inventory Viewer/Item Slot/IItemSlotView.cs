using InventoryService;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemSlotView : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    void Inject(ItemSlotPresenter presenter);

    void ClearUI();
    void UpdateUI(Sprite item_image, bool stackable, int count);
}
