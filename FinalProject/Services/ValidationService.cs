using FinalProject.Models;
using FinalProject.Services.Interfaces;

namespace FinalProject.Services;

public class ValidationService : IValidationService
{
    public bool IsMoveValid(SudokuBoard board, int row, int column, int value)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (value == 0)
        {
            return true;
        }

        if (value is < 1 or > 9)
        {
            return false;
        }

        return !HasConflictInRow(board, row, column, value)
               && !HasConflictInColumn(board, row, column, value)
               && !HasConflictInBlock(board, row, column, value);
    }

    public bool IsBoardComplete(SudokuBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);

        return board.Cells.All(cell =>
            cell.Value != 0 &&
            IsMoveValid(board, cell.Row, cell.Column, cell.Value));
    }

    private static bool HasConflictInRow(SudokuBoard board, int row, int column, int value)
    {
        for (var currentColumn = 0; currentColumn < SudokuBoard.Size; currentColumn++)
        {
            if (currentColumn != column &&
                board.GetCell(row, currentColumn).Value == value)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasConflictInColumn(SudokuBoard board, int row, int column, int value)
    {
        for (var currentRow = 0; currentRow < SudokuBoard.Size; currentRow++)
        {
            if (currentRow != row &&
                board.GetCell(currentRow, column).Value == value)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasConflictInBlock(SudokuBoard board, int row, int column, int value)
    {
        var blockRowStart = row - (row % 3);
        var blockColumnStart = column - (column % 3);

        for (var currentRow = blockRowStart; currentRow < blockRowStart + 3; currentRow++)
        {
            for (var currentColumn = blockColumnStart; currentColumn < blockColumnStart + 3; currentColumn++)
            {
                if (currentRow == row && currentColumn == column)
                {
                    continue;
                }

                if (board.GetCell(currentRow, currentColumn).Value == value)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
