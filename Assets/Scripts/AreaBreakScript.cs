using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class AreaBreakScript : MonoBehaviour
{
    public GameObject[] AisInScean; 
    //public Transform[] spwanPoints;
    public BoxCollider[] boxCollider;
    public VisualEffect[] visualEffects;
    public int count; 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < visualEffects.Length; i++)
        {
            visualEffects[i].SendEvent("OnStop");
        }
        for (int i = 0; i < AisInScean.Length; i++)
        {
            AisInScean[i].SetActive(false); 
        }
        count = AisInScean.Length; 
        boxCollider = GetComponents<BoxCollider>();
        for (int i = 0; i < boxCollider.Length-1; i++)
        {
            boxCollider[i].enabled = false; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AisInScean.Length; i++)
        {
            if (AisInScean[i]==null)
            {
                count--; 
            }
        }
        if (count <= 0)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < boxCollider.Length; i++)
            {
                boxCollider[i].enabled = true;
            }
            for (int i = 0; i < visualEffects.Length; i++)
            {
                visualEffects[i].SendEvent("OnPlay");
            }
            for (int i = 0; i < AisInScean.Length; i++)
            {
                AisInScean[i].SetActive(true);
            }
        }
    }

}
