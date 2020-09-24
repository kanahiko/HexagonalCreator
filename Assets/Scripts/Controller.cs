using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Camera mainCamera;
    public HexCreator creator;
    Controls controls;
    Vector2 movement;

    public Action<Vector2Int> hexClicked;
    public Action hexDeselected;

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
        controls.PlayerControls.Action.performed += ctx => OnDeselect();
    }


/*    void Update()
    {
        *//*if (movement != Vector2.zero)
        {
            Debug.Log(movement);
        }*//*
    }*/

    void OnClick()
    {
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            Vector2 mousePos = Pointer.current.position.ReadValue();
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(mousePos), out hit, 100))
            {
                Vector3 position = hit.point;
                Vector2Int hexCoordinates = Util.GetPositionToCoordinates(position);
                Vector2Int offsetCoordinates = Util.CubeToOffset(hexCoordinates);

                hexClicked?.Invoke(offsetCoordinates);

                /*switch (hex.type)
                {
                    case TileType.Water:
                        hex.type = TileType.Sand;
                        break;
                    case TileType.Sand:
                        hex.type = TileType.Land;
                        break;
                    case TileType.Land:
                        hex.type = TileType.Water;
                        break;
                }

                creator.ChangeHex(hex.y + hex.x*6, hex.type);*/
            }
        }
    }
    private void OnDeselect()
    {
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            hexDeselected?.Invoke();
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