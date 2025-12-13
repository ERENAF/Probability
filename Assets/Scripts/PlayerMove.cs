using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMove : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Базовая скорость движения персонажа добавьте MoveSpeed и IsMoving для анимаций")]
    [Range(1f, 20f)]
    [SerializeField] float speed = 10f;
    [SerializeField] bool IsAbleToRun = false;
    [SerializeField] ForceMode forceMode;
    [Range(0f, 2f)]
    [SerializeField] float AudioStepVolume = 1f;
    [SerializeField] AudioClip[] AudioSteps;

    private float currSpeed;
    private Animator anim;
    private Rigidbody rb;
    private AudioSource audioSource;
    private float stepCooldown = 0.5f;
    private float stepTimer = 0f;

    void Start()
    {
        currSpeed = speed;
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

        // Получаем направление движения относительно rotation объекта
        Vector3 movement = CalculateMovementDirection(moveX, moveY);

        rb.AddForce(movement * currSpeed, forceMode);
        AnimMove(moveX, moveY);
        AudioMove();
    }

    private Vector3 CalculateMovementDirection(float horizontal, float vertical)
    {
        // Используем forward и right самого объекта (персонажа)
        Vector3 objectForward = transform.forward;
        Vector3 objectRight = transform.right;

        // Убираем вертикальную компоненту для движения только по горизонтали
        objectForward.y = 0f;
        objectRight.y = 0f;
        objectForward.Normalize();
        objectRight.Normalize();

        // Комбинируем направления относительно rotation объекта
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
            // Рассчитываем величину движения для анимаций
            float moveMagnitude = new Vector2(horizontal, vertical).magnitude;
            bool IsMoving = moveMagnitude > 0.1f;

            anim.SetBool("IsMoving", IsMoving);
            anim.SetFloat("MoveSpeed", moveMagnitude);

            // Опционально: передаем направление для blend tree анимаций
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
                // Частота шагов зависит от скорости
                stepCooldown = 0.5f / (rb.linearVelocity.magnitude / speed);
                audioSource.PlayOneShot(AudioSteps[Random.Range(0, AudioSteps.Length)], AudioStepVolume);
                stepTimer = stepCooldown;
            }
        }
    }

    // Опционально: для отладки направления движения
    private void OnDrawGizmos()
    {
        // Рисуем направление forward объекта
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);

        // Рисуем направление right объекта
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
    }
}
