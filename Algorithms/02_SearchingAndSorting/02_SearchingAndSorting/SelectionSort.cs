namespace _02_SearchingAndSorting
{
    public static class SelectionSort<T> where T : IComparable
    {
        // unstable sorting
        public static T[] Sort(T[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                T currentValue = array[i];
                int minValueIndex = -1;
                T minValue = currentValue;

                // look for lower value than current value
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j].CompareTo(minValue) < 0)
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
