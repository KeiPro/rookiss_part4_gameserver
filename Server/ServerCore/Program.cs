namespace ServerCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] array = new int[10000, 10000];

            {
                // [][][][][] [][][][][] [][][][][] [][][][][] [][][][][]
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                {
                    for (int x = 0; x < 10000; x++)
                    {
                        array[y, x] = 1;
                    }
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y, x) 걸린 시간 : {end - now}");
            }

            { 
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                {
                    for (int x = 0; x < 10000; x++)
                    {
                        array[x, y] = 1; // 캐시 철학의 공간 지역성에 대한 이점을 활용할 수 없다.
                    }
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x, y) 걸린 시간 : {end - now}");
            }
        }
    }
}
