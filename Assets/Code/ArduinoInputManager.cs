using UnityEngine;
using System.IO.Ports;
using UnityEngine.Events;

public class ArduinoInputManager : MonoBehaviour
{
    [Header("Serial")]
    public string portName = "COM5";
    public int baudRate = 9600;

    [Header("Referências")]
    public QuizController quizController;   

    public Animator player1Animator;
    public Animator player2Animator;

    public Animator cleitonAnimator;
    [Header("Animator Trigger Names")]
    public string player1TriggerName = "P1"; // nome exato do trigger no Animator do player 1
    public string player2TriggerName = "P2"; // nome exato do trigger no Animator do player 2

    [Header("Eventos")]
    public UnityEvent<int> OnPlayerSelected;       // invocado com (1) ou (2)
    public UnityEvent<int,int> OnScoreChanged;     // invocado com (scoreP1, scoreP2)
    public UnityEvent OnArduinoReady;              // opcional: quando Arduino envia "READY"

    [Header("Estado do Jogo (somente leitura durante execução)")]
    [SerializeField] private int currentPlayer = 0; // 0 = nenhum, 1 ou 2
    [SerializeField] private bool roundLocked = false;

    [Header("Placar")]
    public int scoreP1 = 0;
    public int scoreP2 = 0;

    // NOVO: referência direta ao ScoreUI (opcional — será encontrada automaticamente se vazio)
    public ScoreUI scoreUI;

    [Header("Testes")]
    public bool allowKeyboardTesting = true;
    public KeyCode keyPlayer1 = KeyCode.Alpha1; // tecla para simular P1
    public KeyCode keyPlayer2 = KeyCode.Alpha2; // tecla para simular P2
    public KeyCode keyCorrect = KeyCode.C;      // confirma resposta correta (avança e pontua)
    public KeyCode keyReset = KeyCode.R;        // força reset da rodada

    private SerialPort serial;

