using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PossiblePaths
{
    public string name;
    public List<RectTransform> pathPositions; // Lista para añadir todos los caminos posibles en cada nivel.
}