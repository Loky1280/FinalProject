namespace FinalProject.Models;

public class AppSettings
{
    public GameDifficulty DefaultDifficulty { get; private set; } = GameDifficulty.Easy;

    public bool ShowTimer { get; private set; } = true;

    public bool HighlightConflicts { get; private set; } = true;

    public static AppSettings Create(GameDifficulty defaultDifficulty, bool showTimer, bool highlightConflicts)
    {
        return new AppSettings
        {
            DefaultDifficulty = defaultDifficulty,
            ShowTimer = showTimer,
            HighlightConflicts = highlightConflicts
        };
    }
}
