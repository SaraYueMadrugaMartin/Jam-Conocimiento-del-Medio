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
            borderImage.color = Color.red; // Al empezar, se pone por defecto la celda de color base (rojo).
            // TODO: cambiar al sprite base.
        }
    }

    public void SetHighlight(bool state) // Método que establece si debe cambiar de color o no.
    {
        if (borderImage == null) return;

        if (state)
            borderImage.color = Color.green; // TODO: cambiar al sprite con recuadro verde.
        else
            borderImage.color = Color.red;
    }
}
