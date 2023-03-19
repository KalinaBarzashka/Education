using _02_SearchingAndSorting;

int[] array = new int[] { 13, 2, 7, 15, 8, 36, 21, 18, 11, 24, 31 };

// array = BubbleSort.Sort(array);
// array = SelectionSort.Sort(array);
// array = InsertionSort.Sort(array);
array = MergeSort.Sort(array);

Console.WriteLine(String.Join(" ", array));

