using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour

{
    public Transform[] attackAreas;
    public float range;
    public LayerMask myLayerMask; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        RaycastHit hit;
        for (int i = 0; i < attackAreas.Length; i++)
        {
            Debug.DrawLine(attackAreas[i].transform.position + (attackAreas[i].transform.forward * 0.1f), attackAreas[i].transform.position + (Vector3.forward * 0.1f) + (attackAreas[i].transform.forward * range), Color.red, 5f);
            if (Physics.Raycast(attackAreas[i].transform.position, attackAreas[i].transform.forward, out hit, range, myLayerMask))
            {
                
                Debug.Log(hit.transform.name); 
            } 
        }
        
    }
}
