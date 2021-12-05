using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openclose : MonoBehaviour
{
    public GameObject kapanacak;
    public GameObject acilacak;
    
    public void setle()
    {
        kapanacak.SetActive(false);
        acilacak.SetActive(true);


    }

}
