using UnityEngine;
using System; 
using System.Collections; // For coroutines

public class MissileControl : MonoBehaviour
{
    public float speed = 20f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;

    // Static reference to the currently controlled missile
    public static MissileControl CurrentControlledMissile;
    public static event Action OnMissileDestroyed; // Event to notify missile destruction

    private bool controlEnabled = false; // Flag to enable control

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(EnableControlAfterDelay(1f)); // 2 seconds delay before enabling control

    }

    IEnumerator EnableControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        controlEnabled = true;
    }

    void Update()
    {
        // Check if this missile is the currently controlled one
        if (CurrentControlledMissile == this && controlEnabled)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            transform.Rotate(moveVertical * rotationSpeed * Time.deltaTime, moveHorizontal * rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }

    void OnDestroy()
    {
        if (CurrentControlledMissile == this)
        {
            // Trigger the event when the controlled missile is destroyed
            OnMissileDestroyed?.Invoke();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    public void SetAsCurrentControlled()
    {
        CurrentControlledMissile = this;
    }
}
