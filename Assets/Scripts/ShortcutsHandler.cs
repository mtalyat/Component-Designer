using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutsHandler : MonoBehaviour
{
    public static ShortcutsHandler instance;

    public bool Ctrl { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Ctrl = Input.GetKey(KeyCode.LeftControl);
        
        if(Ctrl)
        {
            //shortcuts with control
            if(Input.GetKeyDown(KeyCode.S))
            {
                GameManager.instance.SaveLoadedData();
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                GameManager.instance.CreateNewComponent();
            }

            if(Input.GetKeyDown(KeyCode.A))
            {
                ObjectBuilder.instance.SelectAll();
            }

            if(Input.GetKeyDown(KeyCode.D))
            {
                ObjectBuilder.instance.DuplicateSelected();
            }
        }
        else
        {
            //shortcuts without control

            if (Input.GetKeyDown(KeyCode.R))//reset camera
            {
                CameraController.instance.ResetController();
            }

            if (Input.GetKeyDown(KeyCode.Delete))//delete currently selected peg
            {
                ObjectBuilder.instance.DeleteSelected();
            }

            if(Input.GetKeyDown(KeyCode.Z))
            {
                SelectionTools.instance.SetMode(SelectionTools.SelectionMode.Position);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                SelectionTools.instance.SetMode(SelectionTools.SelectionMode.Rotation);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SelectionTools.instance.SetMode(SelectionTools.SelectionMode.Scale);
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                CameraController.instance.FocusOnSelected();
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.ShowMenu();
            }
        }
    }
}
