using Spectre.Console;

namespace Frank.ServiceBusExplorer.Gui;

public class SpectreEnumInput<T> : IUserInput<T> where T : struct, Enum
{
    private readonly SelectionPrompt<T> _prompt;

    public SpectreEnumInput(string promptText)
    {
        var items = Enum.GetValues<T>();
        
        _prompt = new SelectionPrompt<T>()
            .AddChoices(items)
            .PageSize(10)
            .UseConverter(item => item.ToString())
            .Title(promptText);
    }

    public T Display() => AnsiConsole.Prompt(_prompt);
}