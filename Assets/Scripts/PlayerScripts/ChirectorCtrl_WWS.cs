﻿using UnityEngine;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class ChirectorCtrl_WWS : MonoBehaviour
{

    #region Variables
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    #endregion

    #region Components
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    Vector3 m_GroundNormal;
    #endregion

    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_CapsuleHeight;
    bool m_Attacking;
    [Space]
    [Header("Attack Areas Allocater")]
    public Transform[] attackAreas;
    public float range;
    public LayerMask myLayerMask;
    Vector3 movementVector;
    AttackType attack;
    public VisualEffect vfx;
    public float hitStrength = 100;




    // Use this for initialization
    void Start()
    {
        vfx.SendEvent("OnStop");
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

    }
    public void Move(Vector3 move, bool jump, bool isAttacking, AttackType attType)
    {
        movementVector = move;
        attack = attType;
        CheckGroundStatus();
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {

            HandleGroundedMovement(jump);

            m_Attacking = PlayerInputChirector.isAttacking;
        }
        else
        {

            HandleAirborneMovement();
        }
        UpdateAnimator(move);
    }



    private void HandhelAttacking(bool isAttacking, AttackType attackType)
    {
        if (isAttacking && m_IsGrounded)
        {

            RaycastHit hit;
            m_Animator.applyRootMotion = false;
            transform.Rotate(0, movementVector.y, 0);
            m_Rigidbody.velocity = new Vector3(movementVector.x, transform.position.y, movementVector.z) * 6;
            for (int i = 0; i < attackAreas.Length; i++)
            {
                Debug.DrawLine(attackAreas[i].transform.position + (attackAreas[i].transform.forward * 0.1f), attackAreas[i].transform.position + (Vector3.forward * 0.1f) + (attackAreas[i].transform.forward * range), Color.red, 5f);
                if (Physics.Raycast(attackAreas[i].transform.position, attackAreas[i].transform.forward, out hit, range, myLayerMask))
                {

                    if (hit.transform.tag == "Enemy")
                    {
                        Debug.Log("Hit");
                        if (hit.transform.gameObject.GetComponent<TankAIController>() != null)
                        {
                            if (hit.transform.gameObject.GetComponent<TankAIController>().tankState == TankAIStates.stunned)
                            {
                                hit.transform.gameObject.GetComponent<HealthManager>().TakeDamage(true, 20);
                            }
                            else
                            {
                                return; 
                            }
                        }
                        else
                        {
                            GetComponent<HealthManager>().TakeDamage(true, 10);
                            Rigidbody enemy = hit.transform.gameObject.GetComponent<Rigidbody>();
                            enemy.AddForce(attackAreas[i].transform.forward * hitStrength, ForceMode.Impulse);
                        }
                        
                    }
                }
            }

        }
    }


    //------------------
    //Checks Grounded
    //------------------


    #region AnimatorUpdater
    void UpdateAnimator(Vector3 move)
    {
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        //Debug.Log(m_ForwardAmount);
        //m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("isGrounded", m_IsGrounded);
        m_Animator.SetBool("isAttacking", m_Attacking);

        m_Animator.SetInteger("AttackType", (int)attack);
        if (attack == AttackType.magic && PlayerInputChirector.isAttacking && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            vfx.SendEvent("OnPlay");
            m_Attacking = true;
        }
        if (attack == AttackType.magic && m_Attacking && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            m_Attacking = false;
            PlayerInputChirector.isAttacking = false;
            vfx.SendEvent("OnStop");
        }

        HandhelAttacking(PlayerInputChirector.isAttacking, attack);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }


        float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }



        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.


        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
        if (m_Attacking && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && attack != AttackType.magic)
        {
            m_Attacking = false;
            PlayerInputChirector.isAttacking = false;
        }

    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (m_IsGrounded && Time.deltaTime > 0 && !m_Attacking)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }

    }
    #endregion

    #region GroundCheck etc
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.red, 5f);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }
    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)

        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

    }
    #endregion

    #region MovementHandelers
    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Grounded"))
        {
            // jump!           
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }



    #endregion
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (AIstate == AttackType.magic && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && other.gameObject.tag == "Enemy")
    //    {
    //        Vector3 dir = transform.position - other.transform.position;
    //        dir = dir.normalized;

    //        Rigidbody enemy = other.transform.gameObject.GetComponent<Rigidbody>();
    //        enemy.AddForce(dir * hitStrength);
    //        Debug.Log(other.transform.name); 
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {

        if (attack == AttackType.magic && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<TankAIController>() != null)
            {
                other?.GetComponent<TankAIController>()?.isAttacked(attack);
                //other.GetComponent<HealthManager>().TakeDamage(true, 30); 
            }
            else
            {
                Vector3 dir = transform.position - other.transform.position;
                dir = dir.normalized;
                Rigidbody enemy = other.gameObject.GetComponent<Rigidbody>();
                enemy.AddForce(-dir * hitStrength, ForceMode.Impulse);
                other.GetComponent<HealthManager>().TakeDamage(true, 10);
            }
            


            //Debug.Log(other.transform.name);
        }
    }

}
