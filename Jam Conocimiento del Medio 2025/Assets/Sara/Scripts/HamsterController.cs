using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool isMoving = false;
    [SerializeField] private Animator animCharacter;

    private void Start()
    {
        animCharacter = GetComponent<Animator>();
    }

    /*
     * Se llama cuando se pulsa el botón "Comprobar".
     * Solo se realiza si no se está moviendo el personaje y si hay un camino guardado.
     */
    public void StartPath(List<RectTransform> path, System.Action onComplete = null)
    {
        if (!isMoving && path != null && path.Count > 0)
            StartCoroutine(MoveAlongPath(path, onComplete));
    }

    /*
     * Corrutina para mover y reproducir las animaciones del personaje.
     */
    private IEnumerator MoveAlongPath(List<RectTransform> path, System.Action onComplete)
    {
        isMoving = true;
        animCharacter.SetBool("Walking", true);

        foreach (RectTransform point in path) // Recorre cada pieza del camino.
        {
            Vector3 target = point.position; // Guarda las coordenadas de las piezas.

            while (Mathf.Abs(transform.position.x - target.x) > 0.05f) // Mueve al personaje en X, derecha e izquierda. (Para las animaciones).
            {
                Vector3 direction = new Vector3(Mathf.Sign(target.x - transform.position.x), 0, 0);
                animCharacter.SetFloat("MoveX", direction.x);
                animCharacter.SetFloat("MoveY", 0);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, transform.position.z),moveSpeed * Time.deltaTime);
                yield return null;
            }

            while (Mathf.Abs(transform.position.y - target.y) > 0.05f) // Mueve al personaje en Y, arriba y abajo. (Para las animaciones).
            {
                Vector3 direction = new Vector3(0, Mathf.Sign(target.y - transform.position.y), 0);
                animCharacter.SetFloat("MoveX", 0);
                animCharacter.SetFloat("MoveY", direction.y);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.y, transform.position.z), moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        animCharacter.SetBool("Walking", false);
        isMoving = false;
        CorrectPieces.pathPosition.Clear();
        onComplete?.Invoke();
    }
}
