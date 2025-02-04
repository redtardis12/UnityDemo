using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public float attackCooldown = 1.0f; // Adjustable cooldown time in seconds

    private float lastAttackTime = 0f; // Time when the last attack occurred

    void Update()
    {
        // Check if the left mouse button is clicked and the cooldown has passed
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");

            // Update the last attack time to the current time
            lastAttackTime = Time.time;
        }
    }
}