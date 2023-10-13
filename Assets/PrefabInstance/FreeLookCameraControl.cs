using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using Cinemachine;


public class FreeLookCameraControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static FreeLookCameraControl ins;
    public CameraWhenShootLaser cameraWhenShootLaser;
    private Vector2 _playerTouchVectorOutput;
    private bool _isPlayerTouchingPanel;
    private Vector2 touchCurrentPosition;
    private Vector2 _lookInput;
   public float _touchSpeedSensitivity ;
    private float _speedRotate;
    [SerializeField] private SettingManager settingManager;
    private string _touchXMapTo = "Mouse X";
    private string _touchYMapTo = "Mouse Y";
    public CinemachineFreeLook freeLookCam;
    public int touchID;
    public float _smoothingFactor=2;
    public bool isTargetHeading;

    void Start()
    {
        ins = this;
        _touchSpeedSensitivity = settingManager.slider.value;
        CinemachineCore.GetInputAxis = GetInputAxis;
        

    }
   
    private float GetInputAxis(string axisName)
    {
        _lookInput = PlayerJoystickOutputVector();

        if (axisName == _touchXMapTo)
            return _lookInput.x * _speedRotate;

        if (axisName == _touchYMapTo)

            return _lookInput.y * _speedRotate;


        return Input.GetAxis(axisName);
    }

    private void OutputVectorValue(Vector2 outputValue)
    {
        _playerTouchVectorOutput = outputValue;
    }

    public Vector2 PlayerJoystickOutputVector()
    {
        return _playerTouchVectorOutput;
    }

    public void OnPointerUp(PointerEventData _onPointerUpData)
    {
        OutputVectorValue(Vector2.zero);
        _isPlayerTouchingPanel = false;
        
    }

    public void OnPointerDown(PointerEventData _onPointerDownData)
    {
        OnDrag(_onPointerDownData);
        _isPlayerTouchingPanel = true;
        touchCurrentPosition = _onPointerDownData.position;
        TargetHeading(false, 1, 1);
    }
    private void Update()
    {
        SetMousePostion();
        if (_isPlayerTouchingPanel && Input.touchCount > 0)
        {

            Vector2 newtouchPosition = Input.GetTouch(touchID).position;
            _speedRotate = Vector2.Distance(newtouchPosition, touchCurrentPosition) * _touchSpeedSensitivity;
            touchCurrentPosition = newtouchPosition;
            if (Input.GetTouch(touchID).phase == TouchPhase.Moved) return;
            OutputVectorValue(Vector2.zero);
        }
        else
        {
            _speedRotate = 1;
        }
    }
    public void SetMousePostion()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            touchID = 0;
        }
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).position.x > Input.GetTouch(1).position.x)
            {
                touchID = 0;
            }
            else
            {
                touchID = 1;
            }
        }
    }

    public void OnDrag(PointerEventData _onDragData)
    {
        Vector2 rawInput = new Vector2(_onDragData.delta.normalized.x, _onDragData.delta.normalized.y);
        OutputVectorValue(Vector2.Lerp(_playerTouchVectorOutput, rawInput, _smoothingFactor));
    }
    public void TargetHeading(bool enabled , float waitTime=1, float recenteringTime=1 )
    {
        freeLookCam.m_RecenterToTargetHeading.m_enabled = enabled;
        freeLookCam.m_YAxisRecentering.m_enabled = enabled;
        isTargetHeading = enabled;
        freeLookCam.m_RecenterToTargetHeading.m_WaitTime = Mathf.Clamp(waitTime, 0.5f, waitTime);
        freeLookCam.m_RecenterToTargetHeading.m_RecenteringTime = Mathf.Clamp(recenteringTime, 0.5f, recenteringTime);
        freeLookCam.m_YAxisRecentering.m_WaitTime = Mathf.Clamp(waitTime, 0.5f, waitTime);
        freeLookCam.m_YAxisRecentering.m_RecenteringTime = Mathf.Clamp(recenteringTime, 0.5f, recenteringTime);
    }
   
}



