using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform transform;
    public Animator animator;

    public Entity entity;
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.Run(Input.GetAxisRaw("Horizontal"));

        if (Input.GetMouseButtonDown(0))
        {
            playerCombat.BasicAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }

        if (Input.GetKeyDown(KeyCode.F)) playerCombat.Block(true);
        if (Input.GetKeyUp(KeyCode.F)) playerCombat.Block(false);
    }
}
