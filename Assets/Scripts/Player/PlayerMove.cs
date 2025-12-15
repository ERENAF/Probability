using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMove : MonoBehaviour
{
    [Header("���������")]
    [Tooltip("������� �������� �������� ��������� �������� MoveSpeed � IsMoving ��� ��������")]
    [Range(1f, 20f)]
    [SerializeField] float speed = 10f;
    [SerializeField] bool IsAbleToRun = false;
    [SerializeField] ForceMode forceMode;
    [Range(0f, 2f)]
    [SerializeField] float AudioStepVolume = 1f;
    [SerializeField] AudioClip[] AudioSteps;

    public float currSpeed;
    private Animator anim;
    private Rigidbody rb;
    private AudioSource audioSource;
    private float stepCooldown = 0.5f;
    private float stepTimer = 0f;

    void Start()
    {
        currSpeed = speed + (float)GetComponent<DiceCharacter>().DexterityMod/10;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Run();

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // �������� ����������� �������� ������������ rotation �������
        Vector3 movement = CalculateMovementDirection(moveX, moveY);

        transform.position += movement * Time.deltaTime * speed;
        AnimMove(moveX, moveY);
        AudioMove();
    }

    private Vector3 CalculateMovementDirection(float horizontal, float vertical)
    {
        // ���������� forward � right ������ ������� (���������)
        Vector3 objectForward = transform.forward;
        Vector3 objectRight = transform.right;

        // ������� ������������ ���������� ��� �������� ������ �� �����������
        objectForward.y = 0f;
        objectRight.y = 0f;
        objectForward.Normalize();
        objectRight.Normalize();

        // ����������� ����������� ������������ rotation �������
        return (objectForward * vertical + objectRight * horizontal).normalized;
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsAbleToRun)
        {
            currSpeed = speed * 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && IsAbleToRun)
        {
            currSpeed = speed;
        }
    }

    private void AnimMove(float horizontal, float vertical)
    {
        if (anim != null)
        {
            // ������������ �������� �������� ��� ��������
            float moveMagnitude = new Vector2(horizontal, vertical).magnitude;
            bool IsMoving = moveMagnitude > 0.1f;

            anim.SetBool("IsMoving", IsMoving);
            anim.SetFloat("MoveSpeed", moveMagnitude);

            // �����������: �������� ����������� ��� blend tree ��������
            anim.SetFloat("Horizontal", horizontal);
            anim.SetFloat("Vertical", vertical);
        }
    }

    private void AudioMove()
    {
        if (AudioSteps.Length != 0 && rb.linearVelocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                // ������� ����� ������� �� ��������
                stepCooldown = 0.5f / (rb.linearVelocity.magnitude / speed);
                audioSource.PlayOneShot(AudioSteps[Random.Range(0, AudioSteps.Length)], AudioStepVolume);
                stepTimer = stepCooldown;
            }
        }
    }

    // �����������: ��� ������� ����������� ��������
    private void OnDrawGizmos()
    {
        // ������ ����������� forward �������
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);

        // ������ ����������� right �������
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
    }
}
