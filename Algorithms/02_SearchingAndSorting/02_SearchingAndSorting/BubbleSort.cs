namespace _02_SearchingAndSorting
{
    public static class BubbleSort
    {
        // stable sorting
        public static int[] Sort(int[] array)
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
                    if(array[i + 1] < array[i])
                    {
                        swapped = true;
                        int temp = array[i];
                        array[i] = array[i+1];
                        array[i+1] = temp;
                    }
                }
            }

            return array;
        }
    }
}
