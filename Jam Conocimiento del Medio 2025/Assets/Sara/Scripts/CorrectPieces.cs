using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectPieces : MonoBehaviour
{
    public PieceData[] validPieces;

    public bool IsPieceValid(PieceData piece) // Comprueba si la pieza es del tipo que admite la celda.
    {
        foreach (PieceData valid in validPieces)
        {
            if (valid == piece)
            {
                Debug.Log("Pieza correcta en el hueco correcto");
                return true;
            }
        }

        Debug.Log("Pieza incorrecta en el hueco.");
        return false;
    }
}
