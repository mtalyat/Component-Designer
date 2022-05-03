using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ObjectInteractionHandler : MonoBehaviour
{
    private Camera cam;

    private int componentLayer;
    private int arrowsLayer;

    public float raycastDistance = 20f;

    private Transform clickedArrow;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        componentLayer = LayerMask.GetMask("Component Object");
        arrowsLayer = LayerMask.GetMask("Selection Tools");
    }

    private void Update()
    {
        if (CameraController.instance.isMouseOverUI) return;

        if (Input.GetMouseButtonUp(0))//left
        {
            clickedArrow = null;
        }

        if (clickedArrow != null && Input.GetMouseButton(0))//left click again, held
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //find closest position on the ray from the camera
            Vector3 lineTarget = Helper.FindNearestPointOnLine(ray.origin, ray.direction, clickedArrow.position);

            SelectionTools.instance.Input(lineTarget);
        }

        if (Input.GetMouseButtonDown(0))//left click
        {
            //first, check for arrows
            //then components

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, arrowsLayer))
            {
                //we hit an arrow
                //now determine what to do based on the arrow hit
                clickedArrow = hit.transform;
                SelectionTools.instance.SetAxis(clickedArrow.GetComponent<SelectionTool>().axis);
                SelectionTools.instance.StartInput(Helper.FindNearestPointOnLine(ray.origin, ray.direction, clickedArrow.position));
            }
            else if (Physics.Raycast(ray, out hit, 100f, componentLayer))
            {
                //we hit a child, since they are all offset from the parent and the parent is invisible
                BlockDataObject bdo = hit.transform.GetComponentInParent<BlockDataObject>();

                //select it
                BlockDataObject.Select(bdo, ShortcutsHandler.instance.Ctrl);
            }
            else
            {
                //if nothing hit, unselect
                BlockDataObject.Select(null);
            }
        }
    }
}
