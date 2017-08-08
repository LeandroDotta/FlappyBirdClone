using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Fader effectFlash;
    public Fader effectFade;

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

            if (OnUpdateCurrentScore != null)
                OnUpdateCurrentScore.Invoke(_currentScore);
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

    public Bounds ScreenBounds { get; private set; }

    public delegate void UpdateCurrentScoreAction(int value);
    public static event UpdateCurrentScoreAction OnUpdateCurrentScore;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        // Armazena os limites da câmera
        ScreenBounds = Camera.main.OrthographicBounds();
    }

    private void OnEnable()
    {
        Bird.OnFall += BirdFall;
    }

    private void OnDisable()
    {
        Bird.OnFall -= BirdFall;
    }

    private void BirdFall()
    {
        effectFlash.Fade();
        StopGround();

        if(CurrentScore > HighScore)
            HighScore = CurrentScore;
    }

    public void StopGround()
    {
        GameObject.Find("Ground").GetComponent<Animator>().enabled = false;
    }

    public void LoadGame()
    {
        effectFade.Fade(fadeOutCallback: () => {
            SceneManager.LoadScene("Main");
        });
    }
}