using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using Cinemachine;


public class FreeLookCameraControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 _playerTouchVectorOutput;
    private bool _isPlayerTouchingPanel;
    private Touch _myTouch;
    private int _touchID;
    private Vector2 _lookInput;
    [SerializeField] private float _touchSpeedSensitivity ;
    //[SerializeField] private float _touchSpeedSensitivity = 3f;
    [SerializeField] private SettingManager settingManager;
    private string _touchXMapTo = "Mouse X";
    private string _touchYMapTo = "Mouse Y";
    void Start()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
        _touchSpeedSensitivity = settingManager.slider.value;

    }

    private float GetInputAxis(string axisName)
    {
        _lookInput = PlayerJoystickOutputVector();

        if (axisName == _touchXMapTo)
            return _lookInput.x * _touchSpeedSensitivity;

        if (axisName == _touchYMapTo)

            return _lookInput.y * _touchSpeedSensitivity;


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
        _touchSpeedSensitivity = settingManager.slider.value;
        OnDrag(_onPointerDownData);
        _touchID = _myTouch.fingerId;
        _isPlayerTouchingPanel = true;
      
    }
    private void FixedUpdate()
    {


        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                _myTouch = Input.GetTouch(i);
                if (_isPlayerTouchingPanel)
                {
                    if (_myTouch.fingerId == _touchID)
                    {
                        if (_myTouch.phase != TouchPhase.Moved)
                            OutputVectorValue(Vector2.zero);
                    }
                }
            }
        }

    }
    public void OnDrag(PointerEventData _onDragData)
    {
        OutputVectorValue(new Vector2(_onDragData.delta.normalized.x, _onDragData.delta.normalized.y));
    }
    //public Image image;
    //[SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    //private string _touchXMapTo = "Mouse X";
    //private string _touchYMapTo = "Mouse Y";
    //public void OnPointerUp(PointerEventData _onPointerUpData)
    //{
    //    cinemachineFreeLook.m_XAxis.m_InputAxisName = null;
    //    cinemachineFreeLook.m_YAxis.m_InputAxisName = null;
    //    cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0;
    //    cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0;
    //}

    //public void OnPointerDown(PointerEventData _onPointerDownData)
    //{
    //    OnDrag(_onPointerDownData);

    //}

    //public void OnDrag(PointerEventData _onDragData)
    //{
    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, _onDragData.position, _onDragData.enterEventCamera, out Vector2 posOut))
    //    {
    //        cinemachineFreeLook.m_XAxis.m_InputAxisName = _touchXMapTo;
    //        cinemachineFreeLook.m_YAxis.m_InputAxisName = _touchYMapTo;
    //    }
    //}

}



