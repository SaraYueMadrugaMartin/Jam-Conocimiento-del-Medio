using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private HamsterController hamsterControl;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelMedio;
    [SerializeField] private GameObject cellsParent;
    [SerializeField] private List<PossiblePaths> possiblePaths;

    private void Start()
    {
        panelWin.SetActive(false);
        panelMedio.SetActive(false);
    }

    public void CheckPuzzle()
    {
        Debug.Log("Has pulsado el bot�n de Comprobar");

        CorrectPieces[] allCells = cellsParent.GetComponentsInChildren<CorrectPieces>();
        bool rutaCompleta = false;

        foreach (CorrectPieces cell in allCells)
            cell.puzzleManager = this;

        foreach (PossiblePaths path in possiblePaths)
        {
            if (CorrectPieces.pathPosition.Count != path.pathPositions.Count)
                continue;

            bool match = true;

            for (int i = 0; i < path.pathPositions.Count; i++)
            {
                if (CorrectPieces.pathPosition[i] != path.pathPositions[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                rutaCompleta = true;
                break;
            }
        }

        if (rutaCompleta && !hamsterControl.isMoving)
        {
            if(CorrectPieces.pathPosition.Count <= 10) // TODO: Hay que cambiar el sprite en este momento a hamster m�s delgado.
                hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { panelMedio.SetActive(true); }); // TODO: Aqu� ir�a la animaci�n del hamster explotando.
            else if(CorrectPieces.pathPosition.Count >= 11)
                hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { panelWin.SetActive(true); }); // TODO: Llamar a funci�n de FinishAnimation.
        }
        else
        {
            Debug.Log("El camino es inv�lido.");
            ResetPuzzle(allCells);
        }
    }

    private void ResetPuzzle(CorrectPieces[] allCells)
    {
        MovePieces[] allPieces = FindObjectsOfType<MovePieces>();

        foreach (MovePieces piece in allPieces)
            piece.ReturnToStart();

        foreach (CorrectPieces cell in allCells)
            cell.SetPiece(null);

        CorrectPieces.pathPosition.Clear();
    }

    public void FinishAnimation()
    {
        // TODO: A�adir la animaci�n del hamster comiendo las pipas y llen�ndose.
    }

    #region M�todos Interfaz
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
