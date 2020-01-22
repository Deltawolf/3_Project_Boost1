using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 70f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float loadTime = 3f;
    [SerializeField] AudioClip mainEngine, death, success;
    [SerializeField] ParticleSystem mainEngineParticles, successParticles, deathParticles;


    int startLevel = SceneManager.GetActiveScene().buildIndex;
    

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    bool isCollisionEnabled = true;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
            ProcessInput();
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !isCollisionEnabled)
            return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", loadTime);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", loadTime);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(startLevel);
    }

    void LoadNextLevel()
    {
        if (startLevel < SceneManager.sceneCountInBuildSettings)
            startLevel++;
        else if (startLevel == SceneManager.sceneCountInBuildSettings)
            startLevel = 0;

        SceneManager.LoadScene(startLevel);

    }


    void ProcessInput()
    {
        Thrust();
        Rotate();
        if (Debug.isDebugBuild)
            DebugKeys();
    }

    void Thrust()
    {

        if (Input.GetButton("Thrust"))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust*Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
                mainEngineParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void Rotate()
    {
        float rotationSpeed = rcsThrust * Time.deltaTime;

        rigidBody.freezeRotation = true; //take manual control of rotation

        if (Input.GetButton("Left"))
        {
            transform.Rotate(Vector3.forward*rotationSpeed);
        }
        if (Input.GetButton("Right"))
        {
            transform.Rotate(-Vector3.forward*rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    void DebugKeys()

    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
            
        if(Input.GetKeyDown(KeyCode.C))
        {
            isCollisionEnabled = !isCollisionEnabled;
        }
    }
}
