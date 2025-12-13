using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] float JumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer = 1;
    [SerializeField] private bool isAbleHoldJump = false;
    [SerializeField] private float maxTime;
    [SerializeField] private float minTime;
    [SerializeField] private float maxJumpForce;
    [Range(0f, 2f)]
    [SerializeField] private float AudioVolume;
    [SerializeField] private AudioClip[] AudioJumps;
    private float timer = 0;
    private Animator anim;
    private Rigidbody rb;
    private AudioSource Audiosource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
    }
    void JumpVector(float jumpforce)
    {
        timer = 0;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        AudioJump();
    }

    bool CheckGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        return isGrounded;
    }

    void Jump()
    {
        if (CheckGrounded())
        {
            if (isAbleHoldJump)
            {
                // Начало прыжка при нажатии
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    timer = 0;
                }

                // Увеличение таймера при удержании
                if (Input.GetKey(KeyCode.Space))
                {
                    timer += Time.deltaTime;
                }

                // Выполнение прыжка при отпускании
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    float calculatedForce;

                    if (timer < minTime)
                    {
                        calculatedForce = JumpForce; // Минимальная сила прыжка
                    }
                    else if (timer >= maxTime)
                    {
                        calculatedForce = maxJumpForce; // Максимальная сила прыжка
                    }
                    else
                    {
                        // Плавное увеличение силы от JumpForce до maxJumpForce
                        float t = (timer - minTime) / (maxTime - minTime);
                        calculatedForce = Mathf.Lerp(JumpForce, maxJumpForce, t);
                    }

                    JumpVector(calculatedForce);
                    timer = 0;
                }
            }
            else // Простой прыжок без удержания
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    JumpVector(JumpForce);
                }
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetFloat("IsFalling", rb.linearVelocity.y);
            }
        }
    }
    private void AudioJump()
    {
        if (AudioJumps.Length != 0)
        {
            Audiosource.PlayOneShot(AudioJumps[Random.Range(0, AudioJumps.Length)], AudioVolume);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = CheckGrounded() ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

}
