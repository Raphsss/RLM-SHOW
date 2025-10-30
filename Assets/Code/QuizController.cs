using UnityEngine;
using System.Collections.Generic;

public class QuizController : MonoBehaviour
{
    public List<GameObject> questionGroups = new List<GameObject>();

    [Header("Configuração de Movimento")]
    public float moveSpeed = 3f;

    private int currentQuestionIndex = 0;
    private Vector3 targetContainerPosition;
    private bool isMoving = false;
    private const int totalQuestions = 5;

    // NOVO: Identifica o jogador da rodada atual
    [Header("Jogador Atual")]
    public int currentPlayer = 0;

    // NOVO: Referência ao ArduinoInputManager para conceder pontos
    [Header("Referências externas")]
    public ArduinoInputManager arduinoManager;

    void Start()
    {
        if (questionGroups.Count == 0)
        {
            Debug.LogError("QuizController precisa da lista de Perguntas preenchida.");
            return;
        }
        targetContainerPosition = transform.position;

        // Toca o áudio da primeira pergunta ao iniciar
        PlayCurrentQuestionAudio();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetContainerPosition,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(transform.position, targetContainerPosition) < 0.05f)
            {
                transform.position = targetContainerPosition;
                isMoving = false;
                // Toca o áudio da pergunta em destaque
                PlayCurrentQuestionAudio();
            }
        }
    }

    public void AdvanceQuestion()
    {
        if (isMoving || currentQuestionIndex >= totalQuestions - 1)
        {
            if (currentQuestionIndex >= totalQuestions - 1)
            {
                Debug.Log("Quiz Finalizado!");
                // Aqui pode chamar painel de resultados, som de fim, etc.
            }
            return;
        }

        currentQuestionIndex++;

        float nextQuestionLocalX = questionGroups[currentQuestionIndex].transform.localPosition.x;
        targetContainerPosition = new Vector3(
            -nextQuestionLocalX,
            transform.position.y,
            transform.position.z
        );

        isMoving = true;

        // Ao avançar, reabilita/limpa o estado das respostas do novo grupo
        ResetAnswersState();
    }

    // NOVO: Permite ao Arduino determinar quem é o player da rodada
    public void SetCurrentPlayer(int playerID)
    {
        currentPlayer = playerID;
        Debug.Log("Jogador selecionado para responder: " + currentPlayer);
    }

    private void PlayCurrentQuestionAudio()
    {
        if (currentQuestionIndex < questionGroups.Count)
        {
            QuestionAudio qa = questionGroups[currentQuestionIndex].GetComponent<QuestionAudio>();
            if (qa != null)
            {
                qa.PlayQuestionAudio();
            }
        }
    }

    // NOVO: chamada quando a resposta correta foi selecionada
    public void OnCorrectAnswer(int pointsForThisQuestion = 1)
    {
        // Concede pontos para quem apertou primeiro
        if (arduinoManager != null)
        {
            arduinoManager.AwardPointsToCurrentPlayer(pointsForThisQuestion);
            // envia reset para Arduino e libera disputa
            arduinoManager.NextRoundReset();
        }
        else
        {
            Debug.LogWarning("arduinoManager não atribuído em QuizController.");
        }

        // Avança para próxima pergunta e reabilita respostas
        AdvanceQuestion();
    }

    // NOVO: reabilita colisores e reseta visual das respostas do grupo atual
    private void ResetAnswersState()
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= questionGroups.Count) return;

        var answers = questionGroups[currentQuestionIndex].GetComponentsInChildren<InteractableAnswer>(true);
        foreach (var a in answers)
        {
            a.ResetState(); // chama o método que será adicionado em InteractableAnswer
        }
    }
}
