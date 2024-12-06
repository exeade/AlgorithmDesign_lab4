namespace lab4;

public class Ant
{
    public int CurrentVertex { get; private set; }
    public Path Path { get; }
    
    private readonly Graph _graph;
    
    private readonly bool _isWild;
    
    public Ant(Graph graph, int startVertex, bool isWild = false)
    {
        _graph = graph;
        CurrentVertex = startVertex;
        Path = new Path();
        Path.AddVertex(startVertex);
        _isWild = isWild;
    }

    public int ChooseNextVertex(bool[] visited)
    {
        double[] probabilities = new double[_graph.NumberOfVertices];
        double totalProbability = 0.0;
        
        if (_isWild)
        {
            Random randVert = new Random();
            int nextVertex;
            do
            {
                nextVertex = randVert.Next(_graph.NumberOfVertices);
            } while (visited[nextVertex] || nextVertex == CurrentVertex);
            return nextVertex;
        }

        for (int i = 0; i < _graph.NumberOfVertices; i++)
        {
            if (!visited[i] && CurrentVertex != i)
            {
                probabilities[i] = _graph.GetTransitionProbability(CurrentVertex, i);
                totalProbability += probabilities[i];
            }
            else
            {
                probabilities[i] = 0.0;
            }
        }
        
        for (int i = 0; i < probabilities.Length; i++)
        {
            if (probabilities[i] > 0)
            {
                probabilities[i] /= totalProbability;
            }
        }
        
        double[] intervals = new double[_graph.NumberOfVertices];
        double cumulativeProbability = 0.0;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            intervals[i] = cumulativeProbability;
        }
        
        Random rand = new Random();
        double randomValue = rand.NextDouble();

        for (int i = 0; i < intervals.Length; i++)
        {
            if (randomValue < intervals[i])
            {
                return i;
            }
        }
        
        return -1;
    }
    
    public void MoveToNextVertex(int nextVertex)
    {
        Path.AddVertex(nextVertex, _graph.Distances[CurrentVertex, nextVertex]);
        CurrentVertex = nextVertex;
    }

    public void Reset()
    {
        Path.Clear();
        Path.AddVertex(CurrentVertex);
    }
}