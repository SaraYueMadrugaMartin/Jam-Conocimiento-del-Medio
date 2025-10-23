using System.Collections.Generic;
using UnityEngine;

public class CorrectPieces : MonoBehaviour
{
    public PieceData[] validPieces;
    public GameObject currentPiece;

    public static List<RectTransform> pathPosition = new List<RectTransform>();

    /*
     * Método para comparar si las piezas son las admitidas por las celdas.
     */
    public bool IsPieceValid(PieceData piece)
    {
        foreach (PieceData valid in validPieces)
        {
            if (valid == piece)
                return true;
        }
        return false;
    }

    /*
     * Método para sumar puntos al crear el recorrido.
     */
    public int GetScore()
    {
        if (currentPiece == null) return 0;

        TypePiece pieceScript = currentPiece.GetComponent<TypePiece>();
        RectTransform position = GetComponent<RectTransform>();

        if (pieceScript != null && IsPieceValid(pieceScript.pieceData))
        {
            if(!pathPosition.Contains(position))
                pathPosition.Add(position);

            return 1;
        }
        else
        {
            pathPosition.Remove(position);
        }
        return 0;
    }

    /*
     * Método para comprobar qué pieza está actualmente en la celda.
     */
    public void SetPiece(GameObject piece)
    {
        currentPiece = piece;
    }
}
