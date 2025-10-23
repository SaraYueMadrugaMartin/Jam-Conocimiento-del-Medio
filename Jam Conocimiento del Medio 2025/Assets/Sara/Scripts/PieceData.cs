using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Piece/Type")]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public PieceType pieceType;
}

public enum PieceType
{
    Vertical,
    Horizontal,
    EsqUpRight,
    EsqUpLeft,
    EsqDownRight,
    EsqDownLeft,
    Pipa
}