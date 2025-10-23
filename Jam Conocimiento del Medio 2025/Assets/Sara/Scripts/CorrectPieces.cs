using System.Collections.Generic;
using UnityEngine;

public class CorrectPieces : MonoBehaviour
{
    [SerializeField] private PieceData[] validPieces;
    [SerializeField] public GameObject currentPiece;
    public static List<RectTransform> pathPosition = new List<RectTransform>();

    [HideInInspector] public PuzzleManager puzzleManager;

    public bool IsPieceValid(PieceData piece)
    {
        foreach (PieceData valid in validPieces)
        {
            if (valid == piece)
                return true;
        }
        return false;
    }

    public void UpdatePath()
    {
        if (currentPiece == null) return;

        TypePiece pieceScript = currentPiece.GetComponent<TypePiece>();
        RectTransform position = GetComponent<RectTransform>();

        if (pieceScript != null && IsPieceValid(pieceScript.pieceData))
        {
            if (!pathPosition.Contains(position))
                pathPosition.Add(position);
        }
        else
            pathPosition.Remove(position);
    }

    public void SetPiece(GameObject piece)
    {
        currentPiece = piece;
        UpdatePath();
    }
}
