using _02_SearchingAndSorting;

int[] array = new int[] { 13, 2, 7, 15, 8, 36, 21, 18, 11, 24, 31, 16, 1 };
string[] stringArr = new string[] { "karina", "gosho", "kalina", "ivan", "georgi" };

// array = BubbleSort<int>.Sort(array);
// stringArr = BubbleSort<string>.Sort(stringArr);

// array = SelectionSort<int>.Sort(array);
// stringArr = SelectionSort<string>.Sort(stringArr);

// array = InsertionSort<int>.Sort(array);
// stringArr = InsertionSort<string>.Sort(stringArr);

MergeSort<int>.Sort(array, 0, array.Length - 1);
MergeSort<string>.Sort(stringArr, 0, stringArr.Length - 1);

Console.WriteLine(String.Join(" ", array));
Console.WriteLine(String.Join(" ", stringArr));

