using UnityEngine;
using UnityEngine.UI;

public class HoverCell : MonoBehaviour
{
    private Image borderImage;

    private void Awake()
    {
        borderImage = GetComponent<Image>();

        if (borderImage != null)
        {
            borderImage.enabled = true;
        }
    }

    public void SetHighlight(bool state)
    {
        if (borderImage == null) return;

        if (state)
        {
            borderImage.enabled = true;
        }
        else
        {
            borderImage.enabled = true;
        }
    }
}