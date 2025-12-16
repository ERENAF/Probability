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
    public float maxShootDistance = 100f;



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

    public Vector3 GetShootTargetPoint()
    {
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit,maxShootDistance))
        {
            return hit.point;
        }

        return ray.GetPoint(maxShootDistance);
    }

    public Vector3 GetShootDirectionFromPoint(Vector3 shootOrigin)
    {
        Vector3 targetPoint = GetShootTargetPoint();
        return (targetPoint - shootOrigin).normalized;
    }


}
