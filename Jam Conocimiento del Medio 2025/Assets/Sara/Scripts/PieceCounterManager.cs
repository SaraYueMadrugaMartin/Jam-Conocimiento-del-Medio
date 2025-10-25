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
        public int countPieces = 0;
    }

    [SerializeField] private List<Counter> counters;
    private Dictionary<PieceType, int> initialCounts = new Dictionary<PieceType, int>();

    public void InitializeCounters()
    {
        foreach (Counter c in counters)
        {
            c.countPieces = FindPiecesOfType(c.pieceType);
            UpdateText(c);
        }

        SaveInitialCounts();
    }

    public void AddPiece(PieceType type, int amount = 1)
    {
        Counter c = counters.Find(x => x.pieceType == type);

        if (c != null)
        {
            c.countPieces += amount;
            UpdateText(c);
        }
    }

    public void RemovePiece(PieceType type, int amount = 1)
    {
        Counter c = counters.Find(x => x.pieceType == type);

        if (c != null)
        {
            c.countPieces -= amount;
            if (c.countPieces < 0) c.countPieces = 0;
            UpdateText(c);
        }
    }

    private void UpdateText(Counter c)
    {
        if (c.text != null)
            c.text.text = c.countPieces.ToString();
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

    public void SaveInitialCounts()
    {
        initialCounts.Clear();

        foreach (Counter c in counters)
            initialCounts[c.pieceType] = c.countPieces;
    }

    public void ResetToInitialCounts()
    {
        foreach (Counter c in counters)
        {
            if (initialCounts.ContainsKey(c.pieceType))
                c.countPieces = initialCounts[c.pieceType];

            UpdateText(c);
        }
    }
}