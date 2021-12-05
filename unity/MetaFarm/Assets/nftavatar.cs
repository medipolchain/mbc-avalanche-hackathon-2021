using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nftavatar : MonoBehaviour
{
    public GameObject avatar0;
    public GameObject avatar1;
    public GameObject avatar2;
    public GameObject avatar3;
    public GameObject avatar4;

    int id = 100;
    int idmax = 105;


    public void buttonsag()
    {
        id++;
        if (id > 104 || id < 100)
        {
            id = 100;
        }
        Debug.Log("id=" + id);

        if (id == 100)
        {
            avatar1.SetActive(false);
            avatar2.SetActive(false);
            avatar3.SetActive(false);
            avatar4.SetActive(false);
            avatar0.SetActive(true);

        }

        else if (id == 101)
        {
            avatar0.SetActive(false);
            avatar2.SetActive(false);
            avatar3.SetActive(false);   
            avatar4.SetActive(false);
            avatar1.SetActive(true);
        }
           
        else if (id == 102)
        {
            avatar3.SetActive(false);
            avatar4.SetActive(false);
            avatar0.SetActive(false);
            avatar1.SetActive(false);
        avatar2.SetActive(true);
    }
        else if (id == 103)
        {
            avatar0.SetActive(false);
            avatar1.SetActive(false);
            avatar2.SetActive(false);
            avatar4.SetActive(false);
            avatar3.SetActive(true);
        }
        else if (id == 104)
        {
            avatar0.SetActive(false);
            avatar1.SetActive(false);
            avatar2.SetActive(false);
            avatar3.SetActive(false);
            avatar4.SetActive(true);
        }
        
    }

    

    public void buttonsol()
    {
  
    id--;
        if (id > 105 || id < 100)
        {
            id = 100;
        }
        Debug.Log("id=" + id);
        if (id == 100)
            avatar0.SetActive(true);
        else if (id == 101)
        {
            avatar0.SetActive(false);
            avatar4.SetActive(false);
            avatar2.SetActive(false);
            avatar3.SetActive(false);
            avatar1.SetActive(true);
        }  
        else if (id == 102)
        {
            avatar0.SetActive(false);
            avatar1.SetActive(false);
            avatar3.SetActive(false);
            avatar4.SetActive(false);
            avatar2.SetActive(true);
        }
        else if (id == 103)
        {
            avatar0.SetActive(false);
            avatar1.SetActive(false);
            avatar2.SetActive(false);
            avatar4.SetActive(false);
            avatar3.SetActive(true);
        }
        else if (id == 104)
        {
            avatar0.SetActive(false);
            avatar1.SetActive(false);
            avatar3.SetActive(false);
            avatar2.SetActive(false);
            avatar4.SetActive(true);
        }
        else if (id > 104 || id < 100)
            id = 100;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
