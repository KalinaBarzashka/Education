namespace _02_SearchingAndSorting
{
    public static class MergeSort<T> where T : IComparable
    {
        // stable sorting
        public static void Sort(T[] array, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex)
            {
                // array with one element
                return;
            }

            var middleIndex = (startIndex + endIndex) / 2;

            Sort(array, startIndex, middleIndex); // left part of the array
            Sort(array, middleIndex + 1, endIndex); // right part of the array

            Merge(array, startIndex, middleIndex, endIndex);
        }

        public static void Merge(T[] array, int startIndex, int middleIndex, int endIndex)
        {
            // array is already sorted
            if (middleIndex < 0 || middleIndex + 1 >= array.Length 
                || array[middleIndex].CompareTo(array[middleIndex + 1]) <= 0)
            {
                return;
            }

            T[] helperArray = new T[array.Length];
            Array.Copy(array, 0, helperArray, 0, array.Length);
            int indexForLeftArray = startIndex;
            int indexForRightArray = middleIndex + 1;

            for (int i = startIndex; i <= endIndex; i++)
            {
                if(indexForLeftArray > middleIndex)
                {
                    array[i] = helperArray[indexForRightArray++];
                }
                else if(indexForRightArray > endIndex)
                {
                    array[i] = helperArray[indexForLeftArray++];
                }
                else if(helperArray[indexForLeftArray].CompareTo(helperArray[indexForRightArray]) <= 0)
                {
                    array[i] = helperArray[indexForLeftArray++];
                }
                else if (helperArray[indexForLeftArray].CompareTo(helperArray[indexForRightArray]) > 0)
                {
                    array[i] = helperArray[indexForRightArray++];
                }
            }
        }
    }
}
