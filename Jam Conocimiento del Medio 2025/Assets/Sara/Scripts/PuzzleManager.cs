using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private HamsterController hamsterControl;
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
            totalScore += cell.GetScore();

        if (!hamsterControl.isMoving && totalScore >= scoreToWin)
            hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { panelWin.SetActive(true); });
        else
        {
            MovePieces[] allPieces = FindObjectsOfType<MovePieces>();

            foreach (MovePieces piece in allPieces)
                piece.ReturnToStart();

            foreach (CorrectPieces cell in allCells)
                cell.SetPiece(null);

            CorrectPieces.pathPosition.Clear();
        }
    }

    #region Métodos Interfaz
    public void BackMenu()
    {
        SceneManager.LoadScene("00_Main");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("02_Level02");
    }
    #endregion
}
