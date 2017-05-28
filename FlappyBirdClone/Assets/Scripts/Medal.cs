using UnityEngine;
using UnityEngine.UI;

public class Medal : MonoBehaviour {

    public int scoreToBronze;
    public int scoreToSilver;
    public int scoreToGold;
    public int scoreToDiamond;

    public Sprite medalNone;
    public Sprite medalBronze;
    public Sprite medalSilver;
    public Sprite medalGold;
    public Sprite medalDiamond;

    private Image image;
    private GameObject glow;

    void Awake()
    {
        image = GetComponent<Image>();
        glow = transform.Find("Glow").gameObject;
    }

    public void SetMedal(int currentScore)
    {
        if (currentScore < scoreToBronze)
        {
            glow.SetActive(false);

            image.sprite = medalNone;
        }
        else
        {
            glow.SetActive(true);

            if (currentScore < scoreToSilver)
                image.sprite = medalBronze;
            else if (currentScore < scoreToGold)
                image.sprite = medalSilver;
            else if (currentScore < scoreToDiamond)
                image.sprite = medalGold;
            else
                image.sprite = medalDiamond;
        }
    }
}
