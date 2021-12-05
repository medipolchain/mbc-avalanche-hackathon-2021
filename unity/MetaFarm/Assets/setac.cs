using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setac : MonoBehaviour
{
    public GameObject acilacak;
 
    public void acil()
    {
        acilacak.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
