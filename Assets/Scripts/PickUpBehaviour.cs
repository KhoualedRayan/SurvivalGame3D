using UnityEngine;
using static UnityEditor.Progress;

public class PickUpBehaviour : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;
    private Item currentItem;
    public void DoPickUp(Item item)
    {
        currentItem = item;
        //Joue l'animation du personnage pour ramasser l'objet
        playerAnimator.SetTrigger("PickUp");
        //Bloquer les déplacements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public void AddItemToInventory()
    {
        //Ajoute l'objet passé à l'inventaire du joueur
        inventory.AddItem(currentItem.itemData);
        //Détruit l'objet de la scène
        Destroy(currentItem.gameObject);
        currentItem = null;
    }
    public void ReEnablePlayerMovements()
    {
        playerMoveBehaviour.SetCanMove(true);
    }
}
