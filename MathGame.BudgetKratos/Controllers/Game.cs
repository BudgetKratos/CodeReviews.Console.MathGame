namespace MathGame.Controllers;

using Spectre.Console;
using static Enums.Menus;
using static UserInterface;

internal class Game : BaseGame
{
    public void Menu()
    {
        Console.Clear();

        GameChoices gameChoice = AnsiConsole.Prompt(
            new SelectionPrompt<GameChoices>()
                .WrapAround()
                .Title("Pick a game")
                .AddChoices(Enum.GetValues<GameChoices>())
        );

        switch (gameChoice)
        {
            case GameChoices.Addition:
                Operate('+', "DarkOrange");
                break;

            case GameChoices.Division:
                Operate('/', "Aqua");
                break;

            case GameChoices.Multiplication:
                Operate('*', "Green3_1");
                break;

            case GameChoices.Subtraction:
                Operate('-', "Gold1");
                break;

            case GameChoices.Randomizer:
                Operate('?', "DarkViolet");
                break;

            case GameChoices.Back:
                return;
        }
    }

    public void Operate(char op, string displayColor)
    {
        char[] validOperators = ['+', '-', '*', '/'];

        char actualOperation = op;

        bool retry = true;
        bool isComplex = false;

        int userAnswer;
        int score = 0;

        if (op is '?')
            actualOperation = validOperators[RandomNumber.Next(validOperators.Length)];

        if (actualOperation is '/')
            isComplex = true;

        while (retry)
        {
            int numberOfTurns = 5;

            do
            {
                (int, int) operands = GenerateOperands(isComplex);

                userAnswer = AnsiConsole.Ask<int>(
                    $"[{displayColor}]{operands.Item1} {actualOperation} {operands.Item2} = [/]"
                );

                if (CheckAnswer(operands, userAnswer, actualOperation))
                    score++;

                numberOfTurns--;
            } while (numberOfTurns > 0);

            retry = Confirm("Again?", displayColor);

            if (!retry)
            {
                DisplayMessage("Press any key to go back to the previous menu.", displayColor);
                Console.ReadKey();
                Menu();
            }
        }

        ScoreCard.Add($"{CurrentDifficulty} {actualOperation} {score}");
    }

    public (int, int) GenerateOperands(bool isComplex)
    {
        int operandOne,
            operandTwo;

        if (isComplex)
        {
            do
            {
                operandOne = RandomNumber.Next(LowerBound, UpperBound);
                operandTwo = RandomNumber.Next(LowerBound, UpperBound);
            } while (operandOne % operandTwo != 0 && operandOne != operandTwo);
        }
        else
        {
            operandOne = RandomNumber.Next(LowerBound, UpperBound);
            operandTwo = RandomNumber.Next(LowerBound, UpperBound);
        }

        if (operandTwo > operandOne)
            return (operandTwo, operandOne);

        return (operandOne, operandTwo);
    }

    private static bool CheckAnswer((int, int) operands, int answer, char operation)
    {
        switch (operation)
        {
            case '+':
                if (answer == (operands.Item1 + operands.Item2))
                    return true;
                break;

            case '*':
                if (answer == (operands.Item1 * operands.Item2))
                    return true;
                break;

            case '-':
                if (answer == (operands.Item1 - operands.Item2))
                    return true;
                break;

            case '/':
                if (answer == (operands.Item1 / operands.Item2))
                    return true;
                break;
        }

        return false;
    }

    public void ShowScores()
    {
        var table = new Table();
        int i = 1;

        table.Border(TableBorder.Rounded);

        table.AddColumn($"Try #");
        table.AddColumn($"Difficulty");
        table.AddColumn($"Game");
        table.AddColumn($"Score");

        foreach (string score in ScoreCard)
        {
            string[] subStrings = score.Split();

            table.AddRow(
                $"{i}",
                $"{subStrings[0]}", // CurrentDifficulty
                $"{subStrings[1]}", // Game
                $"{subStrings[2]}" // Score
            );

            i++;
        }

        AnsiConsole.Write(table);

        DisplayMessage("Press any key to go back to the previous menu.");
        Console.ReadKey();
        Console.Clear();
        return;
    }

    public void Options()
    {
        string optionsChoice = SelectionMenu(
            "What would you like to do?",
            ["Change difficulty", "Back"]
        );

        if (optionsChoice is "Change difficulty")
            ChangeDifficulty();
        else
            return;
    }

    public void ChangeDifficulty()
    {
        DisplayMessage($"Current difficulty is: {CurrentDifficulty}.");

        DifficultyChoices difficultyChoice = AnsiConsole.Prompt(
                new SelectionPrompt<DifficultyChoices>()
                .WrapAround()
                .Title("Choose wisely:")
                .AddChoices(Enum.GetValues<DifficultyChoices>()));

        if (difficultyChoice is DifficultyChoices.Back)
        {
            Console.Clear();
            return;
        }
        else
            SetDifficulty(difficultyChoice);
    }
}
