using UnityEngine;

//delaunay : https://www.gorillasun.de/blog/bowyer-watson-algorithm-for-delaunay-triangulation/
public class Triangle {
    public Vertex a;
    public Vertex b;
    public Vertex c;
    public Triangle(Vertex a,Vertex b,Vertex c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }
    public Circumcircle MyCircle(Vertex a,Vertex b,Vertex c) {
        Point A = new Point(a.x, a.y);
        Point B = new Point(b.x, b.y);
        Point C = new Point(c.x, c.y);
        Circumcircle circle = Helper.GetCircumcircle(A, B, C);
        return circle;
    }
    public bool InCircumCircle(Vertex v) {
        Point V = new Point(v.x, v.y);
        var dx = MyCircle(a, b, c).Center.X - V.X;
        var dy = MyCircle(a, b, c).Center.Y - V.Y;
        return Mathf.Sqrt((float)(dx * dx + dy * dy)) <= MyCircle(a, b, c).Radius;
    }
}