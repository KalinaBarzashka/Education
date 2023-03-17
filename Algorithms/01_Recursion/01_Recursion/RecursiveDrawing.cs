namespace _01_Recursion
{
    public static class RecursiveDrawing
    {
        public static void Draw(int number)
        {
            if (number <= 0)
            {
                return;
            }
            Console.WriteLine(new String('*', number));

            Draw(number - 1);

            Console.WriteLine(new String('#', number));
        }
    }
}
