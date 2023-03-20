namespace _02_SearchingAndSorting
{
    public static class BubbleSort<T> where T: IComparable
    {
        // stable sorting
        public static T[] Sort(T[] array)
        {
            bool swapped = true;

            while (true)
            {
                if (!swapped)
                {
                    break;
                }

                swapped = false;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if(array[i + 1].CompareTo(array[i]) < 0)// < array[i])
                    {
                        swapped = true;
                        T temp = array[i];
                        array[i] = array[i+1];
                        array[i+1] = temp;
                    }
                }
            }

            return array;
        }
    }
}
