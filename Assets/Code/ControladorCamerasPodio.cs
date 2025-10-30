using UnityEngine;

public class ControladorCamerasPodio : MonoBehaviour
{
    public Camera camPodio1;
    public Camera camPodio2;

    // Chame esta função passando 1 para vencedor jogador 1, 2 para jogador 2
    public void MostrarCameraVencedor(int vencedor)
    {
        camPodio1.enabled = vencedor == 1;
        camPodio2.enabled = vencedor == 2;
    }
}
