using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private float pickUpRange = 2.6f;
    public PickUpBehaviour playerPickUpBehaviour;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Si la cam�ra touche un objet droit devant lui avec pickUpRange, l'objet est stock� dans hit.
        if (Physics.Raycast(transform.position,transform.forward,out hit,pickUpRange))
        {
            //Si c'est un item
            if (hit.transform.CompareTag("Item"))
            {
                Debug.Log("There's an item in front of us.");
                //Et si on appuie sur E Alors
                if(Input.GetKeyDown(KeyCode.E))
                {
                    //On rajoute l'item � l'inventaire et le supprime de la sc�ne
                    playerPickUpBehaviour.DoPickUp(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
    }
}
