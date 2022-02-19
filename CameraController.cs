using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private float maxDist;
    private void Update()
    {
        cameraFollow.transform.position = GetMouseWorldPos()/2;
    }
    private Vector2 GetMouseWorldPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 resVec = new Vector2(Mathf.Clamp(mousePos.x, -maxDist, maxDist), Mathf.Clamp(mousePos.y, -maxDist, maxDist));
        return resVec;
    }
}
