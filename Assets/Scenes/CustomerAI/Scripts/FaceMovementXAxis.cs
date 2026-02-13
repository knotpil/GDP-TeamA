using UnityEngine;
using UnityEngine.AI;

public class FaceMovementXAxis : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Dont touch it please
    }

    void Update()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            // Movement Faces X direction
            Quaternion targetRotation = Quaternion.LookRotation(
                velocity.normalized,
                Vector3.up
            );

            // Rotate so X axis points forward instead of Z
            targetRotation *= Quaternion.Euler(0f, -90f, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}