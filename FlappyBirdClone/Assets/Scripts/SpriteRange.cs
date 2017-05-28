using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRange : MonoBehaviour {

    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
    }

    public void SetSprite()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
