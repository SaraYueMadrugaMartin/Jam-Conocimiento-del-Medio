using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private HamsterController hamsterControl;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;
    [SerializeField] private GameObject cellsParent;
    [SerializeField] private List<PossiblePaths> possiblePaths;

    private void Start()
    {
        panelWin.SetActive(false);
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);
    }

    public void CheckPuzzle()
    {
        Debug.Log("Has pulsado el bot�n de Comprobar");

        CorrectPieces[] allCells = cellsParent.GetComponentsInChildren<CorrectPieces>();
        foreach (CorrectPieces cell in allCells)
            cell.puzzleManager = this;

        PossiblePaths matchedPath = null;

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
                matchedPath = path;
                break;
            }
        }

        if (matchedPath == null || hamsterControl.isMoving)
        {
            Debug.Log("El camino es inv�lido o el hamster se est� moviendo.");
            ResetPuzzle(allCells);
            return;
        }

        int piezasUsadas = matchedPath.pathPositions.Count;
        int minPiezas = possiblePaths.Min(p => p.pathPositions.Count);
        int maxPiezas = possiblePaths.Max(p => p.pathPositions.Count);

        Debug.Log($"Ruta completada con {piezasUsadas} piezas. (min: {minPiezas}, max: {maxPiezas})");

        int estrellas;
        if (piezasUsadas >= maxPiezas)
            estrellas = 3; // Camino m�s largo
        else if (piezasUsadas <= minPiezas)
            estrellas = 1; // Camino m�s corto
        else
            estrellas = 2; // Intermedio

        switch (estrellas)
        {
            case 1:
                hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { FinOneStar(); });
                break;
            case 2:
                hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { FinTwoStar(); });
                break;
            case 3:
                hamsterControl.StartPath(new List<RectTransform>(CorrectPieces.pathPosition), () => { FinThreeStar(); });
                break;
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

    public void FinOneStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
    }

    public void FinTwoStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
    }

    public void FinThreeStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
        threeStar.SetActive(true);
    }

    #region M�todos Interfaz
    public void BackMenu()
    {
        SceneManager.LoadScene("00_Main");
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    #endregion
}