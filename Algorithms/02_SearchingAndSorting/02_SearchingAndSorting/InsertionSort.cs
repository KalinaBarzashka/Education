namespace _02_SearchingAndSorting
{
    public static class InsertionSort
    {
        // stable sorting
        public static int[] Sort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int currentValue = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j] > currentValue)
                    {
                        int tempValue = array[j];
                        array[j] = currentValue;
                        array[j + 1] = tempValue;
                        continue;
                    }

                    break;
                }
            }
            return array;
        }
    }
}
