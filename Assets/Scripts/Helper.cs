using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {
    public static Circumcircle GetCircumcircle(Point A, Point B, Point C) {
        double D = 2 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));
        
        if (Math.Abs(D) < 1e-10)
        {
            throw new InvalidOperationException("Les points sont colinéaires.");
        }

        double Ux = ((A.X * A.X + A.Y * A.Y) * (B.Y - C.Y) +
                     (B.X * B.X + B.Y * B.Y) * (C.Y - A.Y) +
                     (C.X * C.X + C.Y * C.Y) * (A.Y - B.Y)) / D;

        double Uy = ((A.X * A.X + A.Y * A.Y) * (C.X - B.X) +
                     (B.X * B.X + B.Y * B.Y) * (A.X - C.X) +
                     (C.X * C.X + C.Y * C.Y) * (B.X - A.X)) / D;

        double radius = Math.Sqrt((Ux - A.X) * (Ux - A.X) + (Uy - A.Y) * (Uy - A.Y));

        return new Circumcircle(new Point(Ux, Uy), radius);
    }
}
