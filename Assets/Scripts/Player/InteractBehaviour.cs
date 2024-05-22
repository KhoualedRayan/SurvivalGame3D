using UnityEngine;
using static UnityEditor.Progress;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;
    [SerializeField]
    private GameObject pickaxeVisual;

    private Item currentItem;
    private Harvestable currentHarvestable;
    public void DoPickUp(Item item)
    {
        //Si l'enventaire on ne ramasse rien
        if(inventory.IsFull() )
        {
            Debug.Log("L'inventaire est plein, impossible de ramasser : " + item.name);
            return; 
        }
        currentItem = item;
        //Joue l'animation du personnage pour ramasser l'objet
        playerAnimator.SetTrigger("PickUp");
        //Bloquer les d�placements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public void DoHarvest(Harvestable harvestable)
    {
        pickaxeVisual.SetActive(true);
        currentHarvestable = harvestable;
        //Joue l'animation du personnage pour Miner
        playerAnimator.SetTrigger("Harvest");
        //Bloquer les d�placements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public void BreakHarvestable()
    {
        //Pour chaque ressource dans l'item � casser
        foreach (Ressource ressource in currentHarvestable.GetHarvestablesItems())
        {
            //On boucle avec un nombre al�atoire entre le min et le max d'items r�cup�rables en cassant l'item
            for(int i = 0; i< Random.Range(ressource.minAmountSpawned,(float) ressource.maxAmountSpawned); ++i)
            {
                //On les cr�e et les mets dans la sc�ne � l'endroit du collectable
                GameObject instantiatiedRessource = GameObject.Instantiate(ressource.itemData.prefab);
                instantiatiedRessource.transform.position = currentHarvestable.transform.position;
            }
        }
        //On d�truit l'objet
        Destroy(currentHarvestable.gameObject);
    }
    public void AddItemToInventory()
    {
        //Ajoute l'objet pass� � l'inventaire du joueur
        inventory.AddItem(currentItem.itemData);
        //D�truit l'objet de la sc�ne
        Destroy(currentItem.gameObject);
    }
    public void ReEnablePlayerMovements()
    {
        playerMoveBehaviour.SetCanMove(true);
        pickaxeVisual.SetActive(false);
    }
}
