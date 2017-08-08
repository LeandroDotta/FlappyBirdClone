using UnityEngine;

public class EventScript : MonoBehaviour
{ 
    public void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.Source.PlayOneShot(clip);
    }
}
