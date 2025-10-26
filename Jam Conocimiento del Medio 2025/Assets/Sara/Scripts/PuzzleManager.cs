using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private HamsterController hamsterControl;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelPopUp;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;
    [SerializeField] private GameObject cellsParent;
    [SerializeField] private List<PossiblePaths> possiblePaths;
    [SerializeField] private RectTransform endPoint;
    [SerializeField] private Animator animPanelFin;
    [SerializeField] private Animator animPanelExplot;

    private void Start()
    {
        panelWin.SetActive(false);
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);
        panelPopUp.SetActive(false);

        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();
        counterManager.InitializeCounters();
    }

    public void CheckPuzzle()
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        CorrectPieces[] allCells = cellsParent.GetComponentsInChildren<CorrectPieces>();

        foreach (CorrectPieces cell in allCells)
            cell.puzzleManager = this;

        PossiblePaths matchedPath = null;

        foreach (PossiblePaths path in possiblePaths)
        {
            bool match = path.pathPositions.All(pos =>
            {
                CorrectPieces cellComp = pos.GetComponent<CorrectPieces>();
                return cellComp != null && cellComp.currentPiece != null && cellComp.IsPieceValid(cellComp.currentPiece.GetComponent<TypePiece>().pieceData);
            });

            if (match)
            {
                matchedPath = path;
                break;
            }
        }

        if (matchedPath == null || hamsterControl.isMoving)
        {
            StartCoroutine(ShowPopUpPanel());
            ResetPuzzle(allCells);
            return;
        }

        int piezasUsadas = matchedPath.pathPositions.Count;
        int minPiezas = possiblePaths.Min(p => p.pathPositions.Count);
        int maxPiezas = possiblePaths.Max(p => p.pathPositions.Count);

        int estrellas;
        if (piezasUsadas >= maxPiezas)
            estrellas = 3;
        else if (piezasUsadas <= minPiezas)
            estrellas = 1;
        else
            estrellas = 2;

        List<RectTransform> pathToFollow = new List<RectTransform>(matchedPath.pathPositions);

        if (endPoint != null)
            pathToFollow.Add(endPoint);

        switch (estrellas)
        {
            case 1:
                hamsterControl.StartPath(pathToFollow, () => { FinOneStar(); });
                break;
            case 2:
                hamsterControl.StartPath(pathToFollow, () => { FinTwoStar(); });
                break;
            case 3:
                hamsterControl.StartPath(pathToFollow, () => { FinThreeStar(); });
                break;
        }
    }

    private IEnumerator ShowPopUpPanel()
    {
        panelPopUp.SetActive(true);

        if (animPanelExplot != null)
            animPanelExplot.Play("ExplotGordo");

        SFXManager.Instance.PlaySFX("Error");
        yield return new WaitForSeconds(5f);
        panelPopUp.SetActive(false);
    }

    private void ResetPuzzle(CorrectPieces[] allCells)
    {
        MovePieces[] allPieces = FindObjectsOfType<MovePieces>();
        PieceCounterManager counterManager = FindObjectOfType<PieceCounterManager>();

        foreach (MovePieces piece in allPieces)
            piece.ReturnToStart();

        counterManager.ResetToInitialCounts();

        foreach (CorrectPieces cell in allCells)
            cell.SetPiece(null);

        CorrectPieces.pathPosition.Clear();
    }

    public void FinOneStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        animPanelFin.Play("ComerDesnutrido");
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    public void FinTwoStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
        animPanelFin.Play("ComerDesnutrido");
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    public void FinThreeStar()
    {
        panelWin.SetActive(true);
        oneStar.SetActive(true);
        twoStar.SetActive(true);
        threeStar.SetActive(true);
        animPanelFin.Play("ComerDesnutrido");
        SFXManager.Instance.PlaySFX("HamsterWin", false);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("00_Main");
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}