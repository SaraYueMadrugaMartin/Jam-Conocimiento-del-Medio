using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePieces : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Variables
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform piecesPos; // Posición de la pieza movible.

    private Vector2 posInicial; // Posición inicial de la pieza que se mueve.
    [SerializeField] private float pieceThreshold = 150f;
    private List<RectTransform> cellPos; // Lista con las posiciones de todas las celdas posibles para poner las piezas.

    private HoverCell nearestCell = null;
    private HoverCell lastHighlightedCell;
    [SerializeField] private float hoverThreshold = 150f;
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

        foreach (Transform childCell in GameObject.Find("Celdas").transform)
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

        HoverCell closestCell = null;
        float minDistance = Mathf.Infinity;

        foreach (RectTransform cell in cellPos)
        {
            float dist = Vector2.Distance(piecesPos.anchoredPosition, cell.anchoredPosition);

            if (dist < minDistance)
            {
                minDistance = dist;
                closestCell = cell.GetComponent<HoverCell>();
            }
        }

        nearestCell = (minDistance < hoverThreshold) ? closestCell : null;

        if (nearestCell != lastHighlightedCell)
        {
            if (lastHighlightedCell != null)
                lastHighlightedCell.SetHighlight(false);

            if (nearestCell != null)
                nearestCell.SetHighlight(true);

            lastHighlightedCell = nearestCell;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        TypePiece pieceScript = GetComponent<TypePiece>();
        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();

        RectTransform nearestPosCell = null;
        float minDistance = Mathf.Infinity;

        foreach (RectTransform cell in cellPos)
        {
            float distancePieceToCell = Vector2.Distance(piecesPos.anchoredPosition, cell.anchoredPosition);

            if (distancePieceToCell < minDistance)
            {
                minDistance = distancePieceToCell;
                nearestPosCell = cell;
            }
        }

        CorrectPieces cellScript = nearestPosCell?.GetComponent<CorrectPieces>();
        bool canPlace = cellScript != null && cellScript.currentPiece == null && minDistance < pieceThreshold;

        if (canPlace)
        {
            piecesPos.anchoredPosition = nearestPosCell.anchoredPosition;
            cellScript.SetPiece(this.gameObject);
            counterManager?.RemovePiece(pieceScript.pieceData.pieceType);
            SFXManager.Instance.PlaySFX("PlacedTile");
        }
        else
        {
            piecesPos.anchoredPosition = posInicial;
            counterManager?.AddPiece(pieceScript.pieceData.pieceType);
        }

        if (lastHighlightedCell != null)
        {
            lastHighlightedCell.SetHighlight(false);
            lastHighlightedCell = null;
        }

        nearestCell = null;
    }

    public void ReturnToStart()
    {
        TypePiece pieceScript = GetComponent<TypePiece>();
        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();

        piecesPos.anchoredPosition = posInicial;
        counterManager?.AddPiece(pieceScript.pieceData.pieceType);
    }
    #endregion
}
