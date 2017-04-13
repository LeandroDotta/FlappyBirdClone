using UnityEngine;

public enum GameState
{
    Opening,
    PreGame,
    InGame,
    GameOver
}

public class GameManager : MonoBehaviour {
    public UIManager uiManager;

    public Fader flash;
    public Fader restartFade;

    public Sprite[] backgrounds;
    private SpriteRenderer backgroundRenderer;

    // PROPRIEDADES
    public static GameManager Instance { get; private set; }
    private int _currentScore;
    public int CurrentScore
    {
        get
        {
            return _currentScore;
        }
        set
        {
            _currentScore = value;

            if(uiManager.gameObject.activeSelf)
                uiManager.SetScoreText(_currentScore.ToString());
        }
    }

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt("HighScore", 0);
        }
        private set
        {
            PlayerPrefs.SetInt("HighScore", value);
        }
    }

    private GameState _state;
    public GameState State
    {
        get
        {
            return _state;
        }
        private set
        {
            _state = value;

            if (OnChangeState != null)
                OnChangeState.Invoke(value);
        }
    }
    public Bounds ScreenBounds { get; private set; }

    // EVENTOS
    public delegate void ChangeStateAction(GameState state);
    public event ChangeStateAction OnChangeState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        State = GameState.Opening;

        // Armazena os limites da câmera
        ScreenBounds = Camera.main.OrthographicBounds();

        // Atualiza a imagem de fundo
        backgroundRenderer = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        SetBackground();

        restartFade.OnFadeOut += PreGame;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && State == GameState.PreGame)
        {
            State = GameState.InGame;
        }

        //if ((Input.touchCount > 0 || Input.GetKeyDown(KeyCode.R)) && (State == GameState.Opening || State == GameState.GameOver))
        //    Restart();
    }

    public void Restart()
    {
        restartFade.Fade();
        AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxSwoosh2);
    }

    public void PreGame()
    {
        State = GameState.PreGame;

        StartGround();
        SetBackground();

        CurrentScore = 0;
    }

    public void GameOver()
    {
        if (CurrentScore > HighScore)
            HighScore = CurrentScore;

        State = GameState.GameOver;

        flash.Fade();

        StopGround();        
    }

    public void StopGround()
    {
        GameObject.Find("Ground").GetComponent<TextureScroller>().enabled = false;
    }

    public void StartGround()
    {
        GameObject.Find("Ground").GetComponent<TextureScroller>().enabled = true;
    }

    public void SetBackground()
    {
        backgroundRenderer.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }
}