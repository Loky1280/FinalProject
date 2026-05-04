namespace FinalProject.Persistence.Contracts;

public sealed class SettingsDto
{
    public string Difficulty { get; set; } = "Easy";

    public bool ShowTimer { get; set; } = true;

    public bool HighlightConflicts { get; set; } = true;
}
