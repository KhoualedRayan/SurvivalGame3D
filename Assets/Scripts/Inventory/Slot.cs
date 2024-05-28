using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private Image itemVisual;

    [SerializeField] 
    private Text countText;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null) { 
            ToolTipSystem.instance.Show(itemData.description,itemData.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.instance.Hide();
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }
    public void SetItemVisual(Sprite itemVisual)
    {
        this.itemVisual.sprite = itemVisual;
    }
    public void ClickOnSlot()
    {
        if(itemData != null)
        {
            itemActionsSystem.OpenActionPanel(itemData, transform.position);
        }
        
    }
    public Text GetCountText() {  return countText; }
    public void SetCountText(string _countText) {
        this.countText.text = _countText; 
    }
    public ItemData GetItemData() {  return itemData; }
}
