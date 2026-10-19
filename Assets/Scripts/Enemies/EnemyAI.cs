using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float detectRange = 15f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float turnSpeed = 8f;
    [SerializeField] EnemyHealth health;
    [SerializeField] float attackAngularSpeed = 20f;
    [SerializeField] float attackTurnSpeed = 1f;
    Collider enemyCollider;

    float normalAngularSpeed;
    float normalTurnSpeed;

    bool attacking;

    NavMeshAgent agent;
    Transform player;
    float nextAttackTime;
    bool isAlive = true;
    public bool isFriar;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip engageClip;
    bool doOnce = false;

    void Awake() { 
        
        agent = GetComponent<NavMeshAgent>();
        normalAngularSpeed = agent.angularSpeed;
        normalTurnSpeed = turnSpeed;
        enemyCollider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;
        agent.stoppingDistance = attackRange * 0.8f; 
    }

    void Update()
    {
        if (!isAlive) return;
        if (health.notDead == false) {
        animator.SetTrigger("Death");

        foreach (Collider c in GetComponentsInChildren<Collider>(true))
            Destroy(c);

        foreach (HitPlayer h in GetComponentsInChildren<HitPlayer>(true))
            Destroy(h);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        isAlive = false;
        return;
}
        float dist = Vector3.Distance(transform.position, player.position);

    if (attacking)                       
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        FacePlayer();
        return;
    }

        if (dist > detectRange)
        {
            agent.isStopped = true;          
            return;
        }

        if (!doOnce && isFriar)
        {
            doOnce = true;
            audioSource.PlayOneShot(engageClip);
        }

        if (dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position); 
        }
        else
        {
            agent.isStopped = true;    
            agent.velocity = Vector3.zero;             
            FacePlayer();

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                attacking = true;
                animator.SetTrigger("Attack");
            }
        }
    }

    void LateUpdate()
    {
        bool moving = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isWalking", moving);
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
    }

    public void SlowTurn(){
        agent.angularSpeed = attackAngularSpeed;
        turnSpeed = attackTurnSpeed;
    }

    public void NormalTurn(){
        attacking = false;
        agent.angularSpeed = normalAngularSpeed;
        turnSpeed = normalTurnSpeed;
    }

    void OnDisable() => NormalTurn();
}