// File: STPuzzleGenerator.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SudokuToolkit
{
    [Serializable]
    public class STTile
    {
        public int Value { get; set; }
        public bool IsFixed { get; set; }
        public int Row { get; }
        public int Col { get; }

        public STTile(int row, int col, int value = 0, bool isFixed = false)
        {
            Row = row;
            Col = col;
            Value = value;
            IsFixed = isFixed;
        }
    }

    [Serializable]
    public class STGrid
    {
        public const int Size = 9;
        public STTile[,] Tiles { get; private set; }

        public STGrid()
        {
            Tiles = new STTile[Size, Size];
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    Tiles[row, col] = new STTile(row, col);
        }
    }

    public class SudokuGeneratorClass
    {
        private readonly System.Random random;

        public SudokuGeneratorClass(int? seed = null)
        {
            random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }

        public STGrid Generate(STDifficulty difficulty, STSymmetry symmetry)
        {
            var grid = new STGrid();
            if (!Solve(grid)) return grid;
            var puzzle = new STGrid();
            for (int row = 0; row < STGrid.Size; row++)
                for (int col = 0; col < STGrid.Size; col++)
                    puzzle.Tiles[row, col] = new STTile(row, col, grid.Tiles[row, col].Value, true);
            var (minClues, maxClues) = GetClues(difficulty);
            int targetClues = random.Next(minClues, maxClues + 1);
            RemoveNumbers(puzzle, 81 - targetClues, symmetry);
            return puzzle;
        }

        private (int minClues, int maxClues) GetClues(STDifficulty difficulty)
        {
            return difficulty switch
            {
                STDifficulty.Easy => (36, 40),
                STDifficulty.Medium => (28, 35),
                STDifficulty.Hard => (22, 27),
                _ => (28, 35)
            };
        }

        private bool Solve(STGrid grid, int row = 0, int col = 0)
        {
            if (row >= STGrid.Size) return true;
            if (col >= STGrid.Size) return Solve(grid, row + 1, 0);
            if (grid.Tiles[row, col].Value != 0) return Solve(grid, row, col + 1);
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Shuffle(numbers);
            foreach (int num in numbers)
            {
                if (IsValid(grid, row, col, num))
                {
                    grid.Tiles[row, col].Value = num;
                    if (Solve(grid, row, col + 1)) return true;
                    grid.Tiles[row, col].Value = 0;
                }
            }
            return false;
        }

        public bool IsValid(STGrid grid, int row, int col, int num)
        {
            for (int x = 0; x < STGrid.Size; x++)
                if ((x != col && grid.Tiles[row, x].Value == num) || (x != row && grid.Tiles[x, col].Value == num))
                    return false;
            int startRow = row - row % 3, startCol = col - col % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if ((i + startRow != row || j + startCol != col) && grid.Tiles[i + startRow, j + startCol].Value == num)
                        return false;
            return true;
        }

        private void RemoveNumbers(STGrid grid, int count, STSymmetry symmetry)
        {
            var cells = new List<(int row, int col)>();
            for (int row = 0; row < STGrid.Size; row++)
                for (int col = 0; col < STGrid.Size; col++)
                    cells.Add((row, col));
            Shuffle(cells);
            int removed = 0;
            foreach (var (row, col) in cells)
            {
                if (removed >= count || grid.Tiles[row, col].Value == 0) continue;
                int value = grid.Tiles[row, col].Value;
                grid.Tiles[row, col].Value = 0;
                if (CountSolutions(grid) != 1)
                {
                    grid.Tiles[row, col].Value = value;
                    continue;
                }
                if (symmetry != STSymmetry.None)
                {
                    var (symRow, symCol) = GetSymmetricCell(row, col, symmetry);
                    if (symRow >= 0 && grid.Tiles[symRow, symCol].Value != 0)
                    {
                        int symValue = grid.Tiles[symRow, symCol].Value;
                        grid.Tiles[symRow, symCol].Value = 0;
                        if (CountSolutions(grid) != 1)
                        {
                            grid.Tiles[row, col].Value = value;
                            grid.Tiles[symRow, symCol].Value = symValue;
                            continue;
                        }
                        removed++;
                    }
                }
                removed++;
            }
            for (int row = 0; row < STGrid.Size; row++)
                for (int col = 0; col < STGrid.Size; col++)
                    if (grid.Tiles[row, col].Value != 0)
                        grid.Tiles[row, col].IsFixed = true;
        }

        private (int row, int col) GetSymmetricCell(int row, int col, STSymmetry symmetry)
        {
            return symmetry switch
            {
                STSymmetry.Central => (8 - row, 8 - col),
                STSymmetry.Horizontal => (row, 8 - col),
                STSymmetry.Vertical => (8 - row, col),
                _ => (-1, -1)
            };
        }

        private int CountSolutions(STGrid grid)
        {
            int solutions = 0;
            CountSolutionsRecursive(grid, ref solutions, true);
            return solutions;
        }

        private void CountSolutionsRecursive(STGrid grid, ref int solutions, bool fastExit, int row = 0, int col = 0)
        {
            if (solutions > 1 && fastExit) return;
            if (row >= STGrid.Size)
            {
                solutions++;
                return;
            }
            if (col >= STGrid.Size)
            {
                CountSolutionsRecursive(grid, ref solutions, fastExit, row + 1, 0);
                return;
            }
            if (grid.Tiles[row, col].Value != 0)
            {
                CountSolutionsRecursive(grid, ref solutions, fastExit, row, col + 1);
                return;
            }
            for (int num = 1; num <= 9; num++)
            {
                if (IsValid(grid, row, col, num))
                {
                    grid.Tiles[row, col].Value = num;
                    CountSolutionsRecursive(grid, ref solutions, fastExit, row, col + 1);
                    grid.Tiles[row, col].Value = 0;
                }
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public bool GetHint(STGrid grid, out int row, out int col, out int value)
        {
            row = col = value = 0;
            var emptyCells = new List<(int r, int c)>();
            for (int r = 0; r < STGrid.Size; r++)
                for (int c = 0; c < STGrid.Size; c++)
                    if (grid.Tiles[r, c].Value == 0)
                        emptyCells.Add((r, c));
            if (emptyCells.Count == 0) return false;
            Shuffle(emptyCells);
            var solved = new STGrid();
            for (int r = 0; r < STGrid.Size; r++)
                for (int c = 0; c < STGrid.Size; c++)
                    solved.Tiles[r, c] = new STTile(r, c, grid.Tiles[r, c].Value, grid.Tiles[r, c].IsFixed);
            Solve(solved);
            foreach (var (r, c) in emptyCells)
            {
                row = r;
                col = c;
                value = solved.Tiles[r, c].Value;
                return true;
            }
            return false;
        }

        public bool Validate(STGrid grid, bool checkComplete)
        {
            for (int row = 0; row < STGrid.Size; row++)
            {
                var rowSet = new HashSet<int>();
                var colSet = new HashSet<int>();
                for (int col = 0; col < STGrid.Size; col++)
                {
                    int rowVal = grid.Tiles[row, col].Value;
                    int colVal = grid.Tiles[col, row].Value;
                    if (rowVal != 0 && !rowSet.Add(rowVal)) return false;
                    if (colVal != 0 && !colSet.Add(colVal)) return false;
                }
            }
            for (int boxRow = 0; boxRow < 3; boxRow++)
                for (int boxCol = 0; boxCol < 3; boxCol++)
                {
                    var boxSet = new HashSet<int>();
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            int val = grid.Tiles[boxRow * 3 + i, boxCol * 3 + j].Value;
                            if (val != 0 && !boxSet.Add(val)) return false;
                        }
                }
            if (checkComplete)
                for (int row = 0; row < STGrid.Size; row++)
                    for (int col = 0; col < STGrid.Size; col++)
                        if (grid.Tiles[row, col].Value == 0)
                            return false;
            return true;
        }

        public List<(int row, int col)> FindConflicts(STGrid grid)
        {
            var conflicts = new List<(int row, int col)>();
            for (int row = 0; row < STGrid.Size; row++)
            {
                for (int col = 0; col < STGrid.Size; col++)
                {
                    int value = grid.Tiles[row, col].Value;
                    if (value == 0) continue;
                    if (!IsValid(grid, row, col, value))
                        conflicts.Add((row, col));
                }
            }
            return conflicts;
        }

        public string Serialize(STGrid grid)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("v1|");
            for (int row = 0; row < STGrid.Size; row++)
                for (int col = 0; col < STGrid.Size; col++)
                    sb.Append($"{grid.Tiles[row, col].Value}{(grid.Tiles[row, col].IsFixed ? 'T' : 'F')}");
            return sb.ToString();
        }

        public STGrid Deserialize(string data)
        {
            var grid = new STGrid();
            if (string.IsNullOrEmpty(data) || !data.StartsWith("v1|")) return grid;
            data = data.Substring(3);
            if (data.Length != 2 * STGrid.Size * STGrid.Size) return grid;
            try
            {
                for (int i = 0, row = 0; row < STGrid.Size; row++)
                    for (int col = 0; col < STGrid.Size; col++, i += 2)
                    {
                        grid.Tiles[row, col].Value = int.Parse(data[i].ToString());
                        grid.Tiles[row, col].IsFixed = data[i + 1] == 'T';
                    }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SudokuGenerator] Failed to deserialize: {e.Message}");
            }
            return grid;
        }
    }
}