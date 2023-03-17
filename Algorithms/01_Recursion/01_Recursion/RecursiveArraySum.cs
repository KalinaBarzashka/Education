namespace _01_Recursion
{
    public static class RecursiveArraySum // internal
    {
        public static int Sum(int[] array)
        {
            if (array.Length == 1)
            {
                return array[0];
            }

            int[] newArr = new int[array.Length - 1];
            Array.Copy(array, 1, newArr, 0, array.Length - 1);

            var currentSum = array[0] + Sum(newArr);
            return currentSum;
        }
    }
}
