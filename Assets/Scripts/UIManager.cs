using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;

    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCameraScript;

    private float defautHorizontalAimingSpeed;

    private float defautVerticalAimingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        defautHorizontalAimingSpeed = playerCameraScript.horizontalAimingSpeed;
        defautVerticalAimingSpeed = playerCameraScript.verticalAimingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(UIPanels.Any(panel => panel== panel.activeSelf))
        {
            playerCameraScript.horizontalAimingSpeed = 0;
            playerCameraScript.verticalAimingSpeed = 0;
        }
        else
        {
            playerCameraScript.horizontalAimingSpeed = defautHorizontalAimingSpeed;
            playerCameraScript.verticalAimingSpeed= defautVerticalAimingSpeed;
        }
    }
}
