using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]
    float rcsThrust = 70f;
    [SerializeField]
    float mainThrust = 10f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friend");
                break;
            case "Fuel":
                break;
            default:
                print("dead");
                break;
        }
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
            rigidBody.AddRelativeForce(Vector3.up*mainThrust);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
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
