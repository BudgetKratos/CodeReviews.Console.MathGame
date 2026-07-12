namespace MathGame.Controllers;

using static Enums.Menus;

internal abstract class BaseGame
{
    protected Random? RandomNumber { get; } = new();

    protected int LowerBound { get; private set; } = 1;

    protected int UpperBound { get; private set; } = 20;

    protected List<string> ScoreCard { get; set; } = [];

    protected string CurrentDifficulty { get; set; } = "Easy";

    private (int, int) SetBounds(int lower, int upper)
    {
        LowerBound = lower;
        UpperBound = upper;

        return (LowerBound, UpperBound);
    }

    public (int, int) SetDifficulty(DifficultyChoices difficulty)
    {
        if (difficulty.ToString() is "Medium")
        {
            CurrentDifficulty = "Medium";
            return SetBounds(21, 50);
        }
        else if (difficulty.ToString() is "Hard")
        {
            CurrentDifficulty = "Hard";
            return SetBounds(51, 100);
        }
        else if (difficulty.ToString() is "Insane")
        {
            CurrentDifficulty = "Insane";
            return SetBounds(1, 100);
        }
        else
        {
            CurrentDifficulty = "Easy";
            return SetBounds(1, 20);
        }
    }
}

