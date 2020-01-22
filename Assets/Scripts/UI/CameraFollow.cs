using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //카메라가 움직임을 시작하는 거리 X
    public float xMargin = 2f;
    //카메라가 움직임을 시작하는 거리 Y
    public float yMargin = 2f;
    //값이 부드럽게 움직이는 정도(작을수록 부드럽다)
    public float xSmooth = 2f;
    public float ySmooth = 2f;

    public Vector2 maxXAndY; //카메라가 가질 수 있는 최대 X,Y값
    public Vector2 minXAndY; //카메라가 가질 수 있는 최소 X,Y값

    public Transform player; //카메라가 따라다닐 플레이어

    private bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    private bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    private void TrackPlayer()
    {
        //카메라가 이동할 위치
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
        {
            targetX = Mathf.Lerp(
                transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        }

        if (CheckYMargin())
        {
            targetY = Mathf.Lerp(
                transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);


        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    private void FixedUpdate()
    {
        TrackPlayer();
    }
}

