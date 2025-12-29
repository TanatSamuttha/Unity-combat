using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyMovement : MonoBehaviour
{
    public Enemy enemy;

    private Animator animator;
    private Rigidbody rigidbody;
    public Transform transform;

    public float groundCheckBoxDistance;
    public Vector3 groundCheckBoxSize;
    public LayerMask groundLayer;

    public float runSpeed;
    public float jumpPower;
    public float fallMultiplier;
    
    private int faceSide = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = enemy.animator;
        rigidbody = enemy.rigidbody;
        transform = enemy.transform;
    }

    // Update is called once per frame
    void Update()
    {
        OnFalling();
    }

    public void Run(float side)
    {
        if (side == 0 || !OnGround() || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            animator.SetBool("Run", false);
            if(OnGround()) rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
            return;
        }

        animator.SetBool("Run", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || (animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") && OnGround()))
        {
            rigidbody.velocity = new Vector3(runSpeed * side, rigidbody.velocity.y, 0);
            if (side != faceSide)
            {
                faceSide = (int)side;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }
    }

    public float GetFaceSide()
    {
        return faceSide;
    }

    public void Jump()
    {
        if (!OnGround()) return;
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnFalling()
    {
        if (rigidbody.velocity.y < 0 && rigidbody.useGravity)
        {
            rigidbody.velocity += Physics.gravity.y * fallMultiplier * Vector3.up * Time.deltaTime;
        }

        if(!OnGround() && rigidbody.velocity.y != 0)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }
    }

    public bool OnGround()
    {
        Vector3 center = transform.position + Vector3.down * groundCheckBoxDistance;
        return Physics.CheckBox(
            center, 
            groundCheckBoxSize * 0.5f, 
            Quaternion.identity, 
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position + Vector3.down * groundCheckBoxDistance,
            groundCheckBoxSize
        );
    }
}
