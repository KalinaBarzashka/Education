namespace _01_Recursion
{
    public static class GeneratingCombinations
    {
        public static void Generate(int[] set, int[] vector, int index, int border) // set is with unique numbers
        {
            if (index >= vector.Length)
            {
                Console.WriteLine(String.Join(" ", vector));
                return;
            }

            for (int i = border; i < set.Length; i++)
            {
                vector[index] = set[i];
                Generate(set, vector, index + 1, i + 1);
            }
        }
    }
}
