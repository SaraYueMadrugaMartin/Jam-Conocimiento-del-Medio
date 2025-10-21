using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePieces : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Variables
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform piecesPos; // Posición de la pieza movible.

    private Vector2 posInicial; // Posicion inicial de la pieza que se mueve.
    private List<RectTransform> cellPos; // Lista con las posiciones de todas las celdas posibles para poner las piezas.
    #endregion

    #region Basic Functions
    void Awake()
    {
        piecesPos = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        cellPos = new List<RectTransform>();
        posInicial = piecesPos.anchoredPosition; // Guarda la posición inicial de la pieza.

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        foreach (Transform childCell in GameObject.Find("Celdas").transform) // Buscamos el objeto "Celdas" para añadir sus hijos en la lista de posiciones de celdas.
            cellPos.Add(childCell.GetComponent<RectTransform>());
    }
    #endregion

    #region Métodos Move Piece
    public void OnBeginDrag(PointerEventData eventData)
    {        
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        piecesPos.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log("Estás moviendo la pieza.");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        RectTransform nearestPosCell = null;
        float minDistance = Mathf.Infinity;

        foreach (RectTransform cell in cellPos) // Se recorren todas las celdas posibles donde se pueden poner las piezas.
        {
            float distancePieceToCell = Vector2.Distance(piecesPos.anchoredPosition, cell.anchoredPosition); // Calculamos la distancia real a la que está la pieza de la celda.

            if (distancePieceToCell < minDistance) // Si la distancia a la celda es más pequeña que la anterior más cercana registrada, se asigna.
            {
                minDistance = distancePieceToCell;
                nearestPosCell = cell;
            }
        }

        if (nearestPosCell != null && minDistance < 100f) // Si hay una celda disponible y a menor distancia del umbral marcado, la pieza se pone en la posición de dicha celda.
        {
            piecesPos.anchoredPosition = nearestPosCell.anchoredPosition;
            Debug.Log("La pieza está bien colocada.");
        }
        else // Si no hay registrada ninguna celda y el umbral es mayor, la pieza vuelve a su posición inicial.
        {
            piecesPos.anchoredPosition = posInicial;
            Debug.Log("La pieza está mal colocada.");
        }
    }
    #endregion
}
