using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Vari�vel para guardar o valor do placar
    public int score = 0;

    // Refer�ncia ao seu texto UI (Arraste o ScoreDisplay para c�)
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScoreText();
    }

    // Fun��o p�blica chamada pelos cilindros para adicionar pontos
    public void AddScore(int amount)
    {
        if (amount > 0)
        {
            score += amount;
        }
        UpdateScoreText();
    }

    // Fun��o que atualiza o texto na tela
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("O campo 'Score Text' no ScoreManager est� vazio! Arraste seu ScoreDisplay para l�.");
        }
    }
}