using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    InputAction swingAction;
    InputAction stabAction;

    public BoltLauncher boltLauncher;
    public SkinnedMeshRenderer boltString;
    public string shapeName = "Loaded";
    [SerializeField] crossbow_bolts boltCount;
    public FPSMove playerMove;

    int index;
    Coroutine running;

    public Animator swordAnimator;
    public Animator crossbowAnimator;
    public GameObject sword;
    public GameObject crossbow;
    float maxSwingReset = 1.5f;
    float swingReset;
    bool justSwung = false;
    bool isAttacking;
    float attackLen = 0.26f;
    bool doOnce = false;
    float currentTime = 0;
    bool active;
    float from, to, duration, elapsed;
    System.Action onDoneCallback;

    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip reloadSound;


    void Awake()
    {
        swingAction = GetComponent<PlayerInput>().actions["Attack"];
        stabAction = GetComponent<PlayerInput>().actions["Stab"];

        index = boltString.sharedMesh.GetBlendShapeIndex(shapeName);
    }

    void OnEnable()
    {
        swingReset = maxSwingReset;
        if (swingAction != null)
        {
            swingAction.started += OnSwingClubPressed;
        }

        if (stabAction != null)
        {
            stabAction.started += OnStabPressed;
        }
    }

    void OnDisable()
    {
        if (swingAction != null)
        {
            swingAction.started -= OnSwingClubPressed;
        }
        if (stabAction != null){
            stabAction.started -= OnStabPressed;
        }
    }

    void LateUpdate() {
        if (!active) return;
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        boltString.SetBlendShapeWeight(index, Mathf.Lerp(from, to, t));

        if (t >= 1f && onDoneCallback != null)
        {
            var cb = onDoneCallback;
            onDoneCallback = null;
            cb();
        }
    }

    void Update()
    {
        if (playerMove.paused) return;

        if (swingReset <= 0f) {ResetSwings(); return;}
        if (justSwung){
            swingReset -= Time.deltaTime;
        }

        if (isAttacking){
            if (!doOnce){
                currentTime = attackLen;
                doOnce = true;
            }
            currentTime -= Time.deltaTime;
            if (currentTime <= 0){
                isAttacking = false;
                currentTime = 0;
                doOnce = false;
            }
        }
    }

    public void LerpTo(float target, float time, System.Action onDone = null)
    {
        from = boltString.GetBlendShapeWeight(index);
        to = target;
        duration = Mathf.Max(time, 0.0001f);
        elapsed = 0f;
        onDoneCallback = onDone;
        active = true;
    }

/*
    IEnumerator Lerp(float target, float duration)
    {
        float start = boltString.GetBlendShapeWeight(index);
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            boltString.SetBlendShapeWeight(index, Mathf.Lerp(start, target, t / duration));
            yield return null;
        }
        boltString.SetBlendShapeWeight(index, target);
        running = null;
    }
*/
    void ResetSwings(){
        swingReset = maxSwingReset; justSwung = false;
    }

    void BoolTimer(float length){
        if (length <= 0) return;
        length -= Time.deltaTime;
        if (length <= 0){isAttacking = false;}
    }

    private void OnSwingClubPressed(InputAction.CallbackContext context)
    {
        if (playerMove.paused) return;
        if (sword.activeInHierarchy){
            if (swordAnimator != null)
            {
                if (isAttacking) return;
                AnimatorClipInfo[] clipInfo = swordAnimator.GetCurrentAnimatorClipInfo(0);
                string currentAnimationName = clipInfo[0].clip.name;
                if (currentAnimationName != "SwordSwing1" && currentAnimationName != "SwordSwing2" && currentAnimationName != "SwordStab"){
                    if (!justSwung && swingReset == maxSwingReset){
                        justSwung = true;
                        swordAnimator.SetTrigger("attackOne");
                        isAttacking = true;
                    }
                    if (justSwung && swingReset > 0 && swingReset < maxSwingReset){
                        swordAnimator.SetTrigger("attackTwo");
                        ResetSwings();
                        isAttacking = true;
                    } 
                }
            }
        }

        if (crossbow.activeInHierarchy){
            if (crossbowAnimator != null)
            {
                AnimatorClipInfo[] clipInfo = crossbowAnimator.GetCurrentAnimatorClipInfo(0);
                string  currentAnimationName = clipInfo[0].clip.name;
                if (currentAnimationName != "CBReload"){
                    boltLauncher.ShootBolt();
                    LerpTo(0, 0.2f);
                }
            }
        }
    } 

    private void OnStabPressed(InputAction.CallbackContext context)
    {
        if (playerMove.paused) return;
        if (sword.activeInHierarchy){
            if (swordAnimator != null)
            {
                if (isAttacking) return;
                AnimatorClipInfo[] clipInfo = swordAnimator.GetCurrentAnimatorClipInfo(0);
                string currentAnimationName = clipInfo[0].clip.name;
                if (currentAnimationName != "SwordSwing1" && currentAnimationName != "SwordSwing2" && currentAnimationName != "SwordStab")
                {
                    swordAnimator.SetTrigger("stabEnemy");
                    isAttacking = true;
                }

            }
        }
        if (crossbow.activeInHierarchy){
            if (crossbowAnimator != null)
            {
                if (boltCount.value <= 0) return;
                AnimatorClipInfo[] clipInfo = crossbowAnimator.GetCurrentAnimatorClipInfo(0);
                string currentAnimationName = clipInfo[0].clip.name;
                if (currentAnimationName == "cbidle" || currentAnimationName == "CBRun")
                {
                    audioPlayer.PlayOneShot(reloadSound, 0.8f);
                    boltCount.SpendBolt();
                    crossbowAnimator.SetTrigger("isReloading");
                    LerpTo(100, 1.5f, boltLauncher.SpawnBolt);
                } 
                
            }
        }
    }
}
