using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform transform;
    public Animator animator;

    public Entity entity;
    public EnemyCombat enemyCombat;
    public EnemyMovement enemyMovement;

    public Player player;

    private float waitDuration;
    private float waitTimeCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = PlayerDistance();
        enemyMovement.Run(playerDistance/MathF.Abs(playerDistance));
        if (MathF.Abs(playerDistance) < enemyCombat.basicAttack_HitboxDistance.x + enemyCombat.basicAttack_HitboxSize.x/2)
        {
            enemyCombat.BasicAttack();
        }
    }

    private float PlayerDistance()
    {
        return player.transform.position.x - transform.position.x;
    }
}
