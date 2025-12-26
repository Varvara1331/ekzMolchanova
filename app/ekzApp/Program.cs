using FloydLibrary;
using Global;

Console.WriteLine("Нахождение кратчайшего пути между точками на карте");
Console.WriteLine("Введите рёбра графа в формате: 'A B L'");
Console.WriteLine("где A, B — номера точек, L — длина дороги");
Console.WriteLine("Для завершения ввода введите 'END'");
Console.WriteLine("Для выхода из программы введите 'Q' в любом поле\n");

var vertices = new HashSet<int>();
var edges = new List<(int, int, double)>();

while (true)
{
    Console.Write("Ребро (A B L) или 'END': ");
    string input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input)) continue;
    if (input.Equals("Q", StringComparison.OrdinalIgnoreCase)) return;
    if (input.Equals("END", StringComparison.OrdinalIgnoreCase)) break;

    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length != 3)
    {
        Console.WriteLine("Ошибка: ожидается 3 значения (A B L)");
        continue;
    }

    if (!int.TryParse(parts[0], out int a) || a <= 0 ||
        !int.TryParse(parts[1], out int b) || b <= 0 ||
        !double.TryParse(parts[2], out double length) || length <= 0)
    {
        Console.WriteLine("Ошибка: A, B — целые положительные числа, L > 0");
        continue;
    }

    vertices.Add(a);
    vertices.Add(b);
    edges.Add((a, b, length));
}

if (vertices.Count == 0)
{
    Console.WriteLine("Нет данных о карте.");
    return;
}

var vertexList = new List<int>(vertices);
vertexList.Sort();
var indexMap = new Dictionary<int, int>();
var vertexByIndex = new Dictionary<int, int>();
for (int i = 0; i < vertexList.Count; i++)
{
    indexMap[vertexList[i]] = i;
    vertexByIndex[i] = vertexList[i];
}

int n = vertexList.Count;
GlobalN.n = n;
double[,] adjMatrix = new double[n, n];

for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++)
        adjMatrix[i, j] = double.PositiveInfinity;

for (int i = 0; i < n; i++)
    adjMatrix[i, i] = 0;

foreach (var (a, b, len) in edges)
{
    int i = indexMap[a], j = indexMap[b];
    if (adjMatrix[i, j] > len) adjMatrix[i, j] = len;
    if (adjMatrix[j, i] > len) adjMatrix[j, i] = len;
}

double[,] distances = FloydClass.Floyd(adjMatrix);

while (true)
{
    Console.Write("Номер первой точки (или 'Q' для выхода): ");
    string startInput = Console.ReadLine()?.Trim();
    if (startInput?.Equals("Q", StringComparison.OrdinalIgnoreCase) == true) break;

    Console.Write("Номер второй точки (или 'Q' для выхода): ");
    string endInput = Console.ReadLine()?.Trim();
    if (endInput?.Equals("Q", StringComparison.OrdinalIgnoreCase) == true) break;

    if (!int.TryParse(startInput, out int start) ||
        !int.TryParse(endInput, out int end))
    {
        Console.WriteLine("Ошибка: введите целые числа.");
        continue;
    }

    if (!indexMap.ContainsKey(start) || !indexMap.ContainsKey(end))
    {
        Console.WriteLine("Ошибка: точка не найдена на карте.");
        continue;
    }

    int idxStart = indexMap[start];
    int idxEnd = indexMap[end];
    double minDistance = distances[idxStart, idxEnd];

    if (minDistance == double.PositiveInfinity)
    {
        Console.WriteLine($"Пути между {start} и {end} не существует.");
        continue;
    }

    var pathIndices = ReconstructPath(idxStart, idxEnd, adjMatrix, distances);
    if (pathIndices == null)
    {
        Console.WriteLine($"Не удалось восстановить путь (расстояние = {minDistance:F2}).");
    }
    else
    {
        var path = pathIndices.Select(idx => vertexByIndex[idx]).ToList();
        Console.WriteLine($"Путь: {string.Join(" → ", path)}");
        Console.WriteLine($"Длина: {minDistance:F2} единиц.");
    }
}

static List<int>? ReconstructPath(int start, int end, double[,] adjMatrix, double[,] dist)
{
    if (start == end)
        return new List<int> { start };

    var path = new List<int>();
    int current = start;
    path.Add(current);

    while (current != end)
    {
        int next = -1;
        double bestReduction = double.NegativeInfinity;

        for (int neighbor = 0; neighbor < GlobalN.n; neighbor++)
        {
            if (adjMatrix[current, neighbor] == double.PositiveInfinity)
                continue;
            if (neighbor == current)
                continue;

            double edgeCost = adjMatrix[current, neighbor];
            double remaining = dist[neighbor, end];

            double totalViaNeighbor = edgeCost + remaining;

            if (Math.Abs(dist[current, end] - totalViaNeighbor) < 1e-9)
            {
                if (next == -1 || edgeCost < adjMatrix[current, next])
                {
                    next = neighbor;
                    bestReduction = edgeCost;
                }
            }
        }

        if (next == -1)
            return null;

        path.Add(next);
        current = next;
    }

    return path;
}
