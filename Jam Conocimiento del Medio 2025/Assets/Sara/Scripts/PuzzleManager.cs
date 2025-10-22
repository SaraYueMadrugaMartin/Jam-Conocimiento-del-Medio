using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Canvas panelWin;
    public GameObject cellsParent;
    public int scoreToWin = 5;

    private void Start()
    {
        //panelWin = GetComponent<Canvas>();
        panelWin.enabled = false;
    }

    public void CheckPuzzle()
    {
        CorrectPieces[] allCells = cellsParent.GetComponentsInChildren<CorrectPieces>();
        int totalScore = 0;

        foreach (CorrectPieces cell in allCells)
        {
            totalScore += cell.GetScore();
        }

        if (totalScore >= scoreToWin)
            panelWin.enabled = true;
        else
        {
            MovePieces[] allPieces = FindObjectsOfType<MovePieces>();
            foreach (MovePieces piece in allPieces)
            {
                piece.ReturnToStart();
            }

            foreach (CorrectPieces cell in allCells)
            {
                cell.SetPiece(null);
            }

            Debug.Log("Todavía no es correcto. Piezas correctas: " + totalScore);
        }
    }
}
