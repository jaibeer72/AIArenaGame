﻿using UnityEngine;

public class PlayerInputChirector : MonoBehaviour
{


    private ChirectorCtrl_WWS m_Character;    // A reference to the Character on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;
    public static bool isAttacking;
    public bool enable;
    AttackType attType; 





    // Use this for initialization
    void Start()
    {
        CursorHide(enable);
        // get the transform of the main camera
        m_Move = new Vector3();
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ChirectorCtrl_WWS>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");

        }
        if (!isAttacking)
        {
            if(Input.GetButtonDown("Fire1")|| Input.GetButton("Fire2")||Input.GetButtonDown("Fire3")) { isAttacking = true; }
            if (Input.GetButtonDown("Fire1")) { attType = AttackType.basic; }
            if (Input.GetButtonDown("Fire2")) { attType = AttackType.stunn; }
            if (Input.GetButtonDown("Fire3")) { attType = AttackType.magic; }
        }
    }

    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate move direction to pass to character

        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump, isAttacking,attType);
        m_Jump = false;
    }

    public void CursorHide(bool enable)
    {
        Cursor.visible = enable;
        if (!enable)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
