using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float MouseSensivity;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private float VerticalLower;
    [SerializeField] private float VerticalUpper;
    private float CurrentVerticalAngle;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CameraRotationUpdate();
    }

    void CameraRotationUpdate()
    {
        var Vertical = -Input.GetAxis("Mouse Y") * MouseSensivity * Time.deltaTime;
        var Horizontal = Input.GetAxis("Mouse X") * MouseSensivity * Time.deltaTime;

        CurrentVerticalAngle = Mathf.Clamp(CurrentVerticalAngle + Vertical, VerticalUpper, VerticalLower);
        transform.localRotation = Quaternion.Euler(CurrentVerticalAngle, 0, 0);
        PlayerTransform.Rotate(0, Horizontal, 0);
    }
}
