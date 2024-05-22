using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [Header("EQUIPMENT SYSTEM VARIABLES")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;
    //Slots d'équipements dans l'inventaire

    [SerializeField]
    private Image headSlotImage;
    [SerializeField]
    private Image chestSlotImage;
    [SerializeField]
    private Image handsSlotImage;
    [SerializeField]
    private Image legsSlotImage;
    [SerializeField]
    private Image feetSlotImage;
    // Boutons de désequipement

    [SerializeField]
    private Button headSlotDesequipButton;
    [SerializeField]
    private Button chestSlotDesequipButton;
    [SerializeField]
    private Button handsSlotDesequipButton;
    [SerializeField]
    private Button legsSlotDesequipButton;
    [SerializeField]
    private Button feetSlotDesequipButton;


    // Garde une trace des équiments actuels
    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetItem;

    public void EquipAction()
    {
        //On cherche le modèle 3D de l'équipement à s'équiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemActionsSystem.GetItemCurrentlySelected()).First();
        if (equipmentLibraryItem != null)
        {
            //On met le sprite de l'item équipé dans le slot de l'inventaire
            switch (itemActionsSystem.GetItemCurrentlySelected().equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipedEquipment(equipedHeadItem);
                    headSlotImage.sprite = itemActionsSystem.GetItemCurrentlySelected().visual;
                    equipedHeadItem = itemActionsSystem.GetItemCurrentlySelected();
                    break;

                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemActionsSystem.GetItemCurrentlySelected().visual;
                    equipedChestItem = itemActionsSystem.GetItemCurrentlySelected();
                    break;

                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipedHandsItem);
                    handsSlotImage.sprite = itemActionsSystem.GetItemCurrentlySelected().visual;
                    equipedHandsItem = itemActionsSystem.GetItemCurrentlySelected();
                    break;

                case EquipmentType.Pants:
                    DisablePreviousEquipedEquipment(equipedLegsItem);
                    legsSlotImage.sprite = itemActionsSystem.GetItemCurrentlySelected().visual;
                    equipedLegsItem = itemActionsSystem.GetItemCurrentlySelected();
                    break;

                case EquipmentType.Feet:
                    DisablePreviousEquipedEquipment(equipedFeetItem);
                    feetSlotImage.sprite = itemActionsSystem.GetItemCurrentlySelected().visual;
                    equipedFeetItem = itemActionsSystem.GetItemCurrentlySelected();
                    break;
            }
            //On désactive les visuels inutiles pour éviter le clipping
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(false);
            }
            //On active le modèle 3D de l'équipement
            equipmentLibraryItem.itemPrefab.SetActive(true);
            // On enlève l'item de l'invetaire et rafrachit le contenu
            Inventory.instance.RemoveItem(itemActionsSystem.GetItemCurrentlySelected());
        }
        else
        {
            Debug.LogError("Equipement : " + itemActionsSystem.GetItemCurrentlySelected().name + " non existant dans la librairie des équipements.");
        }
        itemActionsSystem.CloseActionPanel();
    }
    public void DesequipEquipment(EquipmentType equipmentType)
    {
        //1. Enlever le visuel de l'équipement sur le personnage + ré-activer les parties visuelles qu'on avait désactiv& pour cet objet
        //2. Enlever le visuel de l'équipement de la colonne équipement de l'inventaire
        //3. Renvoyer l'équipement dans l'inventaire du joueur
        //4. Refresh Content
        ItemData currentItem = null;
        if (!Inventory.instance.IsFull())
        {
            switch (equipmentType)
            {
                case EquipmentType.Head:
                    currentItem = equipedHeadItem;
                    equipedHeadItem = null;
                    headSlotImage.sprite = Inventory.instance.GetEmptySlotVisual();
                    break;
                case EquipmentType.Chest:
                    currentItem = equipedChestItem;
                    equipedChestItem = null;
                    chestSlotImage.sprite = Inventory.instance.GetEmptySlotVisual();
                    break;
                case EquipmentType.Hands:
                    currentItem = equipedHandsItem;
                    equipedHandsItem = null;
                    handsSlotImage.sprite = Inventory.instance.GetEmptySlotVisual();
                    break;
                case EquipmentType.Pants:
                    currentItem = equipedLegsItem;
                    equipedLegsItem = null;
                    legsSlotImage.sprite = Inventory.instance.GetEmptySlotVisual();
                    break;
                case EquipmentType.Feet:
                    currentItem = equipedFeetItem;
                    equipedFeetItem = null;
                    feetSlotImage.sprite = Inventory.instance.GetEmptySlotVisual();
                    break;
            }
        }
        else
        {
            Debug.Log("L'invetaire est plein. Impossible de se déquipper de l'item : " + equipmentType.ToString());
            return;
        }
        //On cherche le modèle 3D de l'équipement à se déséquiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();
        if (equipmentLibraryItem != null)
        {
            //On active les visuels désactiver
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(true);
            }
            //On désactive le modèle 3D de l'équipement
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }
        Inventory.instance.AddItem(currentItem);
        Inventory.instance.RefreshContent();
    }

    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable != null)
        {
            //On cherche le modèle 3D de l'équipement à se déséquiper
            EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();
            if (equipmentLibraryItem != null)
            {
                //On active les visuels désactiver
                foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
                {
                    elem.SetActive(true);
                }
                //On désactive le modèle 3D de l'équipement
                equipmentLibraryItem.itemPrefab.SetActive(false);
            }
            Inventory.instance.AddItem(itemToDisable);
        }
    }
    public void UpdateEquipmentsDesequipButton()
    {
        headSlotDesequipButton.onClick.RemoveAllListeners();
        headSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Head); });
        headSlotDesequipButton.gameObject.SetActive(equipedHeadItem);

        chestSlotDesequipButton.onClick.RemoveAllListeners();
        chestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Chest); });
        chestSlotDesequipButton.gameObject.SetActive(equipedChestItem);

        handsSlotDesequipButton.onClick.RemoveAllListeners();
        handsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Hands); });
        handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem);

        legsSlotDesequipButton.onClick.RemoveAllListeners();
        legsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Pants); });
        legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem);

        feetSlotDesequipButton.onClick.RemoveAllListeners();
        feetSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Feet); });
        feetSlotDesequipButton.gameObject.SetActive(equipedFeetItem);

    }
}
