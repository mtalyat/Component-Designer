using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class ThemeColorSetter : MonoBehaviour
//{
//    private static List<ThemeColorSetter> setters = new List<ThemeColorSetter>();

//    private Image image;

//    [SerializeField]
//    private int colorIndex = 0;

//    private void Awake()
//    {
//        image = GetComponent<Image>();

//        setters.Add(this);
//    }

//    private void SetColor()
//    {
//        image.color = Settings.instance.themeColors[colorIndex];
//    }

//    public static void SetColors()
//    {
//        foreach(ThemeColorSetter setter in setters)
//        {
//            setter.SetColor();
//        }
//    }
//}
