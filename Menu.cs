using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public Camera cam;
    public Animator animator;
    public TextMeshProUGUI bestScore;

    void Start()
    {
        bestScore.text = "Best Score \n"+ PlayerPrefs.GetInt("Score");
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.DOMove(cam.transform.position - new Vector3(0, 1, 0), 3);
    }

    public void EndlessMode()
    {
        SoundManager.PlaySound("click");
        animator.SetTrigger("FadeOut");  
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
