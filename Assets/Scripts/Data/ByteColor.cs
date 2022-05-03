using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteColor
{
    public byte r;
    public byte g;
    public byte b;

    public ByteColor()
    {
        r = 0;
        g = 0;
        b = 0;
    }

    public ByteColor(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public ByteColor(Vector3 v)
    {
        r = (byte)v.x;
        g = (byte)v.y;
        b = (byte)v.z;
    }

    public ByteColor(Color c)
    {
        r = (byte)(c.r * 255);
        g = (byte)(c.g * 255);
        b = (byte)(c.b * 255);
    }

    public Color ToColor()
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(r, g, b);
    }

    public override string ToString()
    {
        return $"(R: {r}, G: {g}, B: {b})";
    }
}
