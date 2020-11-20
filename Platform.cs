using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int Color2 { get; set; }

    [Range(0, 2)] public int Color1;

    public GameObject[] cubes { get; set; } = new GameObject[5];

    public Material[] materials = new Material[5];

    private int randomNumber;

    void Start()
    {
        Color2 = Color1 + 1;
        Cubes();

        SetCubesColorAndTag();
    }

    private void Cubes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cubes[i] = transform.GetChild(i).transform.gameObject;
        }
    }

    public GameObject MiddleCube()
    {
        return cubes[0].gameObject;
    }

    private void SetCubesColorAndTag()
    {
        cubes[0].GetComponent<MeshRenderer>().material = materials[4]; //Middle Cube için.
        cubes[0].gameObject.tag = "MiddleCube";

        for (int i = 1; i < cubes.Length; i++)
        {
            randomNumber = Random.Range(Color1, Color2 + 2);
            cubes[i].GetComponent<MeshRenderer>().material = materials[randomNumber];

            if (randomNumber == 0)
                cubes[i].gameObject.tag = "BlueCube";
            else if (randomNumber == 1)
                cubes[i].gameObject.tag = "GreenCube";
            else if (randomNumber == 2)
                cubes[i].gameObject.tag = "RedCube";
            else if (randomNumber == 3)
                cubes[i].gameObject.tag = "YellowCube";

        }
    }
}
