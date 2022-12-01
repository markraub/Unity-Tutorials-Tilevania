using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daytime : MonoBehaviour
{
    [SerializeField] float dayLength;
    [SerializeField] Gradient skygradient;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayLength){
            currentTime = 0;
        }
        Camera cam = GetComponent<Camera>();
        cam.backgroundColor = skygradient.Evaluate(currentTime/dayLength);
        
    }
}
