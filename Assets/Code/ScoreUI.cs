using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public Text scoreP1Text;
    public Text scoreP2Text;
    public TMP_Text scoreP1Tmp;
    public TMP_Text scoreP2Tmp;
    public GameObject highlightP1;
    public GameObject highlightP2;

    void Awake()
    {
        // tenta auto-encontrar os objetos pelo nome
        var go1 = GameObject.Find("ScoreP1");
        var go2 = GameObject.Find("ScoreP2");

        if (scoreP1Text == null && go1 != null)
            scoreP1Text = go1.GetComponent<Text>();
        if (scoreP1Tmp == null && go1 != null)
            scoreP1Tmp = go1.GetComponent<TMP_Text>();

        if (scoreP2Text == null && go2 != null)
            scoreP2Text = go2.GetComponent<Text>();
        if (scoreP2Tmp == null && go2 != null)
            scoreP2Tmp = go2.GetComponent<TMP_Text>();
    }

    void Start()
    {
        // inicializa exibição (caso valores iniciais sejam 0)
        OnScoreChanged(0, 0);
        OnPlayerSelected(0);
    }

    public void OnScoreChanged(int p1, int p2)
    {
        // atualiza os dois tipos (Text e TMP) caso existam
        if (scoreP1Text != null) scoreP1Text.text = "Jogador01: " + p1;
        if (scoreP1Tmp != null) scoreP1Tmp.text = "Jogador01: " + p1;

        if (scoreP2Text != null) scoreP2Text.text = "Jogador02: " + p2;
        if (scoreP2Tmp != null) scoreP2Tmp.text = "Jogador02: " + p2;

        Debug.Log($"ScoreUI atualizado -> P1:{p1} P2:{p2}");
    }

    public void OnPlayerSelected(int player)
    {
        if (highlightP1 != null) highlightP1.SetActive(player == 1);
        if (highlightP2 != null) highlightP2.SetActive(player == 2);
    }
}
