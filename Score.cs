using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class Score : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI score;
    public TextMeshProUGUI multiple;
    public TextMeshProUGUI compliment;
    public TextMeshProUGUI tutorial;

    void Update()
    {
        if (int.Parse(score.text) < player.score)
        {
            if(player.score > PlayerPrefs.GetInt("Score"))
                PlayerPrefs.SetInt("Score", player.score);

            score.text = player.score.ToString();
            StartCoroutine(ScoreScale());
        }

        if (player.comboCounter > 1)
        {
            multiple.text = "x" + player.comboCounter;

            if (player.comboCounter > 2 && 4 > player.comboCounter)
                compliment.text = "perfect";
            else if (player.comboCounter >= 4)
                compliment.text = "wonderful";
        }
        else
        {
            multiple.text = "";
            compliment.text = "";
        }
            
        if(player.comboCounter == 1)
            tutorial.CrossFadeAlpha(0, 1, false);

    }

    IEnumerator ScoreScale()
    {
        score.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        score.transform.DOScale(new Vector3(1, 1, 1), 0.05f).SetEase(Ease.Linear); 
    }

}
