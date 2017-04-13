using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour {
    private bool scored;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Score();
        }
    }

    private void Score()
    {
        if (!scored)
        {
            scored = true;
            GameManager.Instance.CurrentScore++;

            AudioManager.Instance.Source.PlayOneShot(AudioManager.Instance.sfxScore);
        }
    }
}
