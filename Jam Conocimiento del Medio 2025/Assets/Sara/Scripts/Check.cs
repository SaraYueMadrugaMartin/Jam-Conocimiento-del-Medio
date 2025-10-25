using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public static Check Instance;

    [SerializeField] private Image checkImage;

    private void Awake()
    {
        Instance = this;
        if (checkImage != null)
            checkImage.enabled = false;
    }

    public void Show(RectTransform cell)
    {
        if (checkImage == null || cell == null) return;

        checkImage.enabled = true;
        checkImage.rectTransform.position = cell.position;
    }

    public void Hide()
    {
        if (checkImage == null) return;

        checkImage.enabled = false;
    }
}