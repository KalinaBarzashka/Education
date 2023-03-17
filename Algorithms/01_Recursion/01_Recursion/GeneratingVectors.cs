namespace _01_Recursion
{
    public static class GeneratingVectors
    {
        public static void Generate(int index, int[] vector)
        {
            if (index >= vector.Length) // print vactor 
            {
                Console.WriteLine(String.Join(" ", vector));
                return;
            }

            for (int i = 0; i <= 1; i++)
            {
                vector[index] = i;
                Generate(index + 1, vector);
            }
        }
    }
}
