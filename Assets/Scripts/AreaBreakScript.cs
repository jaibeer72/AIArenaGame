using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX; 

public class AreaBreakScript : MonoBehaviour
{
    public GameObject trollAIPrefab; 
    public GameObject TankAIPrefab;
    public Transform[] spwanPoints;
    BoxCollider boxCollider;
    public VisualEffect visualEffect; 
    // Start is called before the first frame update
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>(); 
        visualEffect.SendEvent("OnStop");
        boxCollider = GetComponent<BoxCollider>(); 
        boxCollider.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxCollider.enabled = true;
            visualEffect.SendEvent("OnPlay");
        }
    }

}
