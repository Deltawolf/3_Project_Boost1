using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 0, 0);
    [SerializeField] float period = 2f;
    float movementFactor;

    Vector3 startingPos;
   
    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; //6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f+.5f; //movement factor between -1 and 1 so divide by 2. Now it is between -.5 and .5 so add .5 to make it 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
        
    }
}
