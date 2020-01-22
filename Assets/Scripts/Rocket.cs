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


    int startLevel = 0;
    

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

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
        if (state != State.Alive)
            return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friend");
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
        startLevel++;
        SceneManager.LoadScene(startLevel);
    }


    void ProcessInput()
    {
        Thrust();
        Rotate();
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
}
