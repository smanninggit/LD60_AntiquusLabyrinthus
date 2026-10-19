using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class FPSMove : MonoBehaviour
{
    public float walkSpeed = 5f;
    float speed;
    public float sprintSpeed = 8f;
    public float lookSensitivity = 0.1f;
    public Transform cameraTransform;
    public int framesToSkipAfterLock = 3;

    public Animator swordAnimator;
    public GameObject sword;
    public Animator torchAnimator;
    public Animator crossbowAnimator;
    public GameObject crossbow;
    public GameObject pauseMenu;
    public health playerHealth;
    public Animator playerAnimator;
    public Image fadeImage;
    bool running;

    Vector3 deathPos;
    Quaternion deathRot;
    Vector3 animStartPos;
    Quaternion deathQ;
    bool deathActive;
    bool deathFirst;

    Rigidbody rb;
    InputAction lookAction;
    InputAction sprintAction;
    InputAction pauseAction;
    Vector2 moveInput;
    float pitch;
    int skipLookFrames;
    bool needsClick;
    bool discardFirstLook;
    public bool paused;

    [SerializeField] AudioSource footstepsSource;
    [SerializeField] AudioClip footSteps;
    [SerializeField] AudioSource playerVoice;
    [SerializeField] AudioClip outOfBreathClip;
    [SerializeField] float sprintTimeForVoice = 5f;

    float sprintTimer;
    bool voicePlayed;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lookAction = GetComponent<PlayerInput>().actions["Look"];
        sprintAction = GetComponent<PlayerInput>().actions["Sprint"];
        pauseAction = GetComponent<PlayerInput>().actions["Pause"];
        playerAnimator.enabled = false;
    }

    void Start() { LockCursor(); speed = walkSpeed; }

    void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame()) {SetPaused(!paused); }

        if (paused) return;

        bool clicked = Mouse.current.leftButton.wasPressedThisFrame;

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (clicked) LockCursor();
            return;
        }

        if (needsClick && clicked)
        {
            needsClick = false;
            skipLookFrames = framesToSkipAfterLock;
            discardFirstLook = true;
        }

        if (skipLookFrames > 0) { skipLookFrames--; return; }

        if (playerHealth.value <= 0){
            /*
            if (running) return;
            playerAnimator.enabled = true;
            playerAnimator.SetTrigger("Dead");
            running = true;
            StartCoroutine(FadeAndLoad());
            return;*/
            if (running) return;
            running = true;

            deathPos = transform.position; 
            deathRot = transform.rotation;         
            rb.linearVelocity = Vector3.zero;       
            rb.isKinematic = true;

            playerAnimator.enabled = true;
            playerAnimator.Play("PlayerDead", 0, 0f);
            deathFirst = true;
            deathActive = true;

            StartCoroutine(FadeAndLoad());
            return;
        }

        Vector2 look = lookAction.ReadValue<Vector2>();

        if (discardFirstLook)
        {
            if (look != Vector2.zero) discardFirstLook = false;
            return;
        }

        speed = sprintAction.IsPressed() ? sprintSpeed : walkSpeed;
        bool sprinting = sprintAction.IsPressed() && moveInput.sqrMagnitude > 0.01f;
        if (sprinting)
        {
            sprintTimer += Time.deltaTime;
            if (!voicePlayed && sprintTimer >= sprintTimeForVoice)
            {
                voicePlayed = true;
                playerVoice.PlayOneShot(outOfBreathClip);
            }
        }
        else
        {
            sprintTimer = 0f;
            voicePlayed = false;
        }
        if (speed == sprintSpeed){
            if (sword.activeInHierarchy){
                swordAnimator.SetBool("isRunning", true);
            }
            if (crossbow.activeInHierarchy){
                crossbowAnimator.SetBool("isRunning", true);
            }
            torchAnimator.SetBool("isRunning", true);
        }
        if (speed == walkSpeed){
            if (sword.activeInHierarchy){
                swordAnimator.SetBool("isRunning", false);
            }
            if (crossbow.activeInHierarchy){
                crossbowAnimator.SetBool("isRunning", false);
            }
            torchAnimator.SetBool("isRunning", false);
        }

        transform.Rotate(0f, look.x * lookSensitivity, 0f);
        pitch = Mathf.Clamp(pitch - look.y * lookSensitivity, -85f, 85f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void LateUpdate()
    {
        if (!deathActive) return;

        if (deathFirst)
        {
            animStartPos = transform.position;
            deathQ = deathRot * Quaternion.Inverse(transform.rotation);   
            deathFirst = false;
        }

        transform.position = deathPos + deathQ * (transform.position - animStartPos);
        transform.rotation = deathQ * transform.rotation;
    }

    void FixedUpdate()
    {
        if (running) return;     
        Vector2 input = paused ? Vector2.zero : moveInput;
        Vector3 dir = transform.right * input.x + transform.forward * input.y;
        Vector3 v = dir * speed;
        v.y = rb.linearVelocity.y; 
        rb.linearVelocity = v;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        skipLookFrames = framesToSkipAfterLock;
        needsClick = true;
        discardFirstLook = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetPaused(bool value)
    {
        paused = value;
        if (paused) UnlockCursor(); else LockCursor();
        if (pauseMenu != null) pauseMenu.SetActive(value);
        if (paused) UnlockCursor(); else LockCursor();
    }

    IEnumerator FadeAndLoad(){
        yield return new WaitForSeconds(1.5f);

        Color c = fadeImage.color;
        for (float t = 0f; t < 1; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0f, 1f, t / 1);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene("EndScene");
    }
}