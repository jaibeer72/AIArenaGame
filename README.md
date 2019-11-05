# AI project with everything under explaned

[**[Introduction.](#introduction.) 2**]{dir="ltr"}

[**[Types Of AI.](#types-of-ai.) 3**]{dir="ltr"}

> [[Navigation](#navigation) 3]{dir="ltr"}
>
> [[The Steering Algorithm](#the-steering-algorithm) 5]{dir="ltr"}
>
> [[Behavioral Trees](#behavioral-trees) 6]{dir="ltr"}
>
> [[Data Driven vs Code Driven](#data-driven-vs-code-driven)
> 6]{dir="ltr"}
>
> [[Tree Traversal](#tree-traversal) 6]{dir="ltr"}
>
> [[Finite State Machine](#finite-state-machine) 7]{dir="ltr"}

[**[Other Research.](#other-research.) 10**]{dir="ltr"}

[**[Setting Up Player](#setting-up-player) 10**]{dir="ltr"}

> [[Input Manager](#input-manager) 15]{dir="ltr"}
>
> [[Updated Movement
> ChirectorCtrl\_WWS.cs](#updated-movement-chirectorctrl_wws.cs)
> 17]{dir="ltr"}
>
> [[Adding Attack types Player](#adding-attack-types-player)
> 19]{dir="ltr"}
>
> [[Player Update to work with magic attack and Adding
> VFX](#player-update-to-work-with-magic-attack-and-adding-vfx)
> 20]{dir="ltr"}
>
> [[Chirector controller and VFX controlling
> management](#chirector-controller-and-vfx-controlling-management)
> 21]{dir="ltr"}

[[Setting Up AI](#setting-up-ai) **22**]{dir="ltr"}

> [[AI Script Setting up](#ai-script-setting-up) 22]{dir="ltr"}
>
> [[AI State Contollers](#ai-state-contollers) 29]{dir="ltr"}
>
> [[If Based AI system](#if-based-ai-system) 32]{dir="ltr"}
>
> [[Switch Based AI](#switch-based-ai) 33]{dir="ltr"}

[**[Final output](#final-output) 41**]{dir="ltr"}

[]{dir="ltr"}

[]{dir="ltr"}

[Introduction.]{dir="ltr"}
==========================

[In video games it is said when you give intelligent behavior to
something that is not being controlled by the player it is called NPC
(non-playable Characters)]{dir="ltr"}

[]{dir="ltr"}

[Types Of AI.]{dir="ltr"}
=========================

[Navigation]{dir="ltr"}
-----------------------

[In games a process called navigation mesh is used to give movement to
AI's.]{dir="ltr"}

[A NavMesh is a Data structure]{dir="ltr"}

[]{dir="ltr"}

[In Unity]{dir="ltr"}

["The Navigation System allows you to create characters which can
navigate the game world. It gives your characters the ability to
understand that they need to take stairs to reach second floor, or to
jump to get over a ditch. The Unity **NavMesh**]{dir="ltr"}

[]{dir="ltr"}

[system consists of the following pieces:]{dir="ltr"}

-   [**NavMesh** (short for Navigation Mesh) is a data structure which
    > describes the walkable surfaces of the game world and allows to
    > find path from one walkable location to another in the game world.
    > The data structure is built, or baked, automatically from your
    > level geometry.]{dir="ltr"}

    > [**NavMesh Agent** component help you to create characters which
    > avoid each other while moving towards their goal. Agents reason
    > about the game world using the NavMesh and they know how to avoid
    > each other as well as moving obstacles.]{dir="ltr"}

    > [**Off-Mesh Link** component allows you to incorporate navigation
    > shortcuts which cannot be represented using a walkable surface.
    > For example, jumping over a ditch or a fence, or opening a door
    > before walking through it, can be all described as Off-mesh
    > links.]{dir="ltr"}

    > [**NavMesh Obstacle** component allows you to describe moving
    > obstacles the agents should avoid while navigating the world. A
    > barrel or a crate controlled by the physics system is a good
    > example of an obstacle. While the obstacle is moving the agents do
    > their best to avoid it, but once the obstacle becomes stationary
    > it will carve a hole in the navmesh so that the agents can change
    > their paths to steer around it, or if the stationary obstacle is
    > blocking the path way, the agents can find a different
    > route."]{dir="ltr"}[^1][]{dir="ltr"}

[This is how unity defines it]{dir="ltr"}

[]{dir="ltr"}

[In terms of Navmesh we use path finding algorithms such as BFS and DFS
to find the shortest location these are called graph search
algorithms]{dir="ltr"}

[]{dir="ltr"}

[The way it works is that]{dir="ltr"}

["we create a funnel that checks each time if the next point is in the
funnel or not. The funnel is composed of 3 points: a central apex, a
left point (called left apex) and a right point (called right apex). At
the beginning, the tested point is on the right side, then we alternate
to the left and so on until we reach our point of destination. (as if we
were walking)]{dir="ltr"}

![\[funnel\_explanation.png\]](.//media/image3.png){width="6.0625in"
height="6.65625in"}[]{dir="ltr"}

[When a point is in the funnel, we continue the algorithm with the other
side.]{dir="ltr"}

[If the point is outside the funnel, depending on which side the tested
point belongs to, we take the apex from the other side of the funnel and
add it to a list of final waypoints.]{dir="ltr"}

[The algorithm is working correctly most of the time. However, the
algorithm had a bug that add the last point twice if none of the
vertices of the last connection before the destination point were added
to the list of final waypoints. We just added an if at the moment but we
could come back later to optimize it.]{dir="ltr"}

[In our case, the funnel algorithm gives this path:]{dir="ltr"}

![The pulled path](.//media/image14.png){width="6.270833333333333in"
height="3.4027777777777777in"}[]{dir="ltr"}

### **[The Steering Algorithm]{dir="ltr"}**

[Now that we have a list of waypoints, we can finally just run our
character at every point.]{dir="ltr"}

[But if there were walls in our geometry, then Buddy would run right
into a corner wall. He won\'t be able to reach his destination because
he isn\'t small enough to avoid the corner walls.]{dir="ltr"}

[That\'s the role of the steering algorithm.]{dir="ltr"}

[Our algorithm is still in heavy development, but its main gist is that
we check if the next position of the agent is not in the navigation
meshes. If that\'s the case, then we change its direction so that the
agent doesn\'t hit the wall like an idiot. There is also a path curving
algorithm, but it\'s still too early to know if we\'ll use that at
all\...]{dir="ltr"}

[With the steering algorithm, we make sure that Buddy moves safely to
his destination. (Look how proud he is!)]{dir="ltr"}

![Buddy is moving](.//media/image5.png){width="6.270833333333333in"
height="3.4027777777777777in"}[]{dir="ltr"}

[So, this is the navigation mesh
algorithm."]{dir="ltr"}[^2][]{dir="ltr"}

[]{dir="ltr"}

[Behavioral Trees]{dir="ltr"}
-----------------------------

[Like navmesh it is a data structure that defines the behavior of a game
works and what the AI can and cannot do in certain
situations]{dir="ltr"}

[]{dir="ltr"}

[]{dir="ltr"}

["a behaviour tree is a tree of hierarchical nodes that control the flow
of decision making of an AI entity. At the extents of the tree, the
leaves, are the actual commands that control the AI entity, and forming
the branches are various types of utility nodes that control the AI's
walk down the trees to reach the sequences of commands best suited to
the situation.]{dir="ltr"}

[]{dir="ltr"}

### [Data Driven vs Code Driven]{dir="ltr"}

[This distinction has little relevance to this guide, however it should
be noted that there are many different possible implementations of
behaviour trees. A main distinction is whether the trees are defined
externally to the codebase, perhaps in XML or a proprietary format and
manipulated with an external editor, or whether the structure of the
trees is defined directly in code via nested class
instances.]{dir="ltr"}

[JBT uses a strange hybrid of these two, where an editor is provided to
allow you to visually construct your behaviour tree, however an exporter
command line tool actually generates java code to represent the
behaviour trees in the code-base.]{dir="ltr"}

[Whatever the implementation, the leaf nodes, the nodes that actually do
the game specific business and control your character or check the
character's situation or surroundings, are something you need to define
yourself in code. Be that in the native language or using a scripting
language such as Lua or Python. These can then be leveraged by your
trees to provide complex behaviours. It is quite how expressive these
nodes can be, sometimes operating more as a standard library to
manipulate data within the tree itself, than just simply character
commands, that really make behaviour trees exciting to me.]{dir="ltr"}

### [Tree Traversal]{dir="ltr"}

[A core aspect of Behavior Trees is that unlike a method within your
codebase, a particular node or branch in the tree may take many ticks of
the game to complete. In the basic implementation of behaviour trees,
the system will traverse down from the root of the tree every single
frame, testing each node down the tree to see which is active,
rechecking any nodes along the way, until it reaches the currently
active node to tick it again.]{dir="ltr"}

[This isn't a very efficient way to do things, especially when the
behaviour tree gets deeper as its developed and expanded during
development. I'd say its a must that any behaviour tree you implement
should store any currently processing nodes so they can be ticked
directly within the behaviour tree engine rather than per tick traversal
of the entire tree. Thankfully JBT fits into this category.]{dir="ltr"}

[Flow]{dir="ltr"}

[A behaviour tree is made up of several types of nodes, however some
core functionality is common to any type of node in a behaviour tree.
This is that they can return one of three statuses. (Depending on the
implementation of the behaviour tree, there may be more than three
return statuses, however I\'ve yet to use one of these in practice and
they are not pertinent to any introduction to the subject) The three
common statuses are as follows:]{dir="ltr"}

[Success]{dir="ltr"}

[Failure]{dir="ltr"}

[Running]{dir="ltr"}

[The first two, as their names suggest, inform their parent that their
operation was a success or a failure. The third means that success or
failure is not yet determined, and the node is still running. The node
will be ticked again next time the tree is ticked, at which point it
will again have the opportunity to succeed, fail or continue
running.]{dir="ltr"}

[This functionality is key to the power of behaviour trees, since it
allows a node\'s processing to persist for many ticks of the game. For
example a Walk node would offer up the Running status during the time it
attempts to calculate a path, as well as the time it takes the character
to walk to the specified location. If the pathfinding failed for
whatever reason, or some other complication arisen during the walk to
stop the character reaching the target location, then the node returns
failure to the parent. If at any point the character\'s current location
equals the target location, then it returns success indicating the Walk
command executed successfully.]{dir="ltr"}

[]{dir="ltr"}

["]{dir="ltr"}[^3][]{dir="ltr"}

[Basically the idea is that it works on the bases of whether or not a
code is running and has failed on its task. If it succeeds then what to
do next is an idea that is implemented. Even players can]{dir="ltr"}

[]{dir="ltr"}

[Finite State Machine]{dir="ltr"} 
----------------------------------

["A finite-state machine, or FSM for short, is a model of computation
based on a hypothetical machine made of one or more states. Only a
single state can be active at the same time, so the machine must
transition from one state to another in order to perform different
actions.]{dir="ltr"}

[FSMs are commonly used to organize and represent an execution flow,
which is useful to implement AI in games. The \"brain\" of an enemy, for
instance, can be implemented using a FSM: every state represents an
action, such as attack or evade:]{dir="ltr"}

![](.//media/image12.png){width="6.041666666666667in"
height="3.6458333333333335in"}[]{dir="ltr"}

[FSM representing the brain of an enemy.]{dir="ltr"}

[An FSM can be represented by a graph, where the nodes are the states
and the edges are the transitions. Each edge has a label informing when
the transition should happen, like the player is near label in the
figure above, which indicates that the machine will transition from
wander to attack if the player is near.]{dir="ltr"}

[Planning States and Their Transitions]{dir="ltr"}

[The implementation of a FSM begins with the states and transitions it
will have. Imagine the following FSM, representing the brain of an ant
carrying leaves home:]{dir="ltr"}

![](.//media/image7.png){width="6.041666666666667in"
height="3.6458333333333335in"}[]{dir="ltr"}

[FSM representing the brain of an ant.]{dir="ltr"}

[The starting point is the find leaf state, which will remain active
until the ant finds the leaf. When that happens, the current state is
transitioned to go home, which remains active until the ant gets home.
When the ant finally arrives home, the active state becomes find leaf
again, so the ant repeats its journey.]{dir="ltr"}

[If the active state is find leaf and the mouse cursor approaches the
ant, there is a transition to the run away state. While that state is
active, the ant will run away from the mouse cursor. When the cursor is
not a threat anymore, there is a transition back to the find leaf
state.]{dir="ltr"}

[Since there are transitions connecting find leaf and run away, the ant
will always run away from the mouse cursor when it approaches *as long
as the ant is finding the leaf*. That *will not* happen if the active
state is go home (check out the figure below). In that case the ant will
walk home fearlessly, only transitioning to the find leaf state when it
arrives home.]{dir="ltr"}

![](.//media/image9.png){width="6.041666666666667in"
height="3.6458333333333335in"}[]{dir="ltr"}

[FSM representing the brain of an ant. Notice the lack of a transition
between run away and go home. "]{dir="ltr"}[^4][]{dir="ltr"}

[]{dir="ltr"}

[This is the most basic type of AI and can be understood easily but it
is slightly difficult to scale and create complex behavior.]{dir="ltr"}

[.]{dir="ltr"}

[Other Research.]{dir="ltr"}
============================

[Setting Up Player]{dir="ltr"} 
===============================

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------
  [\
  \[RequireComponent(typeof(Rigidbody))\]\
  \[RequireComponent(typeof(CapsuleCollider))\]\
  \[RequireComponent(typeof(Animator))\]\
  \
  public class ChirectorCtrl\_WWS : MonoBehaviour\
  {\
  \
  \#region Variables\
  \[SerializeField\]\
  float m\_MovingTurnSpeed = 360;\
  \[SerializeField\] float m\_StationaryTurnSpeed = 180;\
  \[SerializeField\] float m\_JumpPower = 12f;\
  \[Range(1f, 4f)\] \[SerializeField\] float m\_GravityMultiplier = 2f;\
  \[SerializeField\] float m\_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others\
  \[SerializeField\] float m\_MoveSpeedMultiplier = 1f;\
  \[SerializeField\] float m\_AnimSpeedMultiplier = 1f;\
  \[SerializeField\] float m\_GroundCheckDistance = 0.1f;\
  \#endregion\
  \
  \#region Components\
  Rigidbody m\_Rigidbody;\
  Animator m\_Animator;\
  Vector3 m\_CapsuleCenter;\
  CapsuleCollider m\_Capsule;\
  Vector3 m\_GroundNormal;\
  \#endregion\
  \
  bool m\_IsGrounded;\
  float m\_OrigGroundCheckDistance;\
  const float k\_Half = 0.5f;\
  float m\_TurnAmount;\
  float m\_ForwardAmount;\
  float m\_CapsuleHeight;\
  \
  \
  // Use this for initialization\
  void Start()\
  {\
  \
  m\_Animator = GetComponent\<Animator\>();\
  m\_Rigidbody = GetComponent\<Rigidbody\>();\
  m\_Capsule = GetComponent\<CapsuleCollider\>();\
  m\_CapsuleHeight = m\_Capsule.height;\
  m\_CapsuleCenter = m\_Capsule.center;\
  \
  m\_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX \| RigidbodyConstraints.FreezeRotationY \| RigidbodyConstraints.FreezeRotationZ;\
  m\_OrigGroundCheckDistance = m\_GroundCheckDistance;\
  \
  }\
  public void Move(Vector3 move, bool jump)\
  {\
  \
  CheckGroundStatus();\
  if (move.magnitude \> 1f) move.Normalize();\
  move = transform.InverseTransformDirection(move);\
  move = Vector3.ProjectOnPlane(move, m\_GroundNormal);\
  m\_TurnAmount = Mathf.Atan2(move.x, move.z);\
  m\_ForwardAmount = move.z;\
  ApplyExtraTurnRotation();\
  \
  // control and velocity handling is different when grounded and airborne:\
  // control and velocity handling is different when grounded and airborne:\
  if (m\_IsGrounded)\
  {\
  m\_Capsule.center = new Vector3(0, 1f, 0);\
  HandleGroundedMovement(jump);\
  }\
  else\
  {\
  m\_Capsule.center = new Vector3(0, 1.5f, 0);\
  HandleAirborneMovement();\
  }\
  UpdateAnimator(move);\
  }\
  \
  \
  //\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--\
  //Checks Grounded\
  //\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--\
  \
  \
  \#region AnimatorUpdater\
  void UpdateAnimator(Vector3 move)\
  {\
  m\_Animator.SetFloat(\"Forward\", m\_ForwardAmount, 0.1f, Time.deltaTime);\
  m\_Animator.SetFloat(\"Turn\", m\_TurnAmount, 0.1f, Time.deltaTime);\
  //Debug.Log(m\_ForwardAmount);\
  //m\_Animator.SetBool(\"Crouch\", m\_Crouching);\
  m\_Animator.SetBool(\"isGrounded\", m\_IsGrounded);\
  \
  \
  if (!m\_IsGrounded)\
  {\
  m\_Animator.SetFloat(\"Jump\", m\_Rigidbody.velocity.y);\
  }\
  \
  \
  float runCycle = Mathf.Repeat(m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m\_RunCycleLegOffset, 1);\
  \
  float jumpLeg = (runCycle \< k\_Half ? 1 : -1) \* m\_ForwardAmount;\
  \
  if (m\_IsGrounded)\
  {\
  m\_Animator.SetFloat(\"JumpLeg\", jumpLeg);\
  }\
  \
  \
  \
  // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,\
  // which affects the movement speed because of the root motion.\
  \
  \
  if (m\_IsGrounded && move.magnitude \> 0)\
  {\
  m\_Animator.speed = m\_AnimSpeedMultiplier;\
  }\
  else\
  {\
  // don\'t use that while airborne\
  m\_Animator.speed = 1;\
  }\
  }\
  \
  public void OnAnimatorMove()\
  {\
  // we implement this function to override the default root motion.\
  // this allows us to modify the positional speed before it\'s applied.\
  if (m\_IsGrounded && Time.deltaTime \> 0)\
  {\
  Vector3 v = (m\_Animator.deltaPosition \* m\_MoveSpeedMultiplier) / Time.deltaTime;\
  \
  // we preserve the existing y part of the current velocity.\
  v.y = m\_Rigidbody.velocity.y;\
  m\_Rigidbody.velocity = v;\
  }\
  }\
  \#endregion\
  \
  \#region UnderstantLater\
  void CheckGroundStatus()\
  {\
  RaycastHit hitInfo;\
  \#if UNITY\_EDITOR\
  // helper to visualise the ground check ray in the scene view\
  Debug.DrawLine(transform.position + (Vector3.up \* 0.1f), transform.position + (Vector3.up \* 0.1f) + (Vector3.down \* m\_GroundCheckDistance), Color.red, 5f);\
  \#endif\
  // 0.1f is a small offset to start the ray from inside the character\
  // it is also good to note that the transform position in the sample assets is at the base of the character\
  if (Physics.Raycast(transform.position + (Vector3.up \* 0.1f), Vector3.down, out hitInfo, m\_GroundCheckDistance))\
  {\
  m\_GroundNormal = hitInfo.normal;\
  m\_IsGrounded = true;\
  m\_Animator.applyRootMotion = true;\
  }\
  else\
  {\
  m\_IsGrounded = false;\
  m\_GroundNormal = Vector3.up;\
  m\_Animator.applyRootMotion = false;\
  }\
  }\
  void ApplyExtraTurnRotation()\
  {\
  // help the character turn faster (this is in addition to root rotation in the animation)\
  \
  float turnSpeed = Mathf.Lerp(m\_StationaryTurnSpeed, m\_MovingTurnSpeed, m\_ForwardAmount);\
  transform.Rotate(0, m\_TurnAmount \* turnSpeed \* Time.deltaTime, 0);\
  \
  }\
  \#endregion\
  \
  \#region MovementHandelers\
  void HandleGroundedMovement(bool jump)\
  {\
  // check whether conditions are right to allow a jump:\
  if (jump && m\_Animator.GetCurrentAnimatorStateInfo(0).IsTag(\"Grounded\"))\
  {\
  // jump!\
  m\_Rigidbody.velocity = new Vector3(m\_Rigidbody.velocity.x, m\_JumpPower, m\_Rigidbody.velocity.z);\
  m\_IsGrounded = false;\
  m\_Animator.applyRootMotion = false;\
  m\_GroundCheckDistance = 0.1f;\
  }\
  }\
  \
  void HandleAirborneMovement()\
  {\
  // apply extra gravity from multiplier:\
  Vector3 extraGravityForce = (Physics.gravity \* m\_GravityMultiplier) - Physics.gravity;\
  m\_Rigidbody.AddForce(extraGravityForce);\
  \
  m\_GroundCheckDistance = m\_Rigidbody.velocity.y \< 0 ? m\_OrigGroundCheckDistance : 0.01f;\
  }\
  \
  \
  \
  \#endregion\
  }]{dir="ltr"}

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[Input Manager]{dir="ltr"} 
---------------------------

[﻿]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------
  [using UnityEngine;\
  \
  public class PlayerInputChirector : MonoBehaviour\
  {\
  \
  \
  private ChirectorCtrl\_WWS m\_Character; // A reference to the Character on the object\
  private Transform m\_Cam; // A reference to the main camera in the scenes transform\
  private Vector3 m\_CamForward; // The current forward direction of the camera\
  private Vector3 m\_Move;\
  private bool m\_Jump;\
  float jump;\
  \
  \
  \
  \
  // Use this for initialization\
  void Start()\
  {\
  // get the transform of the main camera\
  m\_Move = new Vector3();\
  if (Camera.main != null)\
  {\
  m\_Cam = Camera.main.transform;\
  }\
  else\
  {\
  Debug.LogWarning(\
  \"Warning: no main camera found. Third person character needs a Camera tagged \\\"MainCamera\\\", for camera-relative controls.\", gameObject);\
  // we use self-relative controls in this case, which probably isn\'t what the user wants, but hey, we warned them!\
  }\
  \
  // get the third person character ( this should never be null due to require component )\
  m\_Character = GetComponent\<ChirectorCtrl\_WWS\>();\
  \
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  if (!m\_Jump)\
  {\
  jump = Input.GetAxis(\"Jump\");\
  if (jump \> 0) { m\_Jump = true; } else { m\_Jump = false; }\
  }\
  }\
  \
  private void FixedUpdate()\
  {\
  // read inputs\
  float h = Input.GetAxis(\"Horizontal\");\
  float v = Input.GetAxis(\"Vertical\");\
  \
  // calculate move direction to pass to character\
  \
  if (m\_Cam != null)\
  {\
  // calculate camera relative direction to move:\
  m\_CamForward = Vector3.Scale(m\_Cam.forward, new Vector3(1, 0, 1)).normalized;\
  m\_Move = v \* m\_CamForward + h \* m\_Cam.right;\
  }\
  else\
  {\
  // we use world-relative directions in the case of no main camera\
  m\_Move = v \* Vector3.forward + h \* Vector3.right;\
  }\
  \
  // pass all parameters to the character control script\
  m\_Character.Move(m\_Move, m\_Jump);\
  m\_Jump = false;\
  }\
  }]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[]{dir="ltr"}

[]{dir="ltr"}

### [Updated Movement ChirectorCtrl\_WWS.cs]{dir="ltr"}

[]{dir="ltr"}

  -----------------------------------------
  [ bool m\_Attacking;\
  \[Space\]\
  \[Header(\"Attack Areas Allocater\")\]\
  public Transform\[\] attackAreas;\
  public float range;\
  public LayerMask myLayerMask;\
  ]{dir="ltr"}

  -----------------------------------------

[]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  [private void FixedUpdate()\
  {\
  HandhelAttacking(m\_Attacking);\
  }\
  \
  private void HandhelAttacking(bool isAttacking)\
  {\
  if(isAttacking && m\_IsGrounded)\
  {\
  m\_Attacking = true;\
  RaycastHit hit;\
  for (int i = 0; i \< attackAreas.Length; i++)\
  {\
  Debug.DrawLine(attackAreas\[i\].transform.position + (attackAreas\[i\].transform.forward \* 0.1f), attackAreas\[i\].transform.position + (Vector3.forward \* 0.1f) + (attackAreas\[i\].transform.forward \* range), Color.red, 5f);\
  if (Physics.Raycast(attackAreas\[i\].transform.position, attackAreas\[i\].transform.forward, out hit, range, myLayerMask))\
  {\
  Debug.Log(\"Reaching\");\
  Debug.Log(hit.transform.name);\
  }\
  }\
  }\
  }]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

  -------------------------------------------------------------------------------------
  [ if(m\_Attacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime\>1)\
  {\
  m\_Attacking = false;\
  }]{dir="ltr"}

  -------------------------------------------------------------------------------------

[]{dir="ltr"}

[m\_Animator.SetBool(\"isAttacking\", m\_Attacking);]{dir="ltr"}

[]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  \
  public class MeleeManager : MonoBehaviour\
  \
  {\
  public Transform\[\] attackAreas;\
  public float range;\
  public LayerMask myLayerMask;\
  // Start is called before the first frame update\
  void Start()\
  {\
  \
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  \
  }\
  \
  public void Attack()\
  {\
  RaycastHit hit;\
  for (int i = 0; i \< attackAreas.Length; i++)\
  {\
  Debug.DrawLine(attackAreas\[i\].transform.position + (attackAreas\[i\].transform.forward \* 0.1f), attackAreas\[i\].transform.position + (Vector3.forward \* 0.1f) + (attackAreas\[i\].transform.forward \* range), Color.red, 5f);\
  if (Physics.Raycast(attackAreas\[i\].transform.position, attackAreas\[i\].transform.forward, out hit, range, myLayerMask))\
  {\
  \
  Debug.Log(hit.transform.name);\
  }\
  }\
  \
  }\
  }]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[Adding Attack types Player]{dir="ltr"} 
----------------------------------------

  --------------------------
  [public enum AttackType\
  {\
  basic,\
  stunn,\
  magic\
  };]{dir="ltr"}

  --------------------------

[]{dir="ltr"}

[Player Update to work with magic attack and Adding VFX]{dir="ltr"} 
--------------------------------------------------------------------

  --------------------------------------
  [Vector3 movementVector;\
  AttackType attack;\
  public VisualEffect vfx;]{dir="ltr"}

  --------------------------------------

[]{dir="ltr"}

  -----------------------------------------------------------------------------------
  [public void Move(Vector3 move, bool jump, bool isAttacking, AttackType attType)\
  {\
  \
  movementVector = move;\
  attack = attType;\
  CheckGroundStatus();\
  if (move.magnitude \> 1f) move.Normalize();\
  move = transform.InverseTransformDirection(move);\
  // control and velocity handling is different when grounded and airborne:\
  if (m\_IsGrounded)\
  {\
  \
  m\_Attacking = PlayerInputChirector.isAttacking;\
  }]{dir="ltr"}

  -----------------------------------------------------------------------------------

[]{dir="ltr"}

  -----------------------------------------------------------------------------------------------------------------
  [m\_Animator.applyRootMotion = false;\
  transform.Rotate(0, movementVector.y, 0);\
  m\_Rigidbody.velocity = new Vector3(movementVector.x, transform.position.y, movementVector.z) \* 6;]{dir="ltr"}

  -----------------------------------------------------------------------------------------------------------------

[Hitting velocity]{dir="ltr"}

  -------------------------------------------------------------------------------
  [if (hit.transform.tag == \"Enemy\")\
  {\
  Rigidbody enemy = hit.transform.gameObject.GetComponent\<Rigidbody\>();\
  enemy.AddForce(attackAreas\[i\].transform.forward \* 50, ForceMode.Impulse);\
  Debug.Log(hit.transform.name);\
  }]{dir="ltr"}

  -------------------------------------------------------------------------------

[]{dir="ltr"}

  -----------------------------------------------------------------------------------------
  [m\_Animator.SetBool(\"isAttacking\", m\_Attacking);\
  \
  m\_Animator.SetInteger(\"AttackType\", (int)attack);\
  if (attack == AttackType.magic) { vfx.SendEvent(\"OnPlay\"); Debug.Log((int)attack); }\
  else { vfx.SendEvent(\"OnStop\"); }\
  HandhelAttacking(PlayerInputChirector.isAttacking, attack);]{dir="ltr"}

  -----------------------------------------------------------------------------------------

[]{dir="ltr"}

[Input Manager]{dir="ltr"}

  --------------------------------------------
  [ public void CursorHide(bool enable)\
  {\
  Cursor.visible = enable;\
  if (!enable)\
  {\
  Cursor.lockState = CursorLockMode.Locked;\
  }\
  else\
  {\
  Cursor.lockState = CursorLockMode.None;\
  }\
  }]{dir="ltr"}

  --------------------------------------------

[]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------
  [ if(Input.GetButtonDown(\"Fire1\")\|\| Input.GetButton(\"Fire2\")\|\|Input.GetButtonDown(\"Fire3\")) { isAttacking = true; }\
  if (Input.GetButtonDown(\"Fire1\")) { attType = AttackType.basic; }\
  if (Input.GetButtonDown(\"Fire2\")) { attType = AttackType.stunn; }\
  if (Input.GetButtonDown(\"Fire3\")) { attType = AttackType.magic; }\
  }]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

### [Chirector controller and VFX controlling management]{dir="ltr"} 

  ------------------------------------------------------------------------------------------------------------------------------------------
  [ private void OnTriggerStay(Collider other)\
  {\
  \
  if (attack == AttackType.magic && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \< 1f && other.gameObject.tag == \"Enemy\")\
  {\
  Vector3 dir = transform.position - other.transform.position;\
  dir = dir.normalized;\
  \
  Rigidbody enemy = other.transform.gameObject.GetComponent\<Rigidbody\>();\
  enemy.AddForce(dir \* hitStrength);\
  }\
  }]{dir="ltr"}

  ------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

  -------------------------------------------------------------------------------------------------------------------------------------------
  [ if (attack == AttackType.magic && PlayerInputChirector.isAttacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \< 1f)\
  {\
  vfx.SendEvent(\"OnPlay\");\
  m\_Attacking = true;\
  }\
  if(attack == AttackType.magic && m\_Attacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \> 1f)\
  {\
  m\_Attacking = false;\
  PlayerInputChirector.isAttacking = false;\
  vfx.SendEvent(\"OnStop\");\
  }\
  ]{dir="ltr"}

  -------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

![](.//media/image8.png){width="6.270833333333333in" height="2.375in"}[]{dir="ltr"}
===================================================================================

![](.//media/image6.png){width="6.270833333333333in"
height="4.694444444444445in"}[]{dir="ltr"}

[Setting Up AI]{dir="ltr"}
==========================

[AI Script Setting up]{dir="ltr"} 
----------------------------------

[Now working on AI and using the similar script to make things work but
first we need navmesh for the AI to walk places]{dir="ltr"}

![](.//media/image13.png){width="6.270833333333333in"
height="2.3333333333333335in"}[]{dir="ltr"}

[]{dir="ltr"}

[]{dir="ltr"}

  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  \
  \
  \[RequireComponent(typeof(Rigidbody))\]\
  \[RequireComponent(typeof(CapsuleCollider))\]\
  \[RequireComponent(typeof(Animator))\]\
  public class AIChirectorManager : MonoBehaviour\
  {\
  \#region Variables\
  \[SerializeField\]\
  float m\_MovingTurnSpeed = 360;\
  \[SerializeField\] float m\_StationaryTurnSpeed = 180;\
  \[SerializeField\] float m\_JumpPower = 12f;\
  \[Range(1f, 4f)\] \[SerializeField\] float m\_GravityMultiplier = 2f;\
  \[SerializeField\] float m\_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others\
  \[SerializeField\] float m\_MoveSpeedMultiplier = 1f;\
  \[SerializeField\] float m\_AnimSpeedMultiplier = 1f;\
  \[SerializeField\] float m\_GroundCheckDistance = 0.1f;\
  \#endregion\
  \
  \#region Components\
  Rigidbody m\_Rigidbody;\
  Animator m\_Animator;\
  Vector3 m\_CapsuleCenter;\
  CapsuleCollider m\_Capsule;\
  Vector3 m\_GroundNormal;\
  \#endregion\
  \
  bool m\_IsGrounded;\
  float m\_OrigGroundCheckDistance;\
  const float k\_Half = 0.5f;\
  float m\_TurnAmount;\
  float m\_ForwardAmount;\
  float m\_CapsuleHeight;\
  bool m\_IsAttacking;\
  \[Space\]\
  \[Header(\"Attack Areas Allocater\")\]\
  public Transform\[\] attackAreas;\
  public float range;\
  public LayerMask myLayerMask;\
  Vector3 movementVector;\
  TankAIStates AIstate;\
  public float hitStrength = 100;\
  \
  \
  \
  \
  // Use this for initialization\
  void Start()\
  {\
  m\_Animator = GetComponent\<Animator\>();\
  m\_Rigidbody = GetComponent\<Rigidbody\>();\
  m\_Capsule = GetComponent\<CapsuleCollider\>();\
  m\_CapsuleHeight = m\_Capsule.height;\
  m\_CapsuleCenter = m\_Capsule.center;\
  \
  m\_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX \| RigidbodyConstraints.FreezeRotationY \| RigidbodyConstraints.FreezeRotationZ;\
  m\_OrigGroundCheckDistance = m\_GroundCheckDistance;\
  \
  }\
  public void Move(Vector3 move, bool jump, bool isAttacking, TankAIStates state)\
  {\
  movementVector = move;\
  AIstate = state;\
  CheckGroundStatus();\
  if (move.magnitude \> 1f) move.Normalize();\
  move = transform.InverseTransformDirection(move);\
  move = Vector3.ProjectOnPlane(move, m\_GroundNormal);\
  m\_TurnAmount = Mathf.Atan2(move.x, move.z);\
  m\_ForwardAmount = move.z;\
  ApplyExtraTurnRotation();\
  \
  // control and velocity handling is different when grounded and airborne:\
  // control and velocity handling is different when grounded and airborne:\
  if (m\_IsGrounded)\
  {\
  \
  HandleGroundedMovement(jump);\
  \
  //m\_IsAttacking = PlayerInputChirector.isAttacking;\
  }\
  else\
  {\
  \
  HandleAirborneMovement();\
  }\
  UpdateAnimator(move);\
  }\
  \
  \
  \
  private void HandhelAttacking(bool isAttacking, TankAIStates attackType)\
  {\
  if (isAttacking && m\_IsGrounded)\
  {\
  \
  RaycastHit hit;\
  m\_Animator.applyRootMotion = false;\
  transform.Rotate(0, movementVector.y, 0);\
  m\_Rigidbody.velocity = new Vector3(movementVector.x, transform.position.y, movementVector.z) \* 6;\
  for (int i = 0; i \< attackAreas.Length; i++)\
  {\
  Debug.DrawLine(attackAreas\[i\].transform.position + (attackAreas\[i\].transform.forward \* 0.1f), attackAreas\[i\].transform.position + (Vector3.forward \* 0.1f) + (attackAreas\[i\].transform.forward \* range), Color.green, 5f);\
  if (Physics.Raycast(attackAreas\[i\].transform.position, attackAreas\[i\].transform.forward, out hit, range, myLayerMask))\
  {\
  \
  if (hit.transform.tag == \"Player\")\
  {\
  Rigidbody enemy = hit.transform.gameObject.GetComponent\<Rigidbody\>();\
  enemy.AddForce(attackAreas\[i\].transform.forward \* hitStrength, ForceMode.Impulse);\
  }\
  }\
  }\
  \
  }\
  }\
  \
  \
  //\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--\
  //Checks Grounded\
  //\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--\
  \
  \
  \#region AnimatorUpdater\
  void UpdateAnimator(Vector3 move)\
  {\
  m\_Animator.SetFloat(\"Forward\", m\_ForwardAmount, 0.1f, Time.deltaTime);\
  m\_Animator.SetFloat(\"Turn\", m\_TurnAmount, 0.1f, Time.deltaTime);\
  //Debug.Log(m\_ForwardAmount);\
  //m\_Animator.SetBool(\"Crouch\", m\_Crouching);\
  m\_Animator.SetBool(\"isGrounded\", m\_IsGrounded);\
  m\_Animator.SetBool(\"isAttacking\", m\_IsAttacking);\
  //\#The type of attack is based on the enum\
  m\_Animator.SetInteger(\"AttackType\", (int)AIstate);\
  //\#Maybe get rid of this part as everything can be managed in Animator controller. using move function\
  //if (AIstate == TankAIStates.idel && PlayerInputChirector.isAttacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \< 1f)\
  //{\
  // m\_IsAttacking = true;\
  //}\
  //if (AIstate == TankAIStates.idel && m\_IsAttacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \> 1f)\
  //{\
  // m\_IsAttacking = false;\
  // PlayerInputChirector.isAttacking = false;\
  //}\
  \
  // Need to make sure on animation compleate is attacking is false.\
  HandhelAttacking(m\_IsAttacking, AIstate);\
  if (!m\_IsGrounded)\
  {\
  m\_Animator.SetFloat(\"Jump\", m\_Rigidbody.velocity.y);\
  }\
  \
  \
  float runCycle = Mathf.Repeat(m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m\_RunCycleLegOffset, 1);\
  \
  float jumpLeg = (runCycle \< k\_Half ? 1 : -1) \* m\_ForwardAmount;\
  \
  if (m\_IsGrounded)\
  {\
  m\_Animator.SetFloat(\"JumpLeg\", jumpLeg);\
  }\
  \
  \
  \
  // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,\
  // which affects the movement speed because of the root motion.\
  \
  \
  if (m\_IsGrounded && move.magnitude \> 0)\
  {\
  m\_Animator.speed = m\_AnimSpeedMultiplier;\
  }\
  else\
  {\
  // don\'t use that while airborne\
  m\_Animator.speed = 1;\
  }\
  if (m\_IsAttacking && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \> 1f && AIstate != TankAIStates.idel)\
  {\
  m\_IsAttacking = false;\
  PlayerInputChirector.isAttacking = false;\
  }\
  \
  }\
  \
  public void OnAnimatorMove()\
  {\
  // we implement this function to override the default root motion.\
  // this allows us to modify the positional speed before it\'s applied.\
  if (m\_IsGrounded && Time.deltaTime \> 0 && !m\_IsAttacking)\
  {\
  Vector3 v = (m\_Animator.deltaPosition \* m\_MoveSpeedMultiplier) / Time.deltaTime;\
  \
  // we preserve the existing y part of the current velocity.\
  v.y = m\_Rigidbody.velocity.y;\
  m\_Rigidbody.velocity = v;\
  }\
  \
  }\
  \#endregion\
  \
  \#region GroundCheck etc\
  void CheckGroundStatus()\
  {\
  RaycastHit hitInfo;\
  \#if UNITY\_EDITOR\
  // helper to visualise the ground check ray in the scene view\
  Debug.DrawLine(transform.position + (Vector3.up \* 0.1f), transform.position + (Vector3.up \* 0.1f) + (Vector3.down \* m\_GroundCheckDistance), Color.red, 5f);\
  \#endif\
  // 0.1f is a small offset to start the ray from inside the character\
  // it is also good to note that the transform position in the sample assets is at the base of the character\
  if (Physics.Raycast(transform.position + (Vector3.up \* 0.1f), Vector3.down, out hitInfo, m\_GroundCheckDistance))\
  {\
  m\_GroundNormal = hitInfo.normal;\
  m\_IsGrounded = true;\
  m\_Animator.applyRootMotion = true;\
  }\
  else\
  {\
  m\_IsGrounded = false;\
  m\_GroundNormal = Vector3.up;\
  m\_Animator.applyRootMotion = false;\
  }\
  }\
  void ApplyExtraTurnRotation()\
  {\
  // help the character turn faster (this is in addition to root rotation in the animation)\
  \
  float turnSpeed = Mathf.Lerp(m\_StationaryTurnSpeed, m\_MovingTurnSpeed, m\_ForwardAmount);\
  transform.Rotate(0, m\_TurnAmount \* turnSpeed \* Time.deltaTime, 0);\
  \
  }\
  \#endregion\
  \
  \#region MovementHandelers\
  void HandleGroundedMovement(bool jump)\
  {\
  // check whether conditions are right to allow a jump:\
  if (jump && m\_Animator.GetCurrentAnimatorStateInfo(0).IsTag(\"Grounded\"))\
  {\
  // jump!\
  m\_Rigidbody.velocity = new Vector3(m\_Rigidbody.velocity.x, m\_JumpPower, m\_Rigidbody.velocity.z);\
  m\_IsGrounded = false;\
  m\_Animator.applyRootMotion = false;\
  m\_GroundCheckDistance = 0.1f;\
  }\
  }\
  \
  void HandleAirborneMovement()\
  {\
  // apply extra gravity from multiplier:\
  Vector3 extraGravityForce = (Physics.gravity \* m\_GravityMultiplier) - Physics.gravity;\
  m\_Rigidbody.AddForce(extraGravityForce);\
  \
  m\_GroundCheckDistance = m\_Rigidbody.velocity.y \< 0 ? m\_OrigGroundCheckDistance : 0.01f;\
  }\
  \
  \
  private void OnTriggerStay(Collider other)\
  {\
  \
  if (AIstate == TankAIStates.idel && m\_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime \< 1f && other.gameObject.tag == \"Enemy\")\
  {\
  Vector3 dir = transform.position - other.transform.position;\
  dir = dir.normalized;\
  \
  Rigidbody enemy = other.transform.gameObject.GetComponent\<Rigidbody\>();\
  enemy.AddForce(-dir \* hitStrength, ForceMode.Impulse);\
  //Debug.Log(other.transform.name);\
  }\
  }\
  \#endregion\
  }]{dir="ltr"}

  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[AI State Contollers]{dir="ltr"}
--------------------------------

[Now Creating AI controllers for each of the different AI's]{dir="ltr"}

[]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  using UnityEngine.AI;\
  \
  \
  \[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))\]\
  public class TankAIController : MonoBehaviour\
  {\
  \
  AIChirectorManager aiChirectorScript;\
  GameObject player;\
  TankAIStates tankState;\
  public float range;\
  NavMeshAgent tankAgent;\
  \
  // Start is called before the first frame update\
  void Start()\
  {\
  tankState = TankAIStates.idel;\
  aiChirectorScript = GetComponent\<AIChirectorManager\>();\
  tankAgent = GetComponent\<NavMeshAgent\>();\
  player = GameObject.FindGameObjectWithTag(\"Player\");\
  tankAgent.SetDestination(player.transform.position);\
  \
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  TankAISateAIManagement();\
  }\
  \
  void TankAISateAIManagement()\
  {\
  \
  // we can use reamaining distance for the distance calculation.\
  if (tankAgent.remainingDistance\>tankAgent.stoppingDistance && !PlayerInputChirector.isAttacking)\
  {\
  tankState = TankAIStates.walking;\
  tankAgent.SetDestination(player.transform.position);\
  aiChirectorScript.Move(tankAgent.desiredVelocity\*0.5f, false, false, tankState);\
  }\
  \
  // Checks if tank is russing to the player if not then use the basic attack.\
  if(tankAgent.remainingDistance\<tankAgent.stoppingDistance && tankState!= TankAIStates.rushAttack)\
  {\
  //Make state idel\
  // basic attack\
  aiChirectorScript.Move(tankAgent.desiredVelocity, false, false, tankState);\
  tankState = TankAIStates.basicAttack;\
  }\
  \
  if (Vector3.Distance(transform.position, player.transform.position) \< range && PlayerInputChirector.isAttacking)\
  {\
  tankState = TankAIStates.rushAttack;\
  //Set destination Away from the player\
  //when AI reaches\
  //set destination to player position\
  //and run to him\
  \
  }\
  }\
  \
  Transform TargetManager(Transform playerPos, bool isGoingTowards)\
  {\
  //if the AI is going towards player\
  // Then return a randome point away from the player\
  // if is going towards the player\
  // return towards the player.\
  return this.transform;\
  }\
  }\
  \
  \
  //Psudocode\
  // Spwan\
  // Idel taunt\
  // if(player \> range && player is !attacking)\
  // walk to player and look at him\
  // if(player\<range)\
  // simple AIstate\
  //if(player \< range and player is attacking)\
  // go back away from range\
  // target player\
  // Rush AIstate.\
  // DamageManager ( attacktype , damageAmount)\
  //if(health\>0 && Tank AI is !Stunned)\
  // dont take Damage\
  //if (Health \>0 && AI is Stunned)\
  // Take Damage\
  //if (Health \> 0 && attackType == magic)\
  // Tank.State == stunned\
  // IEnumerator(Stay stunned for 3 seconds)\
  // and go back to idel\
  // if(Health \< 0)\
  // Death animation]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[State manager]{dir="ltr"}

[]{dir="ltr"}

  ----------------------------
  [public enum TankAIStates\
  {\
  idel,\
  walking,\
  basicAttack,\
  rushAttack,\
  stunned\
  };]{dir="ltr"}

  ----------------------------

[]{dir="ltr"}

### [If Based AI system]{dir="ltr"} 

[]{dir="ltr"}

  -------------------------------------------
  [ public float awayRadius;\
  NavMeshAgent tankAgent;\
  bool isGoingTowardsPlayer = false;\
  public Transform secondaryTarget;\
  bool isAwayTargetSet = false;]{dir="ltr"}

  -------------------------------------------

[]{dir="ltr"}

[ ]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  [ if (isGoingTowardsPlayer && tankAgent.remainingDistance \< tankAgent.stoppingDistance)\
  {\
  //tankAgent.SetDestination(player.transform.position + transform.forward \* 1);\
  //StartCoroutine(RushAttackTime());\
  GetComponent\<AIChirectorManager\>().hitStrength = 10f;\
  tankState = TankAIStates.basicAttack;\
  isGoingTowardsPlayer = false;\
  else if ( tankState == TankAIStates.stunned \|\| tankState == TankAIStates.idel)\
  {\
  StopCoroutine(RushAttackTime());\
  StartCoroutine(StunTime());\
  tankAgent.SetDestination(transform.position);\
  aiChirectorScript.Move(Vector3.zero, false, true, tankState);\
  }\
  IEnumerator RushAttackTime()\
  {\
  yield return new WaitForSeconds(4);\
  //Debug.Log(\"called\");\
  tankState = TankAIStates.rushAttack;\
  TargetManager(player.transform, isGoingTowardsPlayer);\
  }\
  \
  void TargetManager(Transform playerPos, bool isGoingTowards)\
  {\
  if (!isGoingTowards && !isAwayTargetSet)\
  {\
  isGoingTowardsPlayer = false;\
  float randAngle = Random.Range(0.0f, 366.0f);\
  secondaryTarget.position = new Vector3(playerPos.position.x + awayRadius \* Mathf.Cos(randAngle), playerPos.position.y, playerPos.position.z + awayRadius \* Mathf.Sin(randAngle));\
  isAwayTargetSet = true;\
  tankState = TankAIStates.rushAttack;\
  //Set destination Away from the player\
  //when AI reaches\
  //set destination to player position\
  //and run to him\
  \
  tankAgent.speed = 2;\
  }\
  else\
  {\
  }\
  //if the AI is going towards player\
  // Then return a randome point away from the player\
  // if is going towards the player\
  // return towards the player.\
  }\
  ]{dir="ltr"}

  --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

### []{dir="ltr"}

### [Switch Based AI]{dir="ltr"}

[Added terrain basic]{dir="ltr"}

[made a feww changes added post-processing so one]{dir="ltr"}

[]{dir="ltr"}

  -------------------------------------------------------------------------------------------------------------
  [switch (tankState)\
  {\
  case TankAIStates.basicAttack:\
  tankAgent.SetDestination(player.transform.position);\
  aiChirectorScript.Move(tankAgent.desiredVelocity, false, true, tankState);\
  StartCoroutine(RushAttackTime());\
  if (tankAgent.remainingDistance \> tankAgent.stoppingDistance)\
  {\
  tankState = TankAIStates.walking;\
  }\
  break;\
  \
  case TankAIStates.idel:\
  tankAgent.SetDestination(transform.position);\
  aiChirectorScript.Move(Vector3.zero, false, true, tankState);\
  break;\
  \
  case TankAIStates.stunned:\
  StopCoroutine(RushAttackTime());\
  StartCoroutine(StunTime());\
  tankAgent.SetDestination(transform.position);\
  aiChirectorScript.Move(Vector3.zero, false, true, tankState);\
  break;\
  \
  case TankAIStates.walking:\
  tankAgent.SetDestination(player.transform.position);\
  aiChirectorScript.Move(tankAgent.desiredVelocity \* 0.5f, false, false, tankState);\
  if (tankAgent.remainingDistance \< tankAgent.stoppingDistance)\
  {\
  tankState = TankAIStates.basicAttack;\
  }\
  \
  break;\
  \
  case TankAIStates.rushAttack:\
  StopCoroutine(RushAttackTime());\
  if (isAwayTargetSet && !isGoingTowardsPlayer)\
  {\
  tankAgent.SetDestination(secondaryTarget.position);\
  aiChirectorScript.Move(tankAgent.desiredVelocity \* 2, false, false, tankState);\
  }\
  if (isAwayTargetSet && tankAgent.remainingDistance \< tankAgent.stoppingDistance && !isGoingTowardsPlayer)\
  {\
  tankAgent.SetDestination(player.transform.position + transform.forward \* 3);\
  aiChirectorScript.Move(tankAgent.desiredVelocity \* 2, false, false, tankState);\
  isGoingTowardsPlayer = true;\
  isAwayTargetSet = false;\
  }\
  if (isGoingTowardsPlayer && tankAgent.remainingDistance \> tankAgent.stoppingDistance)\
  {\
  tankAgent.SetDestination(player.transform.position);\
  GetComponent\<AIChirectorManager\>().hitStrength = 50f;\
  aiChirectorScript.Move(tankAgent.desiredVelocity \* 2, false, false, tankState);\
  \
  }\
  if (isGoingTowardsPlayer && tankAgent.remainingDistance \< tankAgent.stoppingDistance)\
  {\
  //tankAgent.SetDestination(player.transform.position + transform.forward \* 1);\
  //StartCoroutine(RushAttackTime());\
  GetComponent\<AIChirectorManager\>().hitStrength = 10f;\
  tankState = TankAIStates.basicAttack;\
  isGoingTowardsPlayer = false;\
  \
  \
  }\
  break;\
  default:\
  Debug.Log(\"TankState Error\");\
  break;\
  }]{dir="ltr"}

  -------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[]{dir="ltr"}

  --------------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  \
  public class HealthManager : MonoBehaviour\
  {\
  public int health;\
  int currentHealth;\
  \
  // Start is called before the first frame update\
  void Start()\
  {\
  currentHealth = health;\
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  \
  }\
  public void TakeDamage(bool canTakeDamag, int damage)\
  {\
  if (canTakeDamag)\
  {\
  currentHealth -= damage;\
  Debug.Log(currentHealth);\
  }\
  \
  }\
  }]{dir="ltr"}

  --------------------------------------------------------

[]{dir="ltr"}

  --------------------------------------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  using UnityEngine.AI;\
  \
  \
  \[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))\]\
  public class TrollAIController : MonoBehaviour\
  {\
  \
  AIChirectorManager aiChirectorScript;\
  GameObject player;\
  TankAIStates trollState;\
  NavMeshAgent trollAgent;\
  // Start is called before the first frame update\
  void Start()\
  {\
  trollState = TankAIStates.idel;\
  aiChirectorScript = GetComponent\<AIChirectorManager\>();\
  trollAgent = GetComponent\<NavMeshAgent\>();\
  player = GameObject.FindGameObjectWithTag(\"Player\");\
  trollAgent.SetDestination(player.transform.position);\
  trollState = TankAIStates.walking;\
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  TrollAIStateAIManagement();\
  }\
  \
  void TrollAIStateAIManagement()\
  {\
  switch (trollState)\
  {\
  case TankAIStates.basicAttack:\
  trollAgent.SetDestination(player.transform.position);\
  aiChirectorScript.Move(trollAgent.desiredVelocity, false, true, trollState);\
  if (trollAgent.remainingDistance \> trollAgent.stoppingDistance)\
  {\
  trollState = TankAIStates.walking;\
  }\
  break;\
  case TankAIStates.idel:\
  trollAgent.SetDestination(transform.position);\
  aiChirectorScript.Move(Vector3.zero, false, true, trollState);\
  break;\
  case TankAIStates.stunned:\
  //will not happen but resuing code is fine.\
  Debug.Log(\"TrollState Error Stunned\");\
  break;\
  case TankAIStates.walking:\
  trollAgent.SetDestination(player.transform.position);\
  aiChirectorScript.Move(trollAgent.desiredVelocity, false, false, trollState);\
  if (trollAgent.remainingDistance \< trollAgent.stoppingDistance)\
  {\
  trollState = TankAIStates.basicAttack;\
  }\
  break;\
  case TankAIStates.rushAttack:\
  //will not happen but it is useless to create multyple enums\
  Debug.Log(\"TrollState Error RushAttack\");\
  break;\
  default:\
  Debug.Log(\"TrollState Error default\");\
  break;\
  }\
  }\
  }]{dir="ltr"}

  --------------------------------------------------------------------------------

[]{dir="ltr"}

[using System.Collections.Generic;]{dir="ltr"}

[using UnityEngine;]{dir="ltr"}

  --------------------------------------------------------
  [public class HealthManager : MonoBehaviour\
  {\
  public int health;\
  int currentHealth;\
  \
  // Start is called before the first frame update\
  void Start()\
  {\
  currentHealth = health;\
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  \
  }\
  public void TakeDamage(bool canTakeDamag, int damage)\
  {\
  if (canTakeDamag)\
  {\
  currentHealth -= damage;\
  Debug.Log(currentHealth);\
  }\
  \
  }\
  }]{dir="ltr"}

  --------------------------------------------------------

[]{dir="ltr"}

[Area Breaker Script]{dir="ltr"}

[]{dir="ltr"}

  ---------------------------------------------------
  [using System.Collections.Generic;\
  using UnityEngine;\
  using UnityEngine.Experimental.VFX;\
  \
  public class AreaBreakScript : MonoBehaviour\
  {\
  public GameObject trollAIPrefab;\
  public GameObject TankAIPrefab;\
  public Transform\[\] spwanPoints;\
  BoxCollider boxCollider;\
  public VisualEffect visualEffect;\
  // Start is called before the first frame update\
  void Start()\
  {\
  visualEffect = GetComponent\<VisualEffect\>();\
  visualEffect.SendEvent(\"OnStop\");\
  boxCollider = GetComponent\<BoxCollider\>();\
  boxCollider.enabled = false;\
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  \
  }\
  private void OnTriggerExit(Collider other)\
  {\
  if (other.gameObject.tag == \"Player\")\
  {\
  boxCollider.enabled = true;\
  visualEffect.SendEvent(\"OnPlay\");\
  }\
  }\
  \
  }]{dir="ltr"}

  ---------------------------------------------------

[]{dir="ltr"}

[-\> a few lighting changes and working on more soon.]{dir="ltr"}

[-\> Ai managed Areaseprators workng]{dir="ltr"}

[-\> Healthsystem changes]{dir="ltr"}

[-\> so on]{dir="ltr"}

[]{dir="ltr"}

  ------------------------------------------------------------------------------------------------------------------------------------------------------
  [using UnityEngine.UI;\
  public class EnemyHealth : MonoBehaviour\
  {\
  \
  public int health;\
  public int currentHealth;\
  public Canvas damageTextPrefab;\
  // Start is called before the first frame update\
  void Start()\
  {\
  currentHealth = health;\
  }\
  \
  // Update is called once per frame\
  void Update()\
  {\
  \
  }\
  public void TakeDamage(bool canTakeDamag, int damage)\
  {\
  if (canTakeDamag)\
  {\
  \
  \
  if (currentHealth \> 0)\
  {\
  Canvas damageText = Instantiate(damageTextPrefab, this.transform.position + new Vector3(0, 2.0f, 0), this.transform.rotation, transform) as Canvas;\
  damageText.transform.LookAt(-Camera.main.transform.position);\
  //damageText.transform.Translate(Vector3.up \* Time.deltaTime \* 10f);\
  damageText.GetComponentInChildren\<Text\>().text = damage.ToString();\
  Destroy(damageText.gameObject, 1);\
  float amout = 1f - ((float)currentHealth / (float)health);\
  Debug.Log(amout);\
  currentHealth -= damage;\
  \
  }\
  else\
  {\
  if (gameObject.GetComponent\<TankAIController\>() != null)\
  {\
  gameObject.GetComponent\<TankAIController\>().tankState = AIStates.dead;\
  Destroy(gameObject, 6);\
  }\
  if (gameObject.GetComponent\<TrollAIController\>() != null)\
  {\
  gameObject.GetComponent\<TrollAIController\>().trollState = AIStates.dead;\
  Destroy(gameObject, 6);\
  }\
  }\
  }\
  \
  }\
  }]{dir="ltr"}

  ------------------------------------------------------------------------------------------------------------------------------------------------------

[]{dir="ltr"}

[]{dir="ltr"}

![](.//media/image4.png){width="6.270833333333333in"
height="2.0555555555555554in"}![](.//media/image2.png){width="6.270833333333333in"
height="4.819444444444445in"}[]{dir="ltr"}

[]{dir="ltr"}
=============

[Final output]{dir="ltr"} 
==========================

![](.//media/image11.png){width="6.270833333333333in"
height="3.5277777777777777in"}![](.//media/image16.png){width="6.270833333333333in"
height="3.5277777777777777in"}![](.//media/image10.png){width="6.270833333333333in"
height="3.5277777777777777in"}![](.//media/image17.png){width="6.270833333333333in"
height="3.5277777777777777in"}![](.//media/image15.png){width="6.270833333333333in"
height="3.5277777777777777in"}![](.//media/image18.png){width="6.270833333333333in"
height="3.5277777777777777in"}[]{dir="ltr"}

[]{dir="ltr"}

[^1]: [ Unity Technologies. 2019. Unity - Manual: Navigation System in
    Unity. \[ONLINE\] Available at:
    [[https://docs.unity3d.com/Manual/nav-NavigationSystem.html]{.underline}](https://docs.unity3d.com/Manual/nav-NavigationSystem.html).
    \[Accessed 01 June 2019\].]{dir="ltr"}

[^2]: [ GameDev.net. 2019. Navigation Meshes and Pathfinding -
    Artificial Intelligence - GameDev.net. \[ONLINE\] Available at:
    [[https://www.gamedev.net/articles/programming/artificial-intelligence/navigation-meshes-and-pathfinding-r4880/]{.underline}](https://www.gamedev.net/articles/programming/artificial-intelligence/navigation-meshes-and-pathfinding-r4880/).
    \[Accessed 01 June 2019\].]{dir="ltr"}

[^3]: [ Gamasutra: Chris Simpson\'s Blog - Behavior trees for AI: How
    they work. 2019. Gamasutra: Chris Simpson\'s Blog - Behavior trees
    for AI: How they work. \[ONLINE\] Available at:
    [[https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior\_trees\_for\_AI\_How\_they\_work.php]{.underline}](https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php).
    \[Accessed 01 June 2019\].]{dir="ltr"}

[^4]: [ Game Development Envato Tuts+. 2019. Finite-State Machines:
    Theory and Implementation. \[ONLINE\] Available at:
    [[https://gamedevelopment.tutsplus.com/tutorials/finite-state-machines-theory-and-implementation\--gamedev-11867]{.underline}](https://gamedevelopment.tutsplus.com/tutorials/finite-state-machines-theory-and-implementation--gamedev-11867).
    \[Accessed 01 June 2019\].]{dir="ltr"}
