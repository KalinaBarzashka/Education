namespace _01_Recursion
{
    public static class EightQueens
    {
        public static int[][] board;
        public static int queensCounter;
        public static int solutions;

        public static void Solve()
        {
            board = new int[8][];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new int[8];
                for (int j = 0; j < board[i].Length; j++)
                {
                    board[i][j] = 0;
                }
            }

            PutQueen(0);

            Console.WriteLine(solutions);
            //PrintBoard();
        }

        public static void PutQueen(int row)
        {
            if (queensCounter == 8)
            {
                PrintBoard();
                return;
            }

            if (row == board.Length)
            {
                return;
            }

            for (int col = 0; col < board[row].Length; col++)
            {
                bool isPositionSafe = IsPositionSafe(row, col);
                if (isPositionSafe)
                {
                    board[row][col] = 1;
                    queensCounter++;
                    PutQueen(row + 1);

                    // backtracking
                    board[row][col] = 0;
                    queensCounter--;
                }
            }
        }

        public static bool IsPositionSafe(int row, int col)
        {
            // check horizontal and vertical line
            for (int i = 0; i < board.Length; i++)
            {
                if (board[row][i] == 1 || board[i][col] == 1)
                {
                    return false;
                }
            }

            // check primary diagonal
            int min = Math.Min(row, col);
            int startPrimaryDiagRow = row - min;
            int startPrimaryDiagCol = col - min;

            while (true)
            {
                if (startPrimaryDiagRow >= board.Length || startPrimaryDiagCol >= board.Length)
                {
                    break;
                }

                if (board[startPrimaryDiagRow][startPrimaryDiagCol] == 1)
                {
                    return false;
                }

                startPrimaryDiagRow++;
                startPrimaryDiagCol++;
            }

            // check anti diagonal
            int substractedRow = row - 0;
            int substractedCol = board.Length - 1 - col;
            int minAntiDiagonal = Math.Min(substractedRow, substractedCol);

            int startAntiDiagRow = row - minAntiDiagonal;
            int startAntiDiagCol = col + minAntiDiagonal;

            while(true)
            {
                if (startAntiDiagRow >= board.Length || startAntiDiagCol < 0)
                {
                    break;
                }

                if (board[startAntiDiagRow][startAntiDiagCol] == 1)
                {
                    return false;
                }

                startAntiDiagRow++;
                startAntiDiagCol--;
            }

            // position is safe
            return true;
        }

        public static void PrintBoard()
        {
            solutions++;
            Console.WriteLine("Solution: " + solutions + "------------------------");

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    Console.Write(board[i][j] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("---------------------------------------------------");
        }
    }
}
