public struct Edge {
    public Vertex a;
    public Vertex b;
    public bool Equals(Edge other){return a.Equals(other.a) && b.Equals(other.b);}
}