namespace _02_SearchingAndSorting
{
    public static class InsertionSort<T> where T : IComparable
    {
        // stable sorting
        public static T[] Sort(T[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                T currentValue = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j].CompareTo(currentValue) > 0)
                    {
                        T tempValue = array[j];
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
