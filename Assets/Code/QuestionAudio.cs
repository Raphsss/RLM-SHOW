using UnityEngine;

// Anexe este script a cada objeto QxPERGUNTA (Q1PERGUNTA, Q2PERGUNTA, etc.).
public class QuestionAudio : MonoBehaviour
{
    // Campo para você arrastar o arquivo de áudio da narração (ex: Pergunta1.wav)
    public AudioClip questionClip;

    // Referência ao componente que toca o som, que será adicionado ao objeto.
    private AudioSource audioSource;

    void Start()
    {
        // Garante que o objeto QxPERGUNTA tenha um AudioSource.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Se não tiver, ele adiciona um AudioSource.
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurações básicas do AudioSource.
        audioSource.volume = 1f;
        audioSource.playOnAwake = false; // Não toca ao iniciar.

        // Atribui o clipe de áudio que foi arrastado no Inspector.
        audioSource.clip = questionClip;
    }

    // Função pública que será chamada pelo QuizController para iniciar a narração.
    public void PlayQuestionAudio()
    {
        // Toca o áudio se ele existir e se não estiver tocando.
        if (questionClip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}