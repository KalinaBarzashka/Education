namespace _02_SearchingAndSorting
{
    public static class SelectionSort
    {
        // unstable sorting
        public static int[] Sort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int currentValue = array[i];
                int minValueIndex = -1;
                int minValue = currentValue;

                // look for lower value than current value
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < minValue)
                    {
                        minValueIndex = j;
                        minValue = array[j];
                    }
                }

                if (minValueIndex > -1)
                {
                    array[i] = array[minValueIndex];
                    array[minValueIndex] = currentValue;
                }
            }

            return array;
        }
    }
}
