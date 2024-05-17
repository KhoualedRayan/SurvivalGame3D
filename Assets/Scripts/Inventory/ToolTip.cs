using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ToolTip : MonoBehaviour
{
    [SerializeField]
    private Text header;
    [SerializeField]
    private Text content;

    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private int maxCharacter;

    [SerializeField]
    private RectTransform rectTransform;
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.y = mousePosition.y - 50;
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        
        transform.position = mousePosition;

    }
    public void SetText(string content,string header = "")
    {
        if(header == "")
        {
            this.header.gameObject.SetActive(false);
        }
        else
        {
            this.header.gameObject.SetActive(true);
            this.header.text = header;
        }
        this.content.text = content;

        int headerLength = this.header.text.Length;
        int contentLength = this.content.text.Length;
        this.layoutElement.enabled = (headerLength > this.maxCharacter || contentLength > this.maxCharacter) ? true : false;
    }
}
