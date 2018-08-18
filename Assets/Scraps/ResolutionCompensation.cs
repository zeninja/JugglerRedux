using UnityEngine;
using System.Collections;

public class ResolutionCompensation : MonoBehaviour
{
    public static Vector2 WorldUnitsInCamera;
    public static Vector2 WorldToPixelAmount;

    public Camera m_Camera;

    void Awake()
    {
		m_Camera = Camera.main;
        //Finding Pixel To World Unit Conversion Based On Orthographic Size Of Camera
        WorldUnitsInCamera.y = m_Camera.orthographicSize * 2;
        WorldUnitsInCamera.x = WorldUnitsInCamera.y * Screen.width / Screen.height;

        WorldToPixelAmount.x = Screen.width / WorldUnitsInCamera.x;
        WorldToPixelAmount.y = Screen.height / WorldUnitsInCamera.y;
    }


    //Taking Your Camera Location And Is Off Setting For Position And For Amount Of World Units In Camera
    public Vector2 ConvertToWorldUnits(Vector2 TouchLocation)
    {
        Vector2 returnVec2;

        returnVec2.x = ((TouchLocation.x / WorldToPixelAmount.x) - (WorldUnitsInCamera.x / 2)) +
        m_Camera.transform.position.x;
        returnVec2.y = ((TouchLocation.y / WorldToPixelAmount.y) - (WorldUnitsInCamera.y / 2)) +
        m_Camera.transform.position.y;

        return returnVec2;
    }
}