using UnityEngine;

// Define se a resposta é correta ou errada
public enum AnswerType { Correct, Wrong }

public class InteractableAnswer : MonoBehaviour
{
    // Defina no Inspector: Correct ou Wrong
    public AnswerType answerType;

    // NOVOS CAMPOS: Onde você arrastará os objetos e clipes
    [Header("Configuração de áudio")]
    public AudioSource audioPlayer;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    // Quantos pontos esta resposta vale (configurável)
    [Header("Pontuação")]
    public int pointsForCorrect = 1;

    private QuizController quizController;
    private bool wasClicked = false;

    void Start()
    {
        // Encontra o QuizController na cena
        quizController = FindObjectOfType<QuizController>();
        // O campo 'audioPlayer' será preenchido arrastando no Inspector, não por código.
    }

    public void OnClick()
    {
        if (wasClicked)
        {
            return;
        }

        wasClicked = true;

        // Desativa todos os colisores das respostas irmãs para evitar múltiplos cliques
        var siblings = transform.parent.GetComponentsInChildren<InteractableAnswer>();
        foreach (var s in siblings)
        {
            var col = s.GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }

        // Feedback visual imediato
        var rend = GetComponent<Renderer>();
        if (answerType == AnswerType.Correct)
        {
            if (rend != null) rend.material.color = Color.green;

            // Toca o som de Acerto
            if (audioPlayer != null && correctSound != null)
            {
                audioPlayer.PlayOneShot(correctSound);
            }
        }
        else
        {
            if (rend != null) rend.material.color = Color.red;

            // Toca o som de Erro
            if (audioPlayer != null && wrongSound != null)
            {
                audioPlayer.PlayOneShot(wrongSound);
            }
        }

        // Espera 0.5 segundo (meio segundo) para o som tocar antes de processar resultado
        Invoke("CallAdvanceQuestion", 0.5f);
    }

    // Função que processa o resultado após o delay
    private void CallAdvanceQuestion()
    {
        if (quizController == null)
        {
            Debug.LogWarning("QuizController não encontrado na cena.");
            return;
        }

        if (answerType == AnswerType.Correct)
        {
            // Chama o método que concede pontos ao player que apertou primeiro
            quizController.OnCorrectAnswer(pointsForCorrect);
        }
        else
        {
            // Em caso de errado apenas avança sem pontuar
            quizController.AdvanceQuestion();
        }
    }

    // NOVO: reseta o estado do interactable para a próxima rodada
    public void ResetState()
    {
        wasClicked = false;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Ajuste conforme seu material inicial (aqui usa branco)
            rend.material.color = Color.white;
        }
    }
}