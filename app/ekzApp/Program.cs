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
    Console.WriteLine("Нет данных о карте. Завершение.");
    return;
}

var vertexList = new List<int>(vertices);
vertexList.Sort();
var indexMap = new Dictionary<int, int>();
for (int i = 0; i < vertexList.Count; i++)
    indexMap[vertexList[i]] = i;

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
