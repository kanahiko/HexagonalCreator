using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Camera mainCamera;
    Controls controls;
    Vector2 movement;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        movement = Vector2.zero;
        controls = new Controls();
        controls.PlayerControls.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.PlayerControls.Movement.canceled += ctx => movement = Vector2.zero;

        controls.PlayerControls.Action.performed += ctx => OnClick();
    }

    void Update()
    {
        /*if (movement != Vector2.zero)
        {
            Debug.Log(movement);
        }*/
    }

    void OnClick()
    {
        Vector2 mousePos = Pointer.current.position.ReadValue();
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(mousePos), out hit, 100))
        {
            Vector3 position = hit.point;

            Debug.Log(Util.GetPositionToCoordinates(position));
        }

    }

    private void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }
}