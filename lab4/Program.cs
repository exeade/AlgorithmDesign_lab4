namespace lab4;

public static class Program
{
    public static void Main()
    {
        int numberOfVertices = 250;
        int numberOfAnts = 45;
        int colonyIterations = 1000;
        int stagnationMax = 1000;
        double evaporationRate = 1 - 0.3;
        HashSet<int> usedVertices = new HashSet<int>();
        int alpha = 4;
        int beta = 2;

        Graph graph = new Graph(numberOfVertices, alpha, beta, evaporationRate);
        
        Console.WriteLine(new string('-', 70));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Solving the Travelling Salesman Problem using the Ant Colony Algorithm");
        Console.ResetColor();

        Console.WriteLine(new string('-', 70));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Number of vertices:          {numberOfVertices}");
        Console.WriteLine($"Number of ants:              {numberOfAnts}");
        Console.WriteLine($"Evaporation rate:            {evaporationRate * 100}%");
        Console.WriteLine($"Alpha (pheromone influence): {alpha}");
        Console.WriteLine($"Beta (visibility influence): {beta}");
        Console.ResetColor();

        Console.WriteLine(new string('-', 70));
        Console.WriteLine();

        
        Random rand = new Random();
        List<Ant> ants = new List<Ant>();
        for (int i = 0; i < numberOfAnts; i++)
        {
            int startVertex;
            bool isWild = i < 10;
            do
            {
                startVertex = rand.Next(numberOfVertices);
            } while (usedVertices.Contains(startVertex));

            usedVertices.Add(startVertex);
            ants.Add(new Ant(graph, startVertex, isWild));
        }

        Path? shortestPath = null;
        int stagnationCounter = 0;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Solving started...");
        Console.ResetColor();
        Console.WriteLine();
        for (int iteration = 0; iteration < colonyIterations; iteration++)
        {
            foreach (var ant in ants)
            {
                bool[] visited = new bool[numberOfVertices];
                visited[ant.CurrentVertex] = true;

                for (int step = 0; step < numberOfVertices - 1; step++)
                {
                    int nextVertex = ant.ChooseNextVertex(visited);
                    if (nextVertex == -1) break;

                    ant.MoveToNextVertex(nextVertex);
                    visited[nextVertex] = true;
                }
            }
            
            Path bestPathInIteration = ants.MinBy(ant => ant.Path.Length)!.Path;

            if (shortestPath == null || bestPathInIteration.Length < shortestPath.Length)
            {
                shortestPath = new Path();
                shortestPath.Vertices.AddRange(bestPathInIteration.Vertices);
                shortestPath.Length = bestPathInIteration.Length;
                stagnationCounter = 0;
            }
            else
            {
                stagnationCounter++;
            }
            
            if (stagnationCounter >= stagnationMax)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Stagnation reached. Terminating...");
                Console.ResetColor();
                Console.WriteLine();
                break;
            }
            
            graph.UpdatePheromones(ants);
            
            foreach (var ant in ants)
            {
                ant.Reset();
            }
            
            if ((iteration + 1) % 20 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green; 
                Console.WriteLine($"Iteration {iteration + 1}: Shortest path length = {shortestPath.Length}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Iteration {iteration + 1}: Shortest path length = {shortestPath.Length}");
            }
        }
        
        int maxVerticesPerLine = 10; 
        int vertexCount = shortestPath!.Vertices.Count;
        int padding = 5; 

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Shortest Path:");
        Console.ResetColor();
        Console.WriteLine();
        
        Console.Write(new string(' ', padding));

        for (int i = 0; i < vertexCount; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{shortestPath.Vertices[i],-3}]");
            
            if ((i + 1) % maxVerticesPerLine == 0 && i + 1 < vertexCount)
            {
                Console.WriteLine();
                Console.Write(new string(' ', padding));
            }
            else if (i + 1 < vertexCount)
            {
                Console.ResetColor();
                Console.Write(" -> ");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.ResetColor();
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
