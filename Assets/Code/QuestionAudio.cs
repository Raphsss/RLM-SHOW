using UnityEngine;

// Anexe este script a cada objeto QxPERGUNTA (Q1PERGUNTA, Q2PERGUNTA, etc.).
public class QuestionAudio : MonoBehaviour
{
    // Campo para voc� arrastar o arquivo de �udio da narra��o (ex: Pergunta1.wav)
    public AudioClip questionClip;

    // Refer�ncia ao componente que toca o som, que ser� adicionado ao objeto.
    private AudioSource audioSource;

    void Start()
    {
        // Garante que o objeto QxPERGUNTA tenha um AudioSource.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Se n�o tiver, ele adiciona um AudioSource.
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configura��es b�sicas do AudioSource.
        audioSource.volume = 1f;
        audioSource.playOnAwake = false; // N�o toca ao iniciar.

        // Atribui o clipe de �udio que foi arrastado no Inspector.
        audioSource.clip = questionClip;
    }

    // Fun��o p�blica que ser� chamada pelo QuizController para iniciar a narra��o.
    public void PlayQuestionAudio()
    {
        // Toca o �udio se ele existir e se n�o estiver tocando.
        if (questionClip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}