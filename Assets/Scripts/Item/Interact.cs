using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactRange = 2.6f;
    
    public InteractBehaviour playerInteractBehaviour;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Si la caméra touche un objet droit devant lui selon interactRange, l'objet est stocké dans hit.
        if (Physics.Raycast(transform.position,transform.forward,out hit, interactRange, layerMask))
        {
            //On affiche le text d'interaction
            interactText.SetActive(true);
            //Si on appuie sur E Alors
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Si c'est un item
                if (hit.transform.CompareTag("Item"))
                {
                    //On rajoute l'item à l'inventaire et le supprime de la scène
                    playerInteractBehaviour.DoPickUp(hit.transform.gameObject.GetComponent<Item>());
                }
                //Si c'est un harvestable (collectable)
                if (hit.transform.CompareTag("Harvestable"))
                {
                    playerInteractBehaviour.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
                    print("On a interragi avec l'objet : " + hit.transform.name);
                }
            }

        }
        else
        {
            interactText.SetActive(false);
        }
    }
}
