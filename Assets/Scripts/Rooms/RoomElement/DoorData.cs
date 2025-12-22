using UnityEngine;

public class DoorData : MonoBehaviour
{
    [Tooltip("Использована ли дверь генератором")]
    public bool IsUsed = false;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = IsUsed ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 1.0f);
    }
#endif
}
