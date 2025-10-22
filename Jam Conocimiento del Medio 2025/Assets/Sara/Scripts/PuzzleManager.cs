using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject panelWin;
    public GameObject cellsParent;
    public int scoreToWin = 5;

    private void Start()
    {
        panelWin.SetActive(false);
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
            panelWin.SetActive(true);
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

    public void BackMenu()
    {
        SceneManager.LoadScene("00_Main");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("02_Level02");
    }
}
