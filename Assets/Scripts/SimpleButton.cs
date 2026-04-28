using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
    
    private Text buttonText;
    private Image buttonImage;
    
    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        buttonImage = GetComponent<Image>();
        SetColor(normalColor);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(hoverColor);
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(normalColor);
        transform.localScale = Vector3.one;
    }
    
    void SetColor(Color color)
    {
        if (buttonText != null) buttonText.color = color;
        if (buttonImage != null) buttonImage.color = color;
    }
}