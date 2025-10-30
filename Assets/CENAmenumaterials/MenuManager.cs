using UnityEngine;
using UnityEngine.SceneManagement; // ESSA LINHA � ESSENCIAL!

public class MenuManager : MonoBehaviour
{
    // O nome da cena principal do seu quiz (aquela que voc� estava usando antes)
    [Header("Nome da Cena do Jogo")]
    public string gameSceneName = "SampleScene"; // Mude para o nome da sua cena!

    // Fun��o p�blica chamada pelo bot�o para carregar o jogo
    public void StartGame()
    {
        // SceneManager carrega a cena com o nome que voc� definiu
        SceneManager.LoadScene(gameSceneName);
    }

    // (Opcional) Fun��o para o bot�o de SAIR do jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo...");
        Application.Quit();
    }
}