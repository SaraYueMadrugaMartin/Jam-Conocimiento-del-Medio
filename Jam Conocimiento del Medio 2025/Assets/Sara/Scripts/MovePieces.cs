using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePieces : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Variables 
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private RectTransform piecesPos;
    private Vector2 posInicial;
    [SerializeField] private float pieceThreshold = 150f;
    private List<RectTransform> cellPos;
    private HoverCell nearestCell = null;
    private HoverCell lastHighlightedCell;
    [SerializeField] private float hoverThreshold = 150f;
    private CorrectPieces currentCell = null;
    private bool isPlaced = false;
    #endregion

    #region Basic Functions 
    void Awake()
    {
        piecesPos = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        cellPos = new List<RectTransform>();
        posInicial = piecesPos.anchoredPosition;

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

        if (currentCell != null && currentCell.currentPiece == this.gameObject)
        {
            currentCell.SetPiece(null);
            currentCell = null;
        }

        piecesPos.SetAsLastSibling();
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
                Check.Instance.Show(nearestCell.GetComponent<RectTransform>());
            else
                Check.Instance.Hide();

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
            float distance = Vector2.Distance(piecesPos.anchoredPosition, cell.anchoredPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPosCell = cell;
            }
        }

        CorrectPieces cellScript = nearestPosCell?.GetComponent<CorrectPieces>();
        bool canPlace = cellScript != null && cellScript.currentPiece == null && minDistance < pieceThreshold;

        if (canPlace)
        {
            piecesPos.anchoredPosition = nearestPosCell.anchoredPosition;
            cellScript.SetPiece(this.gameObject);

            if (!isPlaced)
            {
                counterManager?.RemovePiece(pieceScript.pieceData.pieceType);
                isPlaced = true;
            }

            currentCell = cellScript;
            SFXManager.Instance.PlaySFX("PlacedTile");
        }
        else
        {
            piecesPos.anchoredPosition = posInicial;

            if (isPlaced)
            {
                counterManager?.AddPiece(pieceScript.pieceData.pieceType);
                isPlaced = false;
            }
        }

        if (lastHighlightedCell != null)
        {
            lastHighlightedCell.SetHighlight(false);
            lastHighlightedCell = null;
        }

        nearestCell = null;
        Check.Instance.Hide();
    }

    public void ReturnToStart()
    {
        TypePiece pieceScript = GetComponent<TypePiece>();
        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();

        piecesPos.anchoredPosition = posInicial;

        if (currentCell != null)
        {
            currentCell.SetPiece(null);
            currentCell = null;
        }

        if (isPlaced)
        {
            counterManager?.AddPiece(pieceScript.pieceData.pieceType);
            isPlaced = false;
        }
    }
    #endregion
}