using UnityEngine;

public class GameController : MonoBehaviour
{
    void Update()
    {
        // Verifica se o botão esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            // Cria um raio a partir da câmera na posição do clique
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Se o raio atingir um objeto com Collider:
            if (Physics.Raycast(ray, out hit))
            {
                // Tenta encontrar o script InteractableAnswer no objeto atingido
                InteractableAnswer answerScript = hit.transform.GetComponent<InteractableAnswer>();

                if (answerScript != null)
                {
                    // Chama a função de clique no script do cilindro
                    answerScript.OnClick();
                }
            }
        }
    }
}