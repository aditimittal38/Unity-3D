using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractionType { Dragon, Light, Door }

    [SerializeField] private InteractionType type;

    // Dragon
    private Animator animator;
    private bool dragonFlying;

    // Light
    [SerializeField] private Light targetLight;
    private Renderer switchRenderer;

    // Door
    private bool doorOpen;
    private Quaternion closedRot;
    private Quaternion openRot;
    [SerializeField] private float doorOpenAngle = 90f;
    private bool rotating;

    void Start()
    {
        switch (type)
        {
            case InteractionType.Dragon:
                animator = GetComponentInChildren<Animator>();
                if (animator == null) animator = GetComponent<Animator>();
                break;
            case InteractionType.Light:
                switchRenderer = GetComponent<Renderer>();
                if (switchRenderer != null)
                    switchRenderer.material.color = Color.yellow;
                break;
            case InteractionType.Door:
                closedRot = transform.rotation;
                openRot = closedRot * Quaternion.Euler(0f, doorOpenAngle, 0f);
                break;
        }
    }

    void Update()
    {
        if (type == InteractionType.Door && rotating)
        {
            Quaternion target = doorOpen ? openRot : closedRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 5f);
            if (Quaternion.Angle(transform.rotation, target) < 0.5f)
            {
                transform.rotation = target;
                rotating = false;
            }
        }
    }

    public void Interact()
    {
        switch (type)
        {
            case InteractionType.Dragon:
                dragonFlying = !dragonFlying;
                if (animator != null) animator.SetBool("flyParam", dragonFlying);
                UIManager.Instance?.ShowStatus(dragonFlying ? "Dragon takes flight!" : "Dragon lands and rests.");
                break;

            case InteractionType.Light:
                if (targetLight == null) { UIManager.Instance?.ShowStatus("No light assigned!"); return; }
                targetLight.enabled = !targetLight.enabled;
                if (switchRenderer != null)
                    switchRenderer.material.color = targetLight.enabled ? Color.yellow : Color.gray;
                UIManager.Instance?.ShowStatus(targetLight.enabled ? "Light switched ON." : "Light switched OFF.");
                break;

            case InteractionType.Door:
                doorOpen = !doorOpen;
                rotating = true;
                UIManager.Instance?.ShowStatus(doorOpen ? "Door opened." : "Door closed.");
                break;
        }
    }
}
