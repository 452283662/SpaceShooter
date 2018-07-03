using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script defines the borders of ‘Player’s’ movement. Depending on the chosen handling type, it moves the ‘Player’ together with the pointer.
/// </summary>

[System.Serializable]
public class Borders
{
    [Tooltip("offset from viewport borders for player's movement")]
    public float minXOffset = 1f, maxXOffset = 1f, minYOffset = 1f, maxYOffset = 1f;
    [HideInInspector]
    public float minX, maxX, minY, maxY;
}

public class PlayerMoving : MonoBehaviour,IDragHandler,IEndDragHandler {

    [Tooltip("offset from viewport borders for player's movement")]
    public Borders borders;
    Camera mainCamera;
    bool controlIsActive = true;
    float maxBorderX = 7.0f;
    bool setOffest = false;
    Vector3 mouseOffset = Vector3.zero;
    Transform cacheTransform;

    public static PlayerMoving instance; //unique instance of the script for easy access to the script

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        cacheTransform = transform;
        mainCamera = Camera.main;
        ResizeBorders();                //setting 'Player's' moving borders deending on Viewport's size
    }

    public void OnDrag(PointerEventData data)
    {
        if (controlIsActive)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(data.position); //calculating mouse position in the worldspace
            mousePosition.z = cacheTransform.position.z;
            if (!setOffest){
                mouseOffset = cacheTransform.position - mousePosition;
                setOffest = true;
            }
            cacheTransform.position = mousePosition + mouseOffset;

            cacheTransform.position = new Vector3    //if 'Player' crossed the movement borders, returning him back 
                (
                    Mathf.Clamp(cacheTransform.position.x, borders.minX, borders.maxX),
                    Mathf.Clamp(cacheTransform.position.y, borders.minY, borders.maxY),
                0
                );
        }
    }

    public void OnEndDrag(PointerEventData data){
        setOffest = false;
    }

    //setting 'Player's' movement borders according to Viewport size and defined offset
    void ResizeBorders() 
    {
        Vector2 worldZero = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 worldRight = mainCamera.ViewportToWorldPoint(Vector2.right);
        float minX = Mathf.Max(worldZero.x,-maxBorderX);
        float maxX = Mathf.Min(worldRight.x, maxBorderX);

        borders.minX = minX + borders.minXOffset;
        borders.minY = worldZero.y + borders.minYOffset;
        borders.maxX = maxX - borders.maxXOffset;
        borders.maxY = mainCamera.ViewportToWorldPoint(Vector2.up).y - borders.maxYOffset;
    }
}
