namespace _01_Recursion
{
    public static class FindAllPathsInLabyrinth
    {
        public static string[][] labyrinth;
        public static int currentRow = 0;
        public static int currentCol = 0;
        public static void Solve()
        {
            labyrinth = new string[][]
            {
                new string[] { "-", "-", "-", "*", "-", "-" },
                new string[] { "*", "*", "-", "*", "*", "-" },
                new string[] { "-", "-", "-", "-", "-", "-" },
                new string[] { "-", "*", "*", "*", "-", "-" },
                new string[] { "-", "*", "-", "*", "*", "-" },
                new string[] { "-", "-", "-", "-", "-", "e" }
            };


            FindPath(currentRow, currentCol);
        }

        public static void FindPath(int row, int col)
        {
            if (!CanStep(row, col))
            {
                return;
            }

            if (labyrinth[row][col].Equals("e"))
            {
                PrintLabyrint();
                return;
            }

            // mark cell
            labyrinth[row][col] = "x";

            // find path right
            FindPath(row, col + 1);
            // find path down
            FindPath(row + 1, col);
            // find path left
            FindPath(row, col - 1);
            // find path up
            FindPath(row - 1, col);

            // unmark cell
            labyrinth[row][col] = "-";
        }

        public static bool CanStep(int row, int col)
        {
            // out of bounds ?
            if (row < 0 || row >= labyrinth.Length || col >= labyrinth[row].Length || col < 0)
            {
                return false;
            }

            // check for wall
            if (labyrinth[row][col].Equals("*"))
            {
                return false;
            }

            // check for marked cell
            if (labyrinth[row][col].Equals("x"))
            {
                return false;
            }

            return true;
        }

        public static void PrintLabyrint()
        {
            for (int i = 0; i < labyrinth.Length; i++)
            {
                for (int j = 0; j < labyrinth[i].Length; j++)
                {
                    Console.Write(labyrinth[i][j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
