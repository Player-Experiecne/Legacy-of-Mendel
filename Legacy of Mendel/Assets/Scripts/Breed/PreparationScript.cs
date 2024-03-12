using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject PreparePanel; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterPreparationPhase()
    {
        PreparePanel.SetActive(true);
    }
    public void ExitPreparationPhase()
    {
        PreparePanel.SetActive(false);
    }
}