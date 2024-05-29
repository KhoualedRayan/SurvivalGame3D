using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Recipe : MonoBehaviour
{
    private RecipeData currentRecipe;

    [SerializeField]
    private Image crafatableItemImage;

    [SerializeField]
    private GameObject elementRequiredPrefab;

    [SerializeField]
    private Transform elementsRequiredParent;

    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private Sprite canBuildIcon;

    [SerializeField]
    private Sprite cantBuildIcon;

    [SerializeField]
    private Color missingColor;

    [SerializeField]
    private Color availableColor;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;
        crafatableItemImage.sprite = recipe.craftableItem.visual;
        //Ajout du tooltip
        crafatableItemImage.transform.parent.GetComponent<Slot>().SetItemData(recipe.craftableItem) ;

        bool canCraft = true;

        for(int i = 0; i< recipe.requiredItems.Length; i++)
        {
            //Récupère tt les élèments nécessaires pour le craft 
            GameObject requiredItemGO = Instantiate(elementRequiredPrefab, elementsRequiredParent);
            
            Image requiredItemGOImage = requiredItemGO.GetComponent<Image>();
            
            ItemData requiredItem = recipe.requiredItems[i].itemData;

            ElementRequired elementRequired = requiredItemGOImage.GetComponent<ElementRequired>();
            //ToolTip
            requiredItemGO.GetComponent<Slot>().SetItemData(requiredItem);


            //On vérifie si on a tout les objets dans l'inventaire && On met la couleur nécessaire au fond de l'image selon si il est disponible
            ItemInInventory[] itemInInventory = Inventory.instance.GetItemArrayIfExistsInInventory(requiredItem);

            int totalRequiredItemQuantityInInventory = 0;
            for (int y = 0; y < itemInInventory.Length; y++)
            {
                totalRequiredItemQuantityInInventory+= itemInInventory[y].count;
            }

            if (totalRequiredItemQuantityInInventory >= recipe.requiredItems[i].count) 
            {
                requiredItemGOImage.color = availableColor;
            }
            else
            {
                requiredItemGOImage.color = missingColor;
                canCraft = false;
            }

            //On instantie les images des items nécessaires au craft et les textes
            elementRequired.elementImage.sprite = recipe.requiredItems[i].itemData.visual;
            elementRequired.elementCountText.text = recipe.requiredItems[i].count.ToString();
        }
        //Gestion de l'affichage du bouton
        if (!canCraft)
        {
            craftButton.image.sprite = cantBuildIcon;
            craftButton.enabled = false;
        }
        ReziseElementsRequiredParent();
    }
    private void ReziseElementsRequiredParent()
    {
        Canvas.ForceUpdateCanvases();   
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }
    public void CraftItem()
    {
        foreach (ItemInInventory itemInInventory in currentRecipe.requiredItems)
        {
            for(int i = 0; i < itemInInventory.count; i++)
            {
                Inventory.instance.RemoveItem(itemInInventory.itemData);
            }
        }
        Inventory.instance.AddItem(currentRecipe.craftableItem);
    }
}
