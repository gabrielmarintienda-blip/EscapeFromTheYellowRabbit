using UnityEngine;

public class RabbitignoreProps : MonoBehaviour
{
    void Start()
    {
        // Colliders of this object (calculated only once)
        Collider2D[] myColliders = GetComponentsInChildren<Collider2D>(true);

        // Ignore collisions with props
        GameObject props = GameObject.Find("Props");
        if (props != null)
        {
            foreach (Collider2D prop in props.GetComponentsInChildren<Collider2D>(true))
                foreach (Collider2D mine in myColliders)
                    Physics2D.IgnoreCollision(mine, prop);
        }
        else
        {
            Debug.LogWarning("RabbitignoreProps: no object named 'Props' was found.", this);
        }

        // Ignore collisions with every other enemy that has this script
        foreach (RabbitignoreProps other in FindObjectsOfType<RabbitignoreProps>())
        {
            if (other == this) continue;

            foreach (Collider2D theirs in other.GetComponentsInChildren<Collider2D>(true))
                foreach (Collider2D mine in myColliders)
                    Physics2D.IgnoreCollision(mine, theirs);
        }
    }
}