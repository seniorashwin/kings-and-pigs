using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTransition : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string targetScene;
    [SerializeField] private bool isDoorIn; // true = exit door, false = entry door

    Animator anim;
    bool activated;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // DoorOut: play opening automatically when scene loads
        if (!isDoorIn)
            anim.SetTrigger("open");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
            pc.OnDeath(); // reuse death-style hard lock (no input)

        anim.SetTrigger("open");
    }

    // ===== Animation Events =====

    // Call at END of opening_Door (DoorIn)
    public void OnDoorOpened()
    {
        if (isDoorIn)
            SceneManager.LoadScene(targetScene);
    }

    // Call at END of closing_Door (DoorOut)
    public void OnDoorClosed()
    {
        // DoorOut finished, give control back if needed later
    }
}