using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    public int score { get; private set; }
    public int comboCounter { get; private set; } = 0;
    public int deathTime { get; set; } = 0;
    public bool isFlying { get; set; } = false;
    public bool isGameStarted { get; set; } = false;

    public Material playerMaterial;
    public GameObject platformPrefab;
    public Platform platformScript;
    public Transform parentPlatform;
    public Transform particlesParent;

    public LayerMask ignoreCubeParticle;

    public int cubeParticlesInRow = 10;
    public float cubeParticleSize = 0.1f;
    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    Vector3 cubeParticlePivot;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    RaycastHit raycastHit;

    private bool isColorChanged = false;
    private float lastPlatfromAngleY;
    private float playerY = 21.202f;
    private float cubeParticlePivotDistance;
    private float playerSpeed = 0.65f;
    private int turnToMiddleAngle = 180;
    private int materialCounter = 1;

    enum Direction
    {
        Forward,
        Back,
        Right,
        Left,
        Middle
    }
    Direction mode = Direction.Middle;
       
    void Start()
    {
        playerMaterial = platformScript.materials[0];

        cubeParticlePivotDistance = cubeParticleSize * cubeParticlesInRow / 2;
        cubeParticlePivot = new Vector3(cubeParticlePivotDistance,cubeParticlePivotDistance,cubeParticlePivotDistance);   
    }

    void Update()
    {
        DetectCube();

        GoNextPlatfrom();

        MouseSwipe();

        ChangeColorClick();

    }

    public void Swipe()
    {
        if (!isFlying)
        {
            if (Input.touchCount > 0) // touches.Length
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    //İlk basıldığı anın Vector2 pozisyonunu al.
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }

                if (t.phase == TouchPhase.Ended)
                {
                    //Bırakılan anın Vector2 pozisyonunu al.
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    //İlk basılan ve bırakılan anın farkını alıyoruz.
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    //Uzunluğu 1 yapılır.
                    currentSwipe.Normalize();

                    //Swipe hareketi yapılmış mı kontrol edilir.
                    if (!(secondPressPos == firstPressPos))
                    {
                        if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
                        {
                            if (currentSwipe.x < 0)
                            {
                                Move("Left");
                            }
                            else
                            {
                                Move("Right");
                            }
                        }
                        else
                        {
                            if (currentSwipe.y < 0)
                            {
                                Move("Back");
                            }
                            else
                            {
                                Move("Forward");
                            }
                        }
                    }
                }
            }
        }
    } // Mobil için.

    public void MouseSwipe()
    {
        // Debug.Log(mode);
        if (!isFlying)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //Swipe hareketi yapılmış mı kontrol edilir.
                if (!(secondPressPos == firstPressPos))
                {
                    SoundManager.PlaySound("move");

                    isGameStarted = true;

                    if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
                    {
                        if (currentSwipe.x < 0) //Left Swipe.
                        {
                            Move("Left");
                        }
                        else //Right Swipe.
                        {
                            Move("Right");
                        }
                    }
                    else
                    {
                        if (currentSwipe.y < 0) //Back Swipe.
                        {
                            Move("Back");

                        }
                        else //Forward Swipe.
                        {
                            Move("Forward");

                        }
                    }
                }
            }
        }
    } //Mouse

    private void Move(string whichSide)
    {
        
        switch (whichSide)
        {
            
            case "Right":

                if (mode == Direction.Middle && !isFlying)
                {
                    ChangeMode(Direction.Right);
                    transform.DOLocalMove(new Vector3(1, transform.localPosition.y, 0), 1-playerSpeed);
     
                }
                else if (mode == Direction.Left && !isFlying)
                {
                    ChangeMode(Direction.Middle);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 1 - playerSpeed);
                }

                break;

            case "Left":

                if (mode == Direction.Middle && !isFlying)
                {
                    ChangeMode(Direction.Left);
                    transform.DOLocalMove(new Vector3(-1, transform.localPosition.y, 0), 1 - playerSpeed);
                }
                else if (mode == Direction.Right && !isFlying)
                {
                    ChangeMode(Direction.Middle);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 1 - playerSpeed);
                }

                break;

            case "Forward":

                if (mode == Direction.Middle && !isFlying)
                {
                    ChangeMode(Direction.Forward);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 1), 0.5f);
                }
                else if (mode == Direction.Back && !isFlying)
                {
                    ChangeMode(Direction.Middle);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 0.5f);
                }

                break;

            case "Back":

                if (mode == Direction.Middle && !isFlying)
                {
                    ChangeMode(Direction.Back);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, -1), 0.5f);
                }
                else if (mode == Direction.Forward && !isFlying)
                {
                    ChangeMode(Direction.Middle);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 0.5f);
                }

                break;

            case "Mid":
                if(!isFlying)
                {
                    comboCounter = 0;
                    deathTime = 1;
                    ChangeMode(Direction.Middle);
                    transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 0.5f);
                }
                
                break;

            default:
                Debug.Log("Wrong side.");
                break;
        }
    }

    private void ChangeMode(Direction mode)
    {
        this.mode = mode;
    }

    private string DetectCube()
    {
        if (Physics.Raycast(transform.localPosition, -transform.up, out raycastHit, 0.5f, ~ignoreCubeParticle))
        {
            isFlying = false;

            if (raycastHit.transform.gameObject.CompareTag("RedCube"))
            {
                isColorChanged = false;
                return "Red";
            }
            else if(raycastHit.transform.gameObject.CompareTag("BlueCube"))
            {
                isColorChanged = false;
                return "Blue";
            }
            else if (raycastHit.transform.gameObject.CompareTag("GreenCube"))
            {
                isColorChanged = false;
                return "Green";
            }
            else if (raycastHit.transform.gameObject.CompareTag("YellowCube"))
            {
                isColorChanged = false;
                return "Yellow";
            }
            else if(raycastHit.transform.gameObject.CompareTag("MiddleCube"))
            {
                return "Middle";
            }
        }
        StartCoroutine(CountTime(0.1f));
        return "No Passing.";
    }

    private void GoNextPlatfrom()
    {
        if (DetectCube() == playerMaterial.name)
        {
            
            Explode(playerMaterial);

            InstantiatePlatform();

            DestroyPlatform();


            score += Combo();

            playerY -= 8f;

            if (mode == Direction.Left)
                transform.DOMove(new Vector3(-1, playerY, 0), 0.4f).SetDelay(0.15f).SetEase(Ease.Linear);
            else if (mode == Direction.Right)
                transform.DOMove(new Vector3(1, playerY, 0), 0.4f).SetDelay(0.15f).SetEase(Ease.Linear);
            else if (mode == Direction.Back)
                transform.DOMove(new Vector3(0, playerY, -1), 0.4f).SetDelay(0.15f).SetEase(Ease.Linear);
            else if (mode == Direction.Forward)
                transform.DOMove(new Vector3(0, playerY, 1), 0.4f).SetDelay(0.15f).SetEase(Ease.Linear);
            else if (mode == Direction.Middle)
                transform.DOMove(new Vector3(0, playerY, 0), 0.4f).SetDelay(0.15f).SetEase(Ease.Linear);
        }
        else if(DetectCube() == "Middle")
        {
            
            if (mode == Direction.Middle && !isFlying && !isColorChanged)
            {
                isColorChanged = true;

                turnToMiddleAngle += 180;
                transform.DOLocalRotate(new Vector3(0, turnToMiddleAngle, 0), 0.5f);

                playerMaterial = platformScript.materials[materialCounter];
                transform.GetComponent<MeshRenderer>().material = playerMaterial;

                MaterialCounter();
            }   
        }
        else if (DetectCube() != playerMaterial.name)
        {        
            Move("Mid"); 
        } 
    }

    private void ChangeColorClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerMaterial = platformScript.materials[materialCounter];
            transform.GetComponent<MeshRenderer>().material = playerMaterial;

            MaterialCounter();
            
        }
    }

    private void MaterialCounter()
    {
        materialCounter += 1;
        if (materialCounter == 4)
            materialCounter = 1;
    }

    IEnumerator CountTime(float time)
    {
        isFlying = true;

        yield return new WaitForSeconds(time);

        isFlying = false;
    }

    private void Explode(Material material)
    {
        SoundManager.PlaySound("crash"); 

        raycastHit.transform.gameObject.SetActive(false);

        Camera.main.DOShakeRotation(1.5f,1f);

        for (int x = 0; x < cubeParticlesInRow; x++)
        {
            for (int y = 0; y < cubeParticlesInRow; y++)
            {
                for (int z = 0; z < cubeParticlesInRow; z++)
                {
                    CreateParticle(x,y,z,material);
                }
            }
        }

        Vector3 explosionPos = raycastHit.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos,explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,raycastHit.transform.position,explosionRadius,explosionUpward);
            }
        }
    }

    private void CreateParticle(int x, int y, int z,Material material)
    {
        GameObject cubeParticle;

        cubeParticle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeParticle.transform.SetParent(particlesParent);

        Destroy(cubeParticle,3f);

        cubeParticle.GetComponent<MeshRenderer>().material = material;
        cubeParticle.layer = 8;

        cubeParticle.transform.position = raycastHit.transform.position + new Vector3(cubeParticleSize * x, cubeParticleSize * y, cubeParticleSize * z) - cubeParticlePivot;
        cubeParticle.transform.localScale = new Vector3(cubeParticleSize, cubeParticleSize, cubeParticleSize);

        cubeParticle.AddComponent<Rigidbody>();
        cubeParticle.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        cubeParticle.GetComponent<Rigidbody>().mass = cubeParticleSize;
    }

    private void InstantiatePlatform()
    {
        lastPlatfromAngleY = parentPlatform.GetChild(parentPlatform.childCount - 1).transform.position.y - 8;

        Instantiate(platformPrefab, new Vector3(0, lastPlatfromAngleY, 0), Quaternion.identity, parentPlatform);
    }

    private void DestroyPlatform()
    {
        DOTween.Kill(parentPlatform.GetChild(0).gameObject);
        Destroy(parentPlatform.GetChild(0).gameObject,1);
    }

    private int Combo()
    {
        comboCounter += 1;

        if (comboCounter > 0)
        {
            return comboCounter * 100;
        }

        return score;
    }
}
