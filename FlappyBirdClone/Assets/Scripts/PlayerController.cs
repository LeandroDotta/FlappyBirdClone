using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpForce;
    public RuntimeAnimatorController[] animationControllers;

    private bool jump;
    private Vector2 origin;
    private Animator anim;
    private Rigidbody2D rb2d;

    public bool InputEnabled { get; set; }

    void Start()
    {
        origin = transform.position;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        GameManager.Instance.OnChangeState += OnChangeState;
    }

    void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxSwoosh);
        }

        // Velocidade da animação quando está subindo ou descendo
        if (rb2d.velocity.y > -2)
        {
            anim.speed = 1;
        }
        else if (rb2d.velocity.y <= -2)
        {
            anim.speed = 0;
        }

        // Angulo de inclinação
        Vector2 direction = new Vector2(1, Mathf.Clamp(rb2d.velocity.y / 20, -1, 0.3f));
        transform.right = direction;

        // Limita a altura máxima que o pássaro pode atingir
        rb2d.position = new Vector2(rb2d.position.x, Mathf.Clamp(rb2d.position.y, GameManager.Instance.ScreenBounds.min.y, GameManager.Instance.ScreenBounds.max.y));
    }

    void Update()
    {
        if (InputEnabled)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        OnHitObstacle(coll.collider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnHitObstacle(other);
    }

    public void StartPlaying()
    {
        rb2d.isKinematic = false;

        anim.SetBool("playing", true);
    }

    public void SwitchColor()
    {
        GetComponentInChildren<Animator>().runtimeAnimatorController = animationControllers[Random.Range(0, animationControllers.Length)];
    }

    private void OnHitObstacle(Collider2D obstacle)
    {
        if (obstacle.gameObject.layer == LayerMask.NameToLayer("Obstacles") && GameManager.Instance.State == GameState.InGame)
        {
            AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxSmash);

            if (obstacle.CompareTag("Pipe"))
                AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxFall);
            
            GameManager.Instance.GameOver();
        }
    }

    private void OnChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.Opening:
                rb2d.isKinematic = true;
                InputEnabled = false;

                SwitchColor();

                break;

            case GameState.PreGame:
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true;
                InputEnabled = true;

                anim.enabled = true;
                anim.SetBool("playing", false);

                transform.position = new Vector2(origin.x - 2, origin.y);

                SwitchColor();

                break;

            case GameState.InGame:
                rb2d.isKinematic = false;
                anim.SetBool("playing", true);
                break;

            case GameState.GameOver:
                InputEnabled = false;

                anim.enabled = false;
                break;
        }
    }
}
