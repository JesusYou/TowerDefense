using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    public enum ControlType
    {
        WidthParam,
        HeightParam
    }

    public ControlType controlType;
    //相机自适应对象
    public SpriteRenderer focusObject;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public float dragSpeed = 0.5f;
    private float minX, maxX, minY, maxY;
    private float moveX, moveY;
    private Camera _camera;
    private float originAspectRatio;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        originAspectRatio = _camera.aspect;
        maxX = focusObject.bounds.max.x;
        minX = focusObject.bounds.min.x;
        maxY = focusObject.bounds.max.y;
        minY = focusObject.bounds.min.y;
        UpdateCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        //比例发生改变
        if (originAspectRatio != _camera.aspect)
        {
            UpdateCameraSize();
            originAspectRatio = _camera.aspect;
        }

        //水平移动相机
        if (moveX != 0f)
        {
            if (controlType == ControlType.HeightParam)
            {
                bool permit = false;
                //获取右移许可
                if (moveX > 0f)
                {
                    if (_camera.transform.position.x + (_camera.orthographicSize * _camera.aspect) < maxX - offsetX)
                    {
                        permit = true;
                    }
                }
                //获取左移许可
                else
                {
                    if (_camera.transform.position.x - (_camera.orthographicSize * _camera.aspect) > minX + offsetX)
                    {
                        permit = true;
                    }
                }

                if (permit == true)
                {
                    transform.Translate(Vector3.right * moveX * dragSpeed, Space.World);
                }
            }

            moveX = 0f;
        }

        //垂直移动相机
        if (moveY != 0f)
        {
            if (controlType == ControlType.WidthParam)
            {
                bool permit = false;
                //获取上移许可
                if (moveY > 0f)
                {
                    if (_camera.transform.position.y + _camera.orthographicSize < maxY - offsetY)
                    {
                        permit = true;
                    }
                }
                //获取下移许可
                else
                {
                    if (_camera.transform.position.y - _camera.orthographicSize > minY + offsetY)
                    {
                        permit = true;
                    }
                }

                if (permit == true)
                {
                    transform.Translate(Vector3.up * moveY * dragSpeed, Space.World);
                }
            }

            moveY = 0f;
        }
    }
    
    public void MoveX(float distance)
    {
        moveX = distance;
    }

    public void MoveY(float distance)
    {
        moveY = distance;
    }
    
    //更新相机大小适应焦点对象
    private void UpdateCameraSize()
    {
        switch (controlType)
        {
            case ControlType.WidthParam:
                _camera.orthographicSize = (maxX - minX - 2 * offsetX) / (2f * _camera.aspect);
                break;
            case ControlType.HeightParam:
                _camera.orthographicSize = (maxY - minY - 2 * offsetY) / 2f;
                break;
        }
    }
}
