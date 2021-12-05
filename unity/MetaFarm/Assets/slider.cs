using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider : MonoBehaviour
{
    public Text sliderValue;
    public Slider slider1;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderValue.text = slider1.value.ToString("0");

    }
}
