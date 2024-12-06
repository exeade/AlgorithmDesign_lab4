namespace lab4;

public class Path
{
    public List<int> Vertices { get; } = new();
    public double Length { get; set; }

    public void AddVertex(int vertex, double distance = 0.0)
    {
        if (Vertices.Count > 0)
        {
            Length += distance;
        }
        Vertices.Add(vertex);
    }

    public void Clear()
    {
        Vertices.Clear();
        Length = 0.0;
    }

    public bool ContainsEdge(int i, int j)
    {
        for (int k = 0; k < Vertices.Count - 1; k++)
        {
            if ((Vertices[k] == i && Vertices[k + 1] == j) || 
                (Vertices[k] == j && Vertices[k + 1] == i))
            {
                return true;
            }
        }
        return false;
    }
}
