using UnityEngine;

public class GameController : MonoBehaviour
{
    void Update()
    {
        // Verifica se o bot�o esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            // Cria um raio a partir da c�mera na posi��o do clique
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Se o raio atingir um objeto com Collider:
            if (Physics.Raycast(ray, out hit))
            {
                // Tenta encontrar o script InteractableAnswer no objeto atingido
                InteractableAnswer answerScript = hit.transform.GetComponent<InteractableAnswer>();

                if (answerScript != null)
                {
                    // Chama a fun��o de clique no script do cilindro
                    answerScript.OnClick();
                }
            }
        }
    }
}