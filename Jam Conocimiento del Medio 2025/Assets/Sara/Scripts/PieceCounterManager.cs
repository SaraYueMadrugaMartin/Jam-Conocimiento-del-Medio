using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PieceCounterManager : MonoBehaviour
{
    [System.Serializable]
    public class Counter
    {
        public PieceType pieceType;
        public TextMeshProUGUI text;
        public int count = 0;
    }

    [SerializeField] private List<Counter> counters;

    public void InitializeCounters()
    {
        foreach (Counter c in counters)
        {
            c.count = FindPiecesOfType(c.pieceType);
            UpdateText(c);
        }
    }

    public void AddPiece(PieceType type, int amount = 1)
    {
        Counter c = counters.Find(x => x.pieceType == type);
        if (c != null)
        {
            c.count += amount;
            UpdateText(c);
        }
    }

    public void RemovePiece(PieceType type, int amount = 1)
    {
        Counter c = counters.Find(x => x.pieceType == type);
        if (c != null)
        {
            c.count -= amount;
            if (c.count < 0) c.count = 0;
            UpdateText(c);
        }
    }

    private void UpdateText(Counter c)
    {
        if (c.text != null)
            c.text.text = c.count.ToString();
    }

    private int FindPiecesOfType(PieceType type)
    {
        TypePiece[] allPieces = FindObjectsOfType<TypePiece>();
        int count = 0;
        foreach (TypePiece p in allPieces)
        {
            if (p.pieceData.pieceType == type)
                count++;
        }
        return count;
    }
}