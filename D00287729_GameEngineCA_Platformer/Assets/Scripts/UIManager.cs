using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private Image[] lives;
    
    public void SetScore(int score)
    {
        scoreText.SetText("Aliens Remaining: "+score.ToString());
    }

    public void SetVictory(){
        victoryText.SetText("You saved Earth!");
    }

    public void SetLives(int livesRemaining)
    {
        switch (livesRemaining)
        {
            case 7:
                lives[0].enabled = true;
                lives[1].enabled = true;
                lives[2].enabled = true;
                lives[3].enabled = true;
                lives[4].enabled = true;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 6:
                lives[0].enabled = false;
                lives[1].enabled = true;
                lives[2].enabled = true;
                lives[3].enabled = true;
                lives[4].enabled = true;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 5:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = true;
                lives[3].enabled = true;
                lives[4].enabled = true;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 4:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                lives[3].enabled = true;
                lives[4].enabled = true;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 3:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                lives[3].enabled = false;
                lives[4].enabled = true;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 2:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                lives[3].enabled = false;
                lives[4].enabled = false;
                lives[5].enabled = true;
                lives[6].enabled = true;
                break;
            case 1:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                lives[3].enabled = false;
                lives[4].enabled = false;
                lives[5].enabled = false;
                lives[6].enabled = true;
                break;
            case 0:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                lives[3].enabled = false;
                lives[4].enabled = false;
                lives[5].enabled = false;
                lives[6].enabled = false;
                break;
        }
        
    }

}
