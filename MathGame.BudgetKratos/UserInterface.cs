namespace MathGame;

using Spectre.Console;
using static Enums.Menus;
using Controllers;

internal class UserInterface
{
    private readonly Game _game = new();

    private const string BaseColor = "yellow";

    public static void DisplayMessage(string message, string displayColor = BaseColor)
    {
        AnsiConsole.MarkupLine($"[{displayColor}]{message}[/]");
    }

    public static bool Confirm(string question, string displayColor = BaseColor)
    {
        return AnsiConsole.Confirm($"[{displayColor}]{question}[/]");
    }

    public static string SelectionMenu(string title, string[] options)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>().WrapAround().Title($"{title}").AddChoices(options)
        );
    }

    internal void MainMenu()
    {
        Console.Clear();

        AnsiConsole.Write(new FigletText("Math Game").LeftJustified().Color(Color.Red));

        while (true)
        {
            MainMenuChoices mainLoopChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuChoices>()
                    .WrapAround()
                    .Title("Would you like to play a game?")
                    .AddChoices(Enum.GetValues<MainMenuChoices>())
            );

            switch (mainLoopChoice)
            {
                case MainMenuChoices.Play:
                    _game.Menu();
                    break;

                case MainMenuChoices.Scores:
                    _game.ShowScores();
                    break;

                case MainMenuChoices.Options:
                    _game.Options();
                    break;

                case MainMenuChoices.Quit:
                    DisplayMessage("Press any key to quit.");
                    Console.ReadKey();
                    return;
            }
        }
    }
}
