using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDirectionalArrows : MonoBehaviour
{
    private void LateUpdate()
    {
        //at any given time, the rotation for this object should be (0, 0, 0), relative to world space
        transform.rotation = Quaternion.identity;
    }
}
