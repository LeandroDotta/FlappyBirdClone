using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject canvasOpenning;
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
        textCurrentScore = canvasInGame.transform.FindChild("CurrentScore").GetComponent<Text>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnChangeState += OnChangeState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnChangeState -= OnChangeState;
    }

    /// <summary>
    /// Atualiza a GUI de acordo com o estado do jogo
    /// </summary>
    /// <param name="state">Estado atual do jogo.</param>
    private void OnChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.Opening:
                // Exibe a interface inicial
                canvasOpenning.SetActive(true);
                canvasInGame.SetActive(false);
                canvasGameOver.SetActive(false);
                break;

            case GameState.PreGame:
				// Exibe os objetos de UI com as instruções do jogo
				ShowInstructions();

				// Exibe a interface dentro do jogo (pontuação e instruções)
				canvasOpenning.SetActive(false);
                canvasGameOver.SetActive(false);
				canvasInGame.SetActive(true);
                break;

            case GameState.InGame:
                // Quando o pássaro iniciar a movimentação, siginifica que o jogo iniciou
                // portanto, as intruções são removidas da tela
				HideInstructions();
                break;

            case GameState.GameOver:
                // Exibe a tela de "GameOver"
                canvasInGame.SetActive(false);
                canvasGameOver.SetActive(true);

                // Atualiza a pontuação no painel de game over
                textFinalScore.text = GameManager.Instance.CurrentScore.ToString();
                textHighScore.text = GameManager.Instance.HighScore.ToString();

                // Atualiza a medalha de acordo com sua pontuação
                medalScript.SetMedal(GameManager.Instance.CurrentScore);
                break;
        }
    }

    public void SetScoreText(string score)
    {
        textCurrentScore.text = score;
    }

	private void ShowInstructions(){
		canvasInGame.GetComponent<Animator>().enabled = false;

		imgInstructions.gameObject.SetActive(true);
		imgInstructions.color = new Color(imgInstructions.color.r, imgInstructions.color.g, imgInstructions.color.b, 1);
		Color textColor = textGetReady.color;
		Color textShadowColor = textGetReady.GetComponent<Shadow>().effectColor;
		Color textOutlineColor1 = textGetReady.GetComponents<Outline>()[0].effectColor;
		Color textOutlineColor2 = textGetReady.GetComponents<Outline>()[1].effectColor;

		textColor.a = 1;
		textShadowColor.a = 1;
		textOutlineColor1.a = 1;
		textOutlineColor2.a = 1;

		textGetReady.gameObject.SetActive(true);
		textGetReady.color = textColor;
		textGetReady.GetComponent<Shadow>().effectColor = textShadowColor;
		textGetReady.GetComponents<Outline>()[0].effectColor = textOutlineColor1;
		textGetReady.GetComponents<Outline>()[1].effectColor = textOutlineColor2;
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
