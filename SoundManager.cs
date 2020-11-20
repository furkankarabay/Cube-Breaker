using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip crashCubeSound, moveSound, clickSound;
    static AudioSource audioSource;
    void Start()
    {
        crashCubeSound = Resources.Load<AudioClip>("crash");
        moveSound = Resources.Load<AudioClip>("move");
        clickSound = Resources.Load<AudioClip>("click");

        audioSource = GetComponent<AudioSource>();
    } 

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "crash":
                audioSource.PlayOneShot(crashCubeSound);
                break;

            case "move":
                audioSource.PlayOneShot(moveSound);
                break;

            case "click":
                audioSource.PlayOneShot(clickSound);
                break;
            default:
                break;
        }
    }
}
