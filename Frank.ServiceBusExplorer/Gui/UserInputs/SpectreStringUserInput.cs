using Spectre.Console;

namespace Frank.ServiceBusExplorer.Gui.UserInputs;

public class SpectreStringUserInput : IUserInput<string>
{
    private readonly TextPrompt<string> _prompt;

    public SpectreStringUserInput(string promptText) =>
        _prompt = new TextPrompt<string>(promptText)
            .PromptStyle("green")
            .ValidationErrorMessage("Please enter a value")
            .Validate(value => !string.IsNullOrWhiteSpace(value));

    public string Display() => AnsiConsole.Prompt(_prompt);
}