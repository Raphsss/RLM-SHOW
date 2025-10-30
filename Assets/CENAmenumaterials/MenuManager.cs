using UnityEngine;
using UnityEngine.SceneManagement; // ESSA LINHA É ESSENCIAL!

public class MenuManager : MonoBehaviour
{
    // O nome da cena principal do seu quiz (aquela que você estava usando antes)
    [Header("Nome da Cena do Jogo")]
    public string gameSceneName = "SampleScene"; // Mude para o nome da sua cena!

    // Função pública chamada pelo botão para carregar o jogo
    public void StartGame()
    {
        // SceneManager carrega a cena com o nome que você definiu
        SceneManager.LoadScene(gameSceneName);
    }

    // (Opcional) Função para o botão de SAIR do jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo...");
        Application.Quit();
    }
}