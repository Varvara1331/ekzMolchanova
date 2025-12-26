using FloydLibrary;
int n;
int m;
double[,] A;

Console.WriteLine("Нахождение кратчайшего пути между точками на карте");

Console.Write("Введите количество точек на карте: ");
if (int.TryParse(Console.ReadLine(), out n) && n > 0)
{
    for (int i = 0; i < n; i++)
    {
        Console.Write("Введите количество путей на карте: ");
        if (int.TryParse(Console.ReadLine(), out m) && m > 0)
        {
            A = new double[n, m];
            for (int j = 0; j < n; j++)
            {
                Console.Write($"Введите расстояние между точкой {i} и точкой {j} (если расстояние не указано, то введите 0: ");
                if (double.TryParse(Console.ReadLine(), out double aig) && aig > 0)
                {
                    A[i, j] = aig;
                }
            }
        }
        else
        {
            Console.WriteLine("Ошибка ввода: введите целое положительное число");
        }
    }
}
else
{
    Console.WriteLine("Ошибка ввода: введите целое положительное число");
}
