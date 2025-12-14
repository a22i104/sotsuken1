using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float _sensX;
    public float _sensY;
    public Transform _orientation; 
    float _xRotation;
    float _yRotation;

    
    public float rotationSmoothTime = 0.1f;
    private float rotationSmoothVelocityX;
    private float rotationSmoothVelocityY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

     
        _yRotation += Mathf.SmoothDamp(0, _mouseX, ref rotationSmoothVelocityX, rotationSmoothTime);
        _xRotation -= Mathf.SmoothDamp(0, _mouseY, ref rotationSmoothVelocityY, rotationSmoothTime);
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

       
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

    
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
