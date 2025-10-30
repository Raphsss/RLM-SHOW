using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Variável para guardar o valor do placar
    public int score = 0;

    // Referência ao seu texto UI (Arraste o ScoreDisplay para cá)
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScoreText();
    }

    // Função pública chamada pelos cilindros para adicionar pontos
    public void AddScore(int amount)
    {
        if (amount > 0)
        {
            score += amount;
        }
        UpdateScoreText();
    }

    // Função que atualiza o texto na tela
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("O campo 'Score Text' no ScoreManager está vazio! Arraste seu ScoreDisplay para lá.");
        }
    }
}