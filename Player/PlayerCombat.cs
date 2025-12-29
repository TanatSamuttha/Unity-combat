using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Entity entity;
    public Player player;
    public LayerMask enemyLayer;

    private Animator animator;
    public PlayerMovement playerMovement;
    private Rigidbody rigidbody;
    public Transform transform;

    private int previousAnimatorState;

    public Vector3 basicAttack_HitboxDistance;
    public Vector3 basicAttack_HitboxSize;
    public float basicAttack_Damage;
    private HashSet<Collider> basicAttack_AlreadyHit = new HashSet<Collider>();

    public float parryDuration;
    private float parryTimeCount;

    private int Idle_StateHash;
    private int Run_StateHash;
    private int Block_StateHash;
    private int Hit_StateHash;
    private int BasicAttack1_StateHash;
    private int BasicAttack2_StateHash;
    private int BasicAttack3_StateHash;
    private int BasicAttack4_StateHash;

    private int Run_VariableHash;
    private int Hit_VariableHash;
    private int Block_VariableHash;
    private int Parry_VariableHash;
    private int BasicAttack1_VariableHash;
    private int BasicAttack2_VariableHash;
    private int BasicAttack3_VariableHash;
    private int BasicAttack4_VariableHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = player.animator;
        playerMovement = player.playerMovement;
        rigidbody = player.rigidbody;
        transform = player.transform;

        Idle_StateHash = Animator.StringToHash("Base Layer.Idle");
        Run_StateHash = Animator.StringToHash("Base Layer.Run");
        Block_StateHash = Animator.StringToHash("Base Layer.Block");
        Hit_StateHash = Animator.StringToHash("Base Layer.Hit");
        BasicAttack1_StateHash = Animator.StringToHash("Base Layer.BasicAttack1");
        BasicAttack2_StateHash = Animator.StringToHash("Base Layer.BasicAttack2");
        BasicAttack3_StateHash = Animator.StringToHash("Base Layer.BasicAttack3");
        BasicAttack4_StateHash = Animator.StringToHash("Base Layer.BasicAttack4");

        Run_VariableHash = Animator.StringToHash("Run");
        Block_VariableHash = Animator.StringToHash("Block");
        Hit_VariableHash = Animator.StringToHash("Hit");
        Parry_VariableHash = Animator.StringToHash("Parry");
        BasicAttack1_VariableHash = Animator.StringToHash("BasicAttack1");
        BasicAttack2_VariableHash = Animator.StringToHash("BasicAttack2");
        BasicAttack3_VariableHash = Animator.StringToHash("BasicAttack3");
        BasicAttack4_VariableHash = Animator.StringToHash("BasicAttack4");

        parryTimeCount = parryDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAnimatorStateChange())
        {
            basicAttack_AlreadyHit.Clear();
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Idle_StateHash || animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Run_StateHash)
            {
                animator.SetBool(BasicAttack1_VariableHash, false);
                animator.SetBool(BasicAttack2_VariableHash, false);
                animator.SetBool(BasicAttack3_VariableHash, false);
                animator.SetBool(BasicAttack4_VariableHash, false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Hit_StateHash)
            {
                animator.SetBool(Hit_VariableHash, false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Block_StateHash)
            {
                animator.SetBool(Parry_VariableHash, false);
            }
        }

        if (OnBasicAttack())
        {
            BasicAttackHitBox();
        }

        if (parryTimeCount <= parryDuration)
        {
            parryTimeCount += Time.deltaTime;
        }
    }

    private bool CheckAnimatorStateChange()
    {
        int currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if (currentAnimatorState != previousAnimatorState)
        {
            previousAnimatorState = currentAnimatorState;
            return true;
        }
        return false;
    }

    private bool OnBasicAttack()
    {
        bool res = false;
        res |= animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack1_StateHash;
        res |= animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack2_StateHash;
        res |= animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack3_StateHash;
        res |= animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack4_StateHash;
        return res;
    }

    public void BasicAttack()
    {
        if(!playerMovement.OnGround()) return;
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Idle_StateHash || animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Run_StateHash || animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack4_StateHash)
        {
            animator.SetBool(BasicAttack4_VariableHash, false);
            animator.SetBool(BasicAttack1_VariableHash, true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack1_StateHash)
        {
            animator.SetBool(BasicAttack1_VariableHash, false);
            animator.SetBool(BasicAttack2_VariableHash, true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack2_StateHash)
        {
            animator.SetBool(BasicAttack2_VariableHash, false);
            animator.SetBool(BasicAttack3_VariableHash, true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack3_StateHash)
        {
            animator.SetBool(BasicAttack3_VariableHash, false);
            animator.SetBool(BasicAttack4_VariableHash, true);
        }
    }

    private void BasicAttackHitBox()
    {
        Vector3 hitboxPosition = transform.position + new Vector3(basicAttack_HitboxDistance.x * playerMovement.GetFaceSide(), basicAttack_HitboxDistance.y, 0);
        Collider[] hitEnemies = Physics.OverlapBox(hitboxPosition, basicAttack_HitboxSize * 0.5f, transform.rotation, enemyLayer);
        foreach(Collider enemy in hitEnemies)
        {
            if (basicAttack_AlreadyHit.Add(enemy))
            {
                var (success, parry) = enemy.GetComponent<Entity>().Hit(basicAttack_Damage);
                if (success)
                {
                    rigidbody.velocity = new Vector3(0, 0, 0);
                    if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == BasicAttack4_StateHash)
                    {
                        enemy.GetComponent<Rigidbody>().AddForce(new Vector3(10 * playerMovement.GetFaceSide(), 10, 0), ForceMode.Impulse);
                    }
                }
                else if (parry)
                {
                    animator.SetBool(Hit_VariableHash, true);
                }
            }
        }
    }

    public void Block(bool block)
    {
        if (block)
        {
            animator.SetBool(Block_VariableHash, true);
            parryTimeCount = 0;
        }
        else
        {
            animator.SetBool(Block_VariableHash, false);
        }
    }

    public void GetBlock()
    {
        if (entity.hitSuccess) entity.hitSuccess = !(animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Block_StateHash);
        entity.parrying = parryTimeCount < parryDuration;
        animator.SetBool(Parry_VariableHash, parryTimeCount < parryDuration);
    }

    public void HitSuccess()
    {
        animator.SetBool(Hit_VariableHash, true);
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null || player.playerMovement == null) return;

        float face = player.playerMovement.GetFaceSide();

        Vector3 hitboxCenter =
            transform.position +
            new Vector3(
                basicAttack_HitboxDistance.x * face,
                basicAttack_HitboxDistance.y,
                basicAttack_HitboxDistance.z
            );

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(hitboxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, basicAttack_HitboxSize);
    }
}
