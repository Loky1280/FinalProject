using System.IO;
using System.Text.Json;
using FinalProject.Models;
using FinalProject.Persistence.Contracts;
using FinalProject.Services.Interfaces;

namespace FinalProject.Services;

public class SettingsService : ISettingsService
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _settingsFilePath;

    public SettingsService(string? settingsFilePath = null)
    {
        _settingsFilePath = settingsFilePath ?? Path.Combine(AppContext.BaseDirectory, "Persistence", "settings.json");
    }

    public async Task<AppSettings> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_settingsFilePath))
        {
            return AppSettings.Create(GameDifficulty.Easy, showTimer: true, highlightConflicts: true);
        }

        await using var stream = File.OpenRead(_settingsFilePath);
        var dto = await JsonSerializer.DeserializeAsync<SettingsDto>(stream, _serializerOptions, cancellationToken);
        if (dto is null)
        {
            return AppSettings.Create(GameDifficulty.Easy, showTimer: true, highlightConflicts: true);
        }

        var parsed = Enum.TryParse<GameDifficulty>(dto.Difficulty, ignoreCase: true, out var difficulty)
            ? difficulty
            : GameDifficulty.Easy;
        return AppSettings.Create(parsed, dto.ShowTimer, dto.HighlightConflicts);
    }

    public async Task SaveSettingsAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var directoryPath = Path.GetDirectoryName(_settingsFilePath);
        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var dto = new SettingsDto
        {
            Difficulty = settings.DefaultDifficulty.ToString(),
            ShowTimer = settings.ShowTimer,
            HighlightConflicts = settings.HighlightConflicts
        };

        var tempFilePath = _settingsFilePath + ".tmp";

        using (var stream = File.Create(tempFilePath))
        {
            await JsonSerializer.SerializeAsync(stream, dto, _serializerOptions, cancellationToken);
        } 
        File.Move(tempFilePath, _settingsFilePath, overwrite: true);
    }
}
