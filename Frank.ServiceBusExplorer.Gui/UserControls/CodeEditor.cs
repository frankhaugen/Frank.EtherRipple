using System.Windows.Controls;

using ICSharpCode.AvalonEdit.Highlighting;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class CodeEditor : ICSharpCode.AvalonEdit.TextEditor
{
    public CodeEditor(IHighlightingDefinition highlightingDefinition)
    {
        SyntaxHighlighting = highlightingDefinition;
        FontFamily = new("Consolas");
        FontSize = 12;
        ShowLineNumbers = true;
        WordWrap = true;
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    }
}