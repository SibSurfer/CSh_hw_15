class Program
{
    static int MinBusStops(int[][] routes, int start, int target)
    {
        if (start == target) return 0;

        var stopToRoutes = new Dictionary<int, List<int>>();
        for (int i = 0; i < routes.Length; i++)
        {
            foreach (int stop in routes[i])
            {
                if (!stopToRoutes.ContainsKey(stop))
                {
                    stopToRoutes[stop] = new List<int>();
                }
                stopToRoutes[stop].Add(i);
            }
        }

        var queue = new Queue<int>();
        var visited = new HashSet<int>();
        var steps = 0;

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            int size = queue.Count;
            for (int i = 0; i < size; i++)
            {
                int stop = queue.Dequeue();
                if (stop == target) return steps;

                foreach (int route in stopToRoutes[stop])
                {
                    foreach (int nextStop in routes[route])
                    {
                        if (!visited.Contains(nextStop))
                        {
                            queue.Enqueue(nextStop);
                            visited.Add(nextStop);
                        }
                    }
                }
            }
            steps++;
        }

        return -1; 
    }

    static void Main(string[] args)
    {
        int[][] routes1 = new int[][]
        {
            new int[] {1, 2, 7},
            new int[] {3, 6, 7}
        };
        Console.WriteLine(MinBusStops(routes1, 1, 6));

        int[][] routes2 = new int[][]
        {
            new int[] {7, 12},
            new int[] {4, 5, 15},
            new int[] {6},
            new int[] {15, 19},
            new int[] {9, 12, 13}
        };
        Console.WriteLine(MinBusStops(routes2, 15, 12)); 
    }
}
