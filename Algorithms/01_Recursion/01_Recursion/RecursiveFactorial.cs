namespace _01_Recursion
{
    public static class RecursiveFactorial
    {
        public static long Factorial(int number)
        {
            if (number == 0)
            {
                return 1;
            }
            return number * Factorial(number - 1);
        }
    }
}
