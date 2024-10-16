public struct Vertex {
    public double x;
    public double y;

    public Vertex(double x, double y) {
        this.x = x;
        this.y = y;
    }
    public bool Equals(Vertex other){return x == other.x && y == other.y;}
}