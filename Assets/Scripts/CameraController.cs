using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera cam;

    [Header("Input")]

    public float rotateSpeedKeyPress = 100f;

    [Space]

    public float moveSpeedKeyPress = 1f;
    public float moveSpeedDrag = 0.01f;

    [Space]

    public float zoomSpeedKeyPress = 10f;
    public float zoomSpeedScroll = 100f;
    public float minZoomDistance = 1f;

    private Vector3 startingPos;
    private Vector3 startingCameraRot;

    private int mouseDragState = -1;
    private bool isDragging => mouseDragState >= 0;
    private Vector3 lastDragPos = Vector3.zero;

    private bool isOverUI = false;

    public bool isMouseOverUI => isOverUI;

    private int UILayer;

    private float pitch = 0f;
    private float yaw = 0f;

    [SerializeField]
    private Vector2 mouseSensitivity = Vector2.one;

    private const float MAX_PITCH = 90f;

    private bool autoCamera = true;
    private Transform lookingAt = null;
    [SerializeField]
    private float autoCameraSpeed = 1f;

    private void Awake()
    {
        instance = this;

        startingPos = transform.position;
        startingCameraRot = cam.transform.localEulerAngles;
        SetPitchAndYawToCameraAngles();
    }

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        //only do mouse movement when not over UI
        isOverUI = IsPointerOverUIElement();

        //only move the camera if we are in that frame
        //we still need to do checks for when moving/letting go though

        ControlCamera();

        if(autoCamera)
        {
            Vector3 lookingPos = lookingAt?.position ?? Vector3.zero;

            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.LookRotation(lookingPos - transform.position), autoCameraSpeed * Time.deltaTime);

            SetPitchAndYawToCameraAngles();
        }
    }

    private void SetPitchAndYawToCameraAngles()
    {
        pitch = cam.transform.localEulerAngles.x;
        yaw = cam.transform.localEulerAngles.y;
    }

    #region Pointer Over UI

    //following 3 methods: https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    #endregion

    private void ControlCamera()
    {
        RotationUpdate();
        ViewUpdate();
        ZoomUpdate();
        FirstPersonUpdate();
    }

    private void FirstPersonUpdate()
    {
        //first person, dragging to look around
        if (Input.GetMouseButtonUp(1) && mouseDragState == 1)//right click release
        {
            mouseDragState = -1;
        }

        //dragging
        if (mouseDragState == 1)
        {
            //independant of Time.deltaTime, since it is moving directly with the mouse
            yaw += mouseSensitivity.x * Input.GetAxis("Mouse X");
            pitch = Mathf.Clamp(pitch - mouseSensitivity.y * Input.GetAxis("Mouse Y"), -MAX_PITCH, MAX_PITCH);

            cam.transform.localEulerAngles = new Vector3(pitch, yaw, 0f);
        }

        if (Input.GetMouseButtonDown(1) && !isDragging && !isOverUI)//right click
        {
            mouseDragState = 1;
            lastDragPos = Input.mousePosition;
            autoCamera = false;
        }
    }

    private void ZoomUpdate()
    {
        if (isOverUI) return;

        float zoom = 0f;

        if (!ShortcutsHandler.instance.Ctrl && Input.GetKey(KeyCode.W)) // zoom in
        {
            zoom += zoomSpeedKeyPress * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom += zoomSpeedScroll * Time.deltaTime;
        }
        if (!ShortcutsHandler.instance.Ctrl && Input.GetKey(KeyCode.S)) // zoom out
        {
            zoom -= zoomSpeedKeyPress * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom -= zoomSpeedScroll * Time.deltaTime;
        }

        transform.position += cam.transform.forward * zoom;
    }

    private void ViewUpdate()
    {
        Vector3 position = Vector3.zero;

        if (Input.GetMouseButtonUp(2) && mouseDragState == 2)//middle click release
        {
            mouseDragState = -1;
        }

        //dragging
        if(mouseDragState == 2)
        {
            //independant of Time.deltaTime, since it is moving directly with the mouse
            position -= (Input.mousePosition - lastDragPos) * moveSpeedDrag;
            lastDragPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(2) && !isDragging && !isOverUI)//middle click
        {
            mouseDragState = 2;
            lastDragPos = Input.mousePosition;
            autoCamera = false;
        }

        if (isOverUI) return;

        if (Input.GetKey(KeyCode.LeftArrow)) // move left
        {
            position += new Vector3(-moveSpeedKeyPress * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow)) // move right
        {
            position += new Vector3(moveSpeedKeyPress * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow)) // move up
        {
            position += new Vector3(0f, moveSpeedKeyPress * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.DownArrow)) // move down
        {
            position += new Vector3(0f, -moveSpeedKeyPress * Time.deltaTime, 0f);
        }

        transform.position += cam.transform.right * position.x;
        transform.position += cam.transform.up * position.y;
    }

    private void RotationUpdate()
    {
        if (isOverUI) return;

        Vector3 rotation = Vector3.zero;

        if (!ShortcutsHandler.instance.Ctrl && Input.GetKey(KeyCode.A)) // spin left
        {
            rotation += new Vector3(0f, rotateSpeedKeyPress * Time.deltaTime, 0f);
        }
        if (!ShortcutsHandler.instance.Ctrl && Input.GetKey(KeyCode.D)) // spin right
        {
            rotation += new Vector3(0f, -rotateSpeedKeyPress * Time.deltaTime, 0f);
        }

        //rotate around selection tools if they are following something, otherwise rotate around the center
        if (SelectionTools.instance.isMatching)
        {
            transform.RotateAround(SelectionTools.instance.transform.position, Vector3.up, rotation.y);
            transform.RotateAround(SelectionTools.instance.transform.position, Vector3.right, rotation.x);
        }
        else
        {
            transform.RotateAround(Vector3.zero, Vector3.up, rotation.y);
            transform.RotateAround(Vector3.zero, Vector3.right, rotation.x);
        }
        
    }

    public void ResetController()
    {
        transform.position = startingPos;
        cam.transform.eulerAngles = startingCameraRot;
    }

    public void FocusOnSelected()
    {
        if(SelectionTools.instance.isMatching)
        {
            lookingAt = SelectionTools.instance.transform;
        } else
        {
            //if nothing selected, look at world origin
            lookingAt = null;
        }

        autoCamera = true;
    }
}
