using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum GameState { mainMenu, about , mainGame }

public class MainMenuBehivior : MonoBehaviour
{
    GameState gameState; 
    public GameObject player;
    public GameObject FreeLookCam;
    private Animator animator; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
        FreeLookCam.SetActive(false); 
        player.GetComponent<PlayerInputChirector>().enable = false;
        gameState = GameState.mainMenu; 
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        AnimatorUpdater();
    }

    // Animator Update
    public void AnimatorUpdater()
    {
        animator.SetInteger("GameState", (int)gameState);
    }

    public void ChangeGameState(GameState gs)
    {
        gameState = gs; 
    }
}
