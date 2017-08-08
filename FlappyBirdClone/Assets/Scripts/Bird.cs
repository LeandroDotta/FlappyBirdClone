using UnityEngine;

public class Bird : MonoBehaviour {
    public RuntimeAnimatorController[] animationControllers;

    private bool flying;
    private bool falling;
    private Animator anim;
    private PlayerController playerController;

    // EVENTS
    public delegate void StartFlyingAction();
    public static event StartFlyingAction OnStartFlyging;
    
    public delegate void FallAction();
    public static event FallAction OnFall;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();

        SwitchColor();
    }

    private void FixedUpdate()
    {
        // Velocidade da animação quando está subindo ou descendo
        if (playerController.rb2d.velocity.y > -2)
        {
            anim.speed = 1;
        }
        else if (playerController.rb2d.velocity.y <= -2)
        {
            anim.speed = 0;
        }

        // Angulo de inclinação
        Vector2 direction = new Vector2(1, Mathf.Clamp(playerController.rb2d.velocity.y / 20, -1, 0.3f));
        transform.right = direction;
    }

    private void Update()
    {
        if (playerController.IsFirstJump && !flying)
            StartFlying();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        OnHitObstacle(coll.collider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnHitObstacle(other);
    }

    public void SwitchColor()
    {
        anim.runtimeAnimatorController = animationControllers[Random.Range(0, animationControllers.Length)];
    }

    public void StartFlying()
    {
        flying = true;

        anim.SetBool("flying", true);

        if (OnStartFlyging != null)
            OnStartFlyging();
    }

    public void Fall()
    {
        anim.enabled = false;
        playerController.enabled = false;

        if (OnFall != null)
            OnFall();
    }

    private void OnHitObstacle(Collider2D obstacle)
    {
        if (obstacle.gameObject.layer == LayerMask.NameToLayer("Obstacles") && !falling)
        {
            falling = true; 

            AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxSmash);

            if (obstacle.CompareTag("Pipe"))
                AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxFall);

            Fall();
        }
    }
}
