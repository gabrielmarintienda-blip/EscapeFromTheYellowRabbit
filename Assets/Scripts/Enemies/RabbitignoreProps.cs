using UnityEngine;

public class RabbitignoreProps : MonoBehaviour
{
  void Start()
    {
        // Find the parent object that holds every prop in the level
        GameObject props = GameObject.Find("Props");
        if (props == null) return;

        // Ignore collisions between every collider of this object and every prop collider
        foreach (Collider2D prop in props.GetComponentsInChildren<Collider2D>(true))
            foreach (Collider2D mine in GetComponentsInChildren<Collider2D>(true))
                Physics2D.IgnoreCollision(mine, prop);
    }
}
