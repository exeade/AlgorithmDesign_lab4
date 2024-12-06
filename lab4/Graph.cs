namespace lab4;

public class Graph
{
    public int[,] Distances { get; }
    private double[,] Pheromones { get; }
    public int NumberOfVertices { get; }
    private double Lmin { get; }
    private int Alpha { get; }
    private int Beta { get; }
    private double EvaporationRate { get; }
    public Graph(int numberOfVertices, int alpha, int beta, double evaporationRate)
    {
        NumberOfVertices = numberOfVertices;
        Alpha = alpha;
        Beta = beta;
        EvaporationRate = evaporationRate;
        Distances = new int[NumberOfVertices, NumberOfVertices];
        Pheromones = new double[NumberOfVertices, NumberOfVertices];
        
        InitializeGraph();
        Lmin = FindLmin();
    }

    private void InitializeGraph()
    {
        Random rand = new Random();

        for (int i = 0; i < NumberOfVertices; i++)
        {
            for (int j = i + 1; j < NumberOfVertices; j++)
            {
                int distance = rand.Next(1, 41);
                Distances[i, j] = distance;
                Distances[j, i] = distance;
                
                Pheromones[i, j] = 0.2;
                Pheromones[j, i] = 0.2;
            }
        }
    }

    private double FindLmin()
    {
        bool[] visited = new bool[NumberOfVertices];
        double totalDistance = 0;
        int currentVertex = 0;
        visited[currentVertex] = true;
        int visitedCount = 1;

        while (visitedCount < NumberOfVertices)
        {
            double minDistance = double.MaxValue;
            int nextVertex = -1;
            
            for (int i = 0; i < NumberOfVertices; i++)
            {
                if (!visited[i] && Distances[currentVertex, i] < minDistance)
                {
                    minDistance = Distances[currentVertex, i];
                    nextVertex = i;
                }
            }
            
            totalDistance += minDistance;
            visited[nextVertex] = true;
            currentVertex = nextVertex;
            visitedCount++;
        }

        return totalDistance;
    }
    
    public double GetTransitionProbability(int currentVertex, int nextVertex)
    {
        double pheromoneLevel = Pheromones[currentVertex, nextVertex];
        double distance = Distances[currentVertex, nextVertex];
        double heuristic = 1.0 / distance;
        return Math.Pow(pheromoneLevel, Alpha) * Math.Pow(heuristic, Beta);
    }
    
    public void UpdatePheromones(List<Ant> ants)
    {
        for (int i = 0; i < NumberOfVertices; i++)
        {
            for (int j = 0; j < NumberOfVertices; j++)
            {
                if (i != j)
                {
                    Pheromones[i, j] *= EvaporationRate;

                    double deltaPheromone = 0.0;
                    foreach (var ant in ants)
                    {
                        if (ant.Path.ContainsEdge(i, j))
                        {
                            deltaPheromone += Lmin / Distances[i, j];
                        }
                    }

                    Pheromones[i, j] += deltaPheromone;
                }
            }
        }
    }
    
    public void PrintGraphInfo()
    {
        Console.WriteLine("Distances between vertices:");
        for (int i = 0; i < NumberOfVertices; i++)
        {
            for (int j = 0; j < NumberOfVertices; j++)
            {
                Console.Write(Distances[i, j] + "\t");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nPheromones on paths:");
        for (int i = 0; i < NumberOfVertices; i++)
        {
            for (int j = 0; j < NumberOfVertices; j++)
            {
                Console.Write(Pheromones[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}
