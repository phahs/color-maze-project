using UnityEngine;
using System.Collections;

public class Coordinates
{
    public int x, z;

    public Coordinates()
    {
        x = 0;
        z = 0;
    }

    public Coordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static Coordinates operator + (Coordinates a, Coordinates b)
    {
        a.x += b.x;
        a.z += b.z;

        return a;
    }

    public static Coordinates operator - (Coordinates a, Coordinates b)
    {
        a.x -= b.x;
        a.z -= b.z;

        return a;
    }

    public float magnitudeSqrd(Coordinates near, Coordinates far)
    {
        Coordinates temp = far - near;
        float mag = Mathf.Pow(temp.x, 2) + Mathf.Pow(temp.z, 2);

        return mag;
    }
}
