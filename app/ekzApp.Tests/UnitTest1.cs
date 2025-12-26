using FloydLibrary;
namespace ekzApp.Tests
{
    public class Test
    {
        [Fact]
        public void TestFloydCalculate()
        {
            int n = 3;
            double[,] input = new double[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    input[i, j] = double.PositiveInfinity;

            for (int i = 0; i < n; i++)
                input[i, i] = 0;

            input[0, 1] = input[1, 0] = 3;
            input[1, 2] = input[2, 1] = 4;
            input[0, 2] = input[2, 0] = 10;

            double[,] result = FloydClass.Floyd(input);

            Assert.Equal(0, result[0, 0]);
            Assert.Equal(3, result[0, 1]);
            Assert.Equal(7, result[0, 2]); 

            Assert.Equal(3, result[1, 0]);
            Assert.Equal(0, result[1, 1]);
            Assert.Equal(4, result[1, 2]);

            Assert.Equal(7, result[2, 0]);
            Assert.Equal(4, result[2, 1]);
            Assert.Equal(0, result[2, 2]);
        }
    }
}
