 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("Приследуемый обьект")]
    [SerializeField] private Transform target; // Цель за которой следует камера
    [SerializeField] private float sensition; // Чуствительность мыши
    [SerializeField] private float smoothMouseMoveTime;
    [Header("Смещение камеры")]
    [SerializeField] private Vector2 offsetPositon;
    [SerializeField] private Vector3 startOffsetRotation;
    private Vector3 euler; // Углы поворота
    private float mouseY = 0f; // Положение мыши по Y
    private float mouseX = 0f; // Положение мыши по X

    [Header("Настройка приближения камеры")]
    [SerializeField] private float maxZoom = 8f; // Максимальное увелечение
    [SerializeField] private float smoothZoomTime = 0.35f;
    private float zoom; // Увелечение
    private float zoomVelocity = 0f; // Ось Увелечения
    private float mouseScrollDeltaY = 0f;


    private void Start()
    {
        euler = new Vector3
            (startOffsetRotation.x, 
            startOffsetRotation.y, 
            startOffsetRotation.z) ;

        transform.rotation = Quaternion.Euler(euler);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        mouseScrollDeltaY = maxZoom;
        zoom = mouseScrollDeltaY;
    }
    private void LateUpdate()
    {
        MouseScrollWheel();
        MouseRotate();
    }
    private void MouseScrollWheel()
    {
        mouseScrollDeltaY -= Input.mouseScrollDelta.y;
        mouseScrollDeltaY = Mathf.Clamp(mouseScrollDeltaY, 0, maxZoom);

        zoom = Mathf.SmoothDamp(zoom, mouseScrollDeltaY, ref zoomVelocity, smoothZoomTime);
        zoom = Mathf.Clamp(zoom, 0, maxZoom);

        transform.position = target.position;

        transform.position = Vector3.Lerp(transform.position, target.position, Time.fixedDeltaTime);

        transform.Translate(new Vector3(offsetPositon.x, offsetPositon.y, -zoom));
    }
    private void MouseRotate()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * sensition;
            mouseY = Input.GetAxis("Mouse Y") * sensition;

        euler.x -= mouseY;

        euler.x = Mathf.Clamp(euler.x, 0f, 90.0f);

        euler.y += mouseX;

        euler.z = startOffsetRotation.z;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), Time.fixedDeltaTime * sensition);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        euler = new Vector3
           (startOffsetRotation.x,
           startOffsetRotation.y,
           startOffsetRotation.z);

        transform.rotation = Quaternion.Euler(euler);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }

}


