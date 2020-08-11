using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Camera mainCamera;
    public HexCreator creator;
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
            Vector2Int hexCoordinates = Util.GetPositionToCoordinates(position);
            Vector2Int offsetCoordinates = Util.CubeToOffset(hexCoordinates);
            Hex hex = Util.HexExist(offsetCoordinates.x, offsetCoordinates.y);
            switch (hex.type)
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

            creator.ChangeHex(hex.y + hex.x*6, hex.type);
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