using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Player player;

    private float offset;

    void Awake()
    {
        offset = transform.position.y - player.transform.position.y; //Uzaklık.
    }
    void Update()
    {
        if (player.isGameStarted)
        {
            if (!player.isFlying)
            {

                if (player.deathTime == 1)
                {
                    player.deathTime = 0;

                    transform.DOMove(transform.position - new Vector3(0, 1f, 0), .25f);
                }

                SetDifficulty();
            }
            else 
            {
                FollowPlayer();
            }   
        }

        GameOver();
        
    }

    private void SetDifficulty()
    {
        if(player.score > 18000)
            transform.DOMove(transform.position - new Vector3(0, 3f, 0), 1.25f);
        else if (player.score > 14000)
            transform.DOMove(transform.position - new Vector3(0, 2.5f, 0), 1.25f);
        else if (player.score > 12000)
            transform.DOMove(transform.position - new Vector3(0, 2f, 0), 1.25f);
        else if (player.score > 10000)
            transform.DOMove(transform.position - new Vector3(0, 1.7f, 0), 1.25f);
        else if (player.score > 8000)
            transform.DOMove(transform.position - new Vector3(0, 1.4f, 0), 1.25f);
        else if (player.score > 6000)
            transform.DOMove(transform.position - new Vector3(0, 1.25f, 0), 1.25f);
        else if (player.score > 3750)
            transform.DOMove(transform.position - new Vector3(0, 1f, 0), 1.25f);
        else if (player.score > 1750)
            transform.DOMove(transform.position - new Vector3(0, .8f, 0), 1.25f);
        else if (player.score > 750)
            transform.DOMove(transform.position - new Vector3(0, 0.65f, 0), 1.25f);
        else
            transform.DOMove(transform.position - new Vector3(0, 0.5f, 0), 1.25f);
    }

    private void GameOver()
    {
        if (transform.position.y < player.transform.position.y)
        {
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    private void FollowPlayer()
    {
        Vector3 curPos = transform.position;
        curPos.y = player.transform.position.y + offset;
        transform.DOMove(curPos, 0.5f); //Kamera'nın yumuşaklığı.
    }
}
