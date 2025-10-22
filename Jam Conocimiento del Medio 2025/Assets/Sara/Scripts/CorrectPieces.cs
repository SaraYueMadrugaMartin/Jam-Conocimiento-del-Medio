using UnityEngine;

public class CorrectPieces : MonoBehaviour
{
    public PieceData[] validPieces;
    public GameObject currentPiece;

    public bool IsPieceValid(PieceData piece)
    {
        foreach (PieceData valid in validPieces)
        {
            if (valid == piece)
                return true;
        }
        return false;
    }

    public int GetScore()
    {
        if (currentPiece == null) return 0;

        TypePiece pieceScript = currentPiece.GetComponent<TypePiece>();
        if (pieceScript != null && IsPieceValid(pieceScript.pieceData))
            return 1;

        return 0;
    }

    public void SetPiece(GameObject piece)
    {
        currentPiece = piece;
    }
}