    void Start()
    {
        // auto-find ScoreUI se não atribuído
        if (scoreUI == null)
        {
            scoreUI = FindObjectOfType<ScoreUI>();
        }

        try
        {
            serial = new SerialPort(portName, baudRate);
            serial.NewLine = "\n";     // combina com println do Arduino
            serial.ReadTimeout = 50;   // evita bloqueio indefinido
            serial.Open();
            Debug.Log($"[Serial] Aberta: {portName}");

            // envia placar inicial para UI e limpa highlight
            OnScoreChanged?.Invoke(scoreP1, scoreP2);
            if (scoreUI != null) scoreUI.OnScoreChanged(scoreP1, scoreP2);

            // limpa highlight inicial (0 = nenhum)
            OnPlayerSelected?.Invoke(0);
            if (scoreUI != null) scoreUI.OnPlayerSelected(0);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Serial] Falha ao abrir {portName}: {e.Message}");
            // mesmo sem serial, inicializa UI para testes no Editor
            OnScoreChanged?.Invoke(scoreP1, scoreP2);
            if (scoreUI != null) scoreUI.OnScoreChanged(scoreP1, scoreP2);
            OnPlayerSelected?.Invoke(0);
            if (scoreUI != null) scoreUI.OnPlayerSelected(0);
        }
    }

    void Update()
    {
        // teclado para testes — funciona mesmo sem Arduino ou com ele conectado
        if (allowKeyboardTesting)
        {
            // se rodada não estiver travada, permite simular quem apertou primeiro
            if (!roundLocked)
            {
                if (Input.GetKeyDown(keyPlayer1)) SetRoundWinner(1);
                if (Input.GetKeyDown(keyPlayer2)) SetRoundWinner(2);
            }
            else
            {
                // quando rodada já está travada, permitir confirmar acerto ou reset manual
                if (Input.GetKeyDown(keyCorrect))
                {
                    // preferir que o QuizController trate lógica de pontuação/advance
                    if (quizController != null)
                    {
                        quizController.OnCorrectAnswer(1);
                    }
                    else
                    {
                        // fallback: dar pontos direto e resetar
                        AwardPointsToCurrentPlayer(1);
                        NextRoundReset();
                    }
                }

                if (Input.GetKeyDown(keyReset))
                {
                    ResetRound();
                }
            }
        }

        if (serial == null || !serial.IsOpen) return;

        if (roundLocked)
        {
            // Limpa buffer mas ignora novas tentativas até reset
            try { serial.ReadExisting(); } catch { }
            return;
        }

        try
        {
            string line = serial.ReadLine().Trim();

            if (line == "P1")
            {
                SetRoundWinner(1);
            }
            else if (line == "P2")
            {
                SetRoundWinner(2);
            }
            else if (line == "READY")
            {
                OnArduinoReady?.Invoke();
                Debug.Log("Arduino pronto para próxima rodada");
            }
            else
            {
                Debug.Log($"[Serial] Ignorado: {line}");
            }
        }
        catch (System.TimeoutException)
        {
            // Sem linha completa no intervalo: normal
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[Serial] Erro leitura: {e.Message}");
        }
    }

    private void SetRoundWinner(int player)
    {
        roundLocked = true;
        currentPlayer = player;
        Debug.Log($"Primeiro: P{player}");
        OnPlayerSelected?.Invoke(player);
        if (scoreUI != null) scoreUI.OnPlayerSelected(player);

        // Notifica QuizController se existir
        if (quizController != null)
        {
            try { quizController.SetCurrentPlayer(player); }
            catch { /* método pode não existir: proteger para evitar erro */ }
        }

        // DISPARA TRIGGER NO ANIMATOR CORRETO
        if (player == 1)
        {
            if (player1Animator != null) player1Animator.SetTrigger(player1TriggerName);
            else if (cleitonAnimator != null) cleitonAnimator.SetTrigger(player1TriggerName);
        }
        else if (player == 2)
        {
            if (player2Animator != null) player2Animator.SetTrigger(player2TriggerName);
            else if (cleitonAnimator != null) cleitonAnimator.SetTrigger(player2TriggerName);
        }
    }

    // Dá pontos para o player atual da rodada (quem apertou primeiro)
    public void AwardPointsToCurrentPlayer(int points)
    {
        if (currentPlayer == 1) AddPointsToPlayer(1, points);
        else if (currentPlayer == 2) AddPointsToPlayer(2, points);
        else Debug.LogWarning("Nenhum player selecionado para receber pontos.");
    }

    // Dá pontos explicitamente para um player (1 ou 2)
    public void AddPointsToPlayer(int player, int points)
    {
        if (player == 1)
        {
            scoreP1 += points;
        }
        else if (player == 2)
        {
            scoreP2 += points;
        }
        else
        {
            Debug.LogWarning($"Player inválido: {player}");
            return;
        }

        Debug.Log($"Pontos: P1={scoreP1} P2={scoreP2}");

        // notifica listeners via UnityEvent
        OnScoreChanged?.Invoke(scoreP1, scoreP2);

        // atualiza ScoreUI diretamente se encontrado
        if (scoreUI != null) scoreUI.OnScoreChanged(scoreP1, scoreP2);
    }

    // Reseta apenas a rodada (libera disputas e limpa player atual)
    public void ResetRound()
    {
        currentPlayer = 0;
        roundLocked = false;
        // informa Arduino que pode aceitar novos toques
        if (serial != null && serial.IsOpen)
        {
            try { serial.WriteLine("RESET"); }
            catch (System.Exception e) { Debug.LogWarning($"[Serial] Erro ao enviar RESET: {e.Message}"); }
        }

        // limpa highlight na UI
        OnPlayerSelected?.Invoke(0);
        if (scoreUI != null) scoreUI.OnPlayerSelected(0);

        Debug.Log("Rodada resetada, aguardando próxima disputa.");
    }

    // Reseta placar dos 2 players
    public void ResetScores()
    {
        scoreP1 = 0;
        scoreP2 = 0;
        OnScoreChanged?.Invoke(scoreP1, scoreP2);
        if (scoreUI != null) scoreUI.OnScoreChanged(scoreP1, scoreP2);
        Debug.Log("Placar resetado.");
    }

    // Método legado mantido (compatibilidade) — chama ResetRound
    public void NextRoundReset()
    {
        ResetRound();
    }

    void OnDisable()
    {
        if (serial != null && serial.IsOpen) serial.Close();
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen) serial.Close();
    }
}
