using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Transform))]
public class PlayerDash : MonoBehaviour
{
    [Header("Main configs")]
    [SerializeField] public float DashForce = 1.0f;
    [SerializeField] public float CoolDown = 1.0f;
    [SerializeField] public float DashDuration = 1.0f;
    private bool isAbleToDash = true;
    public bool isDashing = false;
    private float timer = 0.0f;
    private Rigidbody rb;


    private void Start()
    {
        DashForce += GetComponent<DiceCharacter>().DexterityMod;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Dash();
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isAbleToDash)
        {
            isAbleToDash = false;
            Vector3 direction = transform.forward;
            DashVector(direction);
        }
        if (!isAbleToDash)
        {
            timer += Time.deltaTime;
            if (timer >= CoolDown)
            {
                timer = 0.0f;
                isAbleToDash = true;
            }
        }
    }

    private void DashVector(Vector3 direction)
    {
        isDashing = true;
        rb.AddForce(direction * DashForce, ForceMode.Impulse);
        StartCoroutine(StopDash());
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(DashDuration);
        isDashing = false;
    }
}
