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
    //Slots d'�quipements dans l'inventaire

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
    // Boutons de d�sequipement

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


    // Garde une trace des �quiments actuels
    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetItem;

    public void EquipAction()
    {
        //On cherche le mod�le 3D de l'�quipement � s'�quiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemActionsSystem.GetItemCurrentlySelected()).First();
        if (equipmentLibraryItem != null)
        {
            //On met le sprite de l'item �quip� dans le slot de l'inventaire
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
            //On d�sactive les visuels inutiles pour �viter le clipping
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(false);
            }
            //On active le mod�le 3D de l'�quipement
            equipmentLibraryItem.itemPrefab.SetActive(true);
            // On enl�ve l'item de l'invetaire et rafrachit le contenu
            Inventory.instance.RemoveItem(itemActionsSystem.GetItemCurrentlySelected());
        }
        else
        {
            Debug.LogError("Equipement : " + itemActionsSystem.GetItemCurrentlySelected().name + " non existant dans la librairie des �quipements.");
        }
        itemActionsSystem.CloseActionPanel();
    }
    public void DesequipEquipment(EquipmentType equipmentType)
    {
        //1. Enlever le visuel de l'�quipement sur le personnage + r�-activer les parties visuelles qu'on avait d�sactiv& pour cet objet
        //2. Enlever le visuel de l'�quipement de la colonne �quipement de l'inventaire
        //3. Renvoyer l'�quipement dans l'inventaire du joueur
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
            Debug.Log("L'invetaire est plein. Impossible de se d�quipper de l'item : " + equipmentType.ToString());
            return;
        }
        //On cherche le mod�le 3D de l'�quipement � se d�s�quiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();
        if (equipmentLibraryItem != null)
        {
            //On active les visuels d�sactiver
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(true);
            }
            //On d�sactive le mod�le 3D de l'�quipement
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }
        Inventory.instance.AddItem(currentItem);
        Inventory.instance.RefreshContent();
    }

    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable != null)
        {
            //On cherche le mod�le 3D de l'�quipement � se d�s�quiper
            EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();
            if (equipmentLibraryItem != null)
            {
                //On active les visuels d�sactiver
                foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
                {
                    elem.SetActive(true);
                }
                //On d�sactive le mod�le 3D de l'�quipement
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
