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
        SFXManager.Instance.PlaySFX("ButtonPress");
        Debug.Log("Has pulsado el botón de Comprobar");

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
            Debug.Log("El camino es inválido o el hamster se está moviendo.");
            ResetPuzzle(allCells);
            return;
        }

        int piezasUsadas = matchedPath.pathPositions.Count;
        int minPiezas = possiblePaths.Min(p => p.pathPositions.Count);
        int maxPiezas = possiblePaths.Max(p => p.pathPositions.Count);

        Debug.Log($"Ruta completada con {piezasUsadas} piezas. (min: {minPiezas}, max: {maxPiezas})");

        int estrellas;
        if (piezasUsadas >= maxPiezas)
            estrellas = 3;
        else if (piezasUsadas <= minPiezas)
            estrellas = 1;
        else
            estrellas = 2;

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
        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();

        foreach (MovePieces piece in allPieces)
        {
            piece.ReturnToStart();

            TypePiece pieceScript = piece.GetComponent<TypePiece>();
            if (pieceScript != null)
            {
                counterManager?.AddPiece(pieceScript.pieceData.pieceType);
            }
        }

        foreach (CorrectPieces cell in allCells)
            cell.SetPiece(null);

        CorrectPieces.pathPosition.Clear();
    }

    public void FinOneStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    public void FinTwoStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    public void FinThreeStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
        threeStar.SetActive(true);
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    #region Métodos Interfaz
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