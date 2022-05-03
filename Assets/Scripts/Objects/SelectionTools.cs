using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SelectionTools : MonoBehaviour
{
    public enum Axis : int
    {
        X,
        Y,
        Z
    }

    public enum SelectionMode : int
    {
        Position,
        Rotation,
        Scale
    }

    public static SelectionTools instance;

    private List<BlockDataObject> matching = new List<BlockDataObject>();
    private List<Vector3> matchingOffsets = new List<Vector3>();
    private List<Quaternion> matchingRotations = new List<Quaternion>();
    private Transform following;

    public bool isScalable => matching.Count == 1 && matching[0].scalable;

    public bool isMatching => matching.Any();

    private SelectionMode mode = SelectionMode.Position;
    private Axis axis = Axis.Y;

    private Transform selectedTool;

    [SerializeField]
    private float scaleScale = 0.2f;

    public bool isHidden { get; private set; }

    private float startValue;
    private Vector3 startVector;

    [Header("Rounding")]

    [SerializeField]
    private float positionRounding = 0.25f;

    [SerializeField]
    private float rotationRounding = 22.5f;

    [SerializeField]
    private float scaleRounding = 0.25f;

    [Header("UI References")]
    [SerializeField] private UnityEngine.UI.Button positionButton;
    [SerializeField] private UnityEngine.UI.Button rotationButton;
    [SerializeField] private UnityEngine.UI.Button scaleButton;
    [SerializeField] private UnityEngine.UI.Button duplicateButton;
    [SerializeField] private UnityEngine.UI.Button deleteButton;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (following != null)
        {
            transform.position = following.position;
            transform.rotation = following.rotation;
        }

        //keep same size regardless of zoom
        float size = (CameraController.instance.transform.position - transform.position).magnitude;
        transform.localScale = Vector3.one * size * scaleScale;
    }

    public void SetMatch(BlockDataObject obj)
    {
        RemoveAllMatches();

        AddMatch(obj);
    }

    public void AddMatch(BlockDataObject obj)
    {
        if(!matching.Any())
        {
            //first one
            following = obj.transform;

            Show();
        } else
        {
            if(mode == SelectionMode.Scale)
            {
                //not first one
                SetMode(SelectionMode.Position);
            }
        }

        matching.Add(obj);
        matchingOffsets.Add(obj.transform.position - following.transform.position);
        matchingRotations.Add(obj.transform.rotation);

        SetButtonInteractables();
    }

    public void RemoveMatch(BlockDataObject obj)
    {
        int index = matching.IndexOf(obj);

        if (index == -1) return;

        matching.RemoveAt(index);
        matchingOffsets.RemoveAt(index);
        matchingRotations.RemoveAt(index);

        if(!matching.Any())
        {
            //last one
            following = null;

            Hide();
        }

        SetButtonInteractables();
    }

    public void RemoveAllMatches()
    {
        matching.Clear();
        matchingOffsets.Clear();
        matchingRotations.Clear();

        following = null;

        Hide();

        SetButtonInteractables();
    }

    private void MoveTo(Vector3 target)
    {
        if (!UnityEngine.Input.GetKey(KeyCode.LeftControl))
        {
            target = Helper.RoundToNearest(target, positionRounding);
        }

        for (int i = 0; i < matching.Count; i++)
        {
            matching[i].transform.position = target + matchingOffsets[i];
        }
    }

    private void RotateBy(float angle)
    {
        for (int i = 0; i < matching.Count; i++)
        {
            //set angle
            switch (axis)
            {
                case Axis.X:
                    matching[i].transform.localRotation = matchingRotations[i] * Quaternion.AngleAxis(angle, Vector3.right);
                    break;
                case Axis.Y:
                    matching[i].transform.localRotation = matchingRotations[i] * Quaternion.AngleAxis(angle, Vector3.up);
                    break;
                case Axis.Z:
                    matching[i].transform.localRotation = matchingRotations[i] * Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
            }
            matchingRotations[i] = Quaternion.Euler(matching[i].transform.localEulerAngles);
        }
    }

    private float GetRotation(Vector3 target)
    {
        //get angle to target
        Vector2 target2d;
        switch (axis)
        {
            case Axis.X:
                target2d = new Vector2(target.y - following.position.y, target.z - following.position.z);
                break;
            case Axis.Y:
                target2d = new Vector2(target.z - following.position.z, target.x - following.position.x);
                break;
            case Axis.Z:
                target2d = new Vector2(target.x - following.position.x, target.y - following.position.y);
                break;
            default:
                target2d = Vector2.zero;
                break;
        }

        float angle = Mathf.Atan2(target2d.y, target2d.x) * Mathf.Rad2Deg;

        if (!UnityEngine.Input.GetKey(KeyCode.LeftControl))
        {
            angle = Helper.RoundToNearest(angle, rotationRounding);
        }

        return angle;
    }

    private void ScaleTo(float scale)
    {
        if (!UnityEngine.Input.GetKey(KeyCode.LeftControl))
        {
            scale = Helper.RoundToNearest(scale, scaleRounding);
        }

        switch (axis)
        {
            case Axis.X:
                following.localScale = startVector + new Vector3(scale, 0f, 0f);
                break;
            case Axis.Y:
                following.localScale = startVector + new Vector3(0f, scale, 0f);
                break;
            case Axis.Z:
                following.localScale = startVector + new Vector3(0f, 0f, scale);
                break;
        }
    }

    private float GetScale(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) * 2f;
    }

    public void StartInput(Vector3 mouseWorldPos)
    {
        if (mode == SelectionMode.Position)
        {
            startVector = Helper.FindNearestPointOnLine(transform.position, selectedTool.up, mouseWorldPos) - following.position;
        }
        else if (mode == SelectionMode.Rotation)
        {
            startVector = matching[0].transform.eulerAngles;
            startValue = GetRotation(mouseWorldPos);
        }
        else if (mode == SelectionMode.Scale)
        {
            startVector = matching[0].transform.localScale;
            startValue = GetScale(mouseWorldPos);
        }
    }

    public void Input(Vector3 mouseWorldPos)
    {
        if (mode == SelectionMode.Position)
        {
            Vector3 target = Helper.FindNearestPointOnLine(transform.position, selectedTool.up, mouseWorldPos);

            MoveTo(target - startVector);
        }
        else if (mode == SelectionMode.Rotation)
        {
            float newRotation = GetRotation(mouseWorldPos);
            RotateBy(newRotation - startValue);
            startValue = newRotation;
        }
        else if (mode == SelectionMode.Scale)
        {
            ScaleTo(GetScale(mouseWorldPos) - startValue);
        }

        foreach(BlockDataObject o in matching)
        {
            o.UpdateData();
        }
    }

    public void SetMode(int i) => SetMode((SelectionMode)i);

    public void SetMode(SelectionMode m)
    {
        if(isHidden)
        {
            mode = m;
        } else
        {
            //visible
            //check for invalid tools for the items
            if (!isScalable && m == SelectionMode.Scale)
            {
                m = SelectionMode.Position;
            }

            //otherwise just set it
            mode = m;

            //refresh view
            Show();
        }

        selectedTool = GetAxisTool(axis);
    }

    public void SetAxis(Axis a)
    {
        axis = a;

        selectedTool = GetAxisTool(a);
    }

    public void Show()
    {
        if (!isHidden)
        {
            Hide();
        }

        //ensure that the mode isn't invalid
        if(!isScalable && mode == SelectionMode.Scale)
        {
            mode = SelectionMode.Position;
            selectedTool = GetAxisTool(axis);
        }

        GetTool(mode).gameObject.SetActive(true);

        isHidden = false;
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GetTool(i).gameObject.SetActive(false);
        }

        isHidden = true;

        SetButtonInteractables();
    }

    private Transform GetTool(int index)
    {
        return transform.GetChild(index);
    }

    private Transform GetTool(SelectionMode mode) => GetTool((int)mode);

    private Transform GetAxisTool(Axis axis)
    {
        return GetTool(mode).GetChild((int)axis);
    }

    private void SetButtonInteractables()
    {
        if(!isMatching)
        {
            //if no object selected, nothing can be done
            positionButton.interactable = false;
            rotationButton.interactable = false;
            scaleButton.interactable = false;
            duplicateButton.interactable = false;
            deleteButton.interactable = false;
            return;
        }

        //always interactable when anything is selected
        positionButton.interactable = true;
        deleteButton.interactable = true;
        rotationButton.interactable = true;
        duplicateButton.interactable = true;

        //others are different
        scaleButton.interactable = isScalable;
    }
}
