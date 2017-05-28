using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpForce;

    [HideInInspector]
    public Rigidbody2D rb2d;

    private bool jump;

    public bool IsFirstJump { get; private set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

        // Limita a altura máxima que o pássaro pode atingir
        rb2d.position = new Vector2(rb2d.position.x, Mathf.Clamp(rb2d.position.y, GameManager.Instance.ScreenBounds.min.y, GameManager.Instance.ScreenBounds.max.y));
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!IsFirstJump)
            {
                IsFirstJump = true;
                rb2d.isKinematic = false;
            }

            jump = true;
        }
    }
}
