using Global;

namespace FloydLibrary
{
    public class FloydClass
    {
        public static double[,] Floyd(double[,] a)
        {
            double[,] d = new double[GlobalN.n, GlobalN.n];
            d = (double[,])a.Clone();
            for (int i = 1; i <= GlobalN.n; i++)
                for (int j = 0; j <= GlobalN.n - 1; j++)
                    for (int k = 0; k <= GlobalN.n - 1; k++)
                        if (d[j, k] > d[j, i - 1] + d[i - 1, k])
                            d[j, k] = d[j, i - 1] + d[i - 1, k];
            return d;
        }
    }
}
