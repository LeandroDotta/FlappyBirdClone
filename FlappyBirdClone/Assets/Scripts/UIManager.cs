using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject canvasInGame;
    public GameObject canvasGameOver;

    public Text textFinalScore;
    public Text textHighScore;
    public Text textGetReady;
    public Image imgInstructions;

    public Medal medalScript;

    private Text textCurrentScore;

    private void Start()
    {
        textCurrentScore = canvasInGame.transform.Find("CurrentScore").GetComponent<Text>();
    }

    private void OnEnable()
    {
        GameManager.OnUpdateCurrentScore += SetScoreText;
        Bird.OnStartFlyging += HideInstructions;
        Bird.OnFall += ShowGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnUpdateCurrentScore -= SetScoreText;
        Bird.OnStartFlyging -= HideInstructions;
        Bird.OnFall -= ShowGameOver;
    }

    public void SetScoreText(int score)
    {
        textCurrentScore.text = score.ToString();
    }

    public void ShowGameOver()
    {
        canvasInGame.SetActive(false);
        canvasGameOver.SetActive(true);

        // Atualiza a pontuação no painel de game over
        textFinalScore.text = GameManager.Instance.CurrentScore.ToString();
        textHighScore.text = GameManager.Instance.HighScore.ToString();

        // Atualiza a medalha de acordo com sua pontuação
        medalScript.SetMedal(GameManager.Instance.CurrentScore);
    }

	/// <summary>
	/// Habilita a animação que foi criada no editor para esconder os objetos de instruções 
	/// criadas na UI. A animação inicia desativada por padrão, e quando é habilitada, remove
	/// as instruções com um efeito de "fade out".
	/// </summary>
	private void HideInstructions(){
		canvasInGame.GetComponent<Animator>().enabled = true;
	}
}
