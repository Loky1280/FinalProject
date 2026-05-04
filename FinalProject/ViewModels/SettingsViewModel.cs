using System.Collections.ObjectModel;
using System.Windows.Input;
using FinalProject.Commands;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.Services.Interfaces;

namespace FinalProject.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private GameDifficulty _selectedDifficulty = GameDifficulty.Easy;
    private bool _showTimer = true;
    private bool _highlightConflicts = true;
    private string _statusMessage = "Завантаження налаштувань...";

    public SettingsViewModel() : this(new SettingsService())
    {
    }

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        Difficulties = new ObservableCollection<GameDifficulty>(Enum.GetValues<GameDifficulty>());
        SaveCommand = new RelayCommand(_ => _ = SaveAsync());
        ReloadCommand = new RelayCommand(_ => _ = LoadAsync());
        _ = LoadAsync();
    }

    public string Title => "Налаштування";

    public ObservableCollection<GameDifficulty> Difficulties { get; }

    public ICommand SaveCommand { get; }

    public ICommand ReloadCommand { get; }

    public GameDifficulty SelectedDifficulty
    {
        get => _selectedDifficulty;
        set => SetProperty(ref _selectedDifficulty, value);
    }

    public bool ShowTimer
    {
        get => _showTimer;
        set => SetProperty(ref _showTimer, value);
    }

    public bool HighlightConflicts
    {
        get => _highlightConflicts;
        set => SetProperty(ref _highlightConflicts, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    private async Task LoadAsync()
    {
        try
        {
            var settings = await _settingsService.GetSettingsAsync();
            SelectedDifficulty = settings.DefaultDifficulty;
            ShowTimer = settings.ShowTimer;
            HighlightConflicts = settings.HighlightConflicts;
            StatusMessage = "Налаштування завантажено.";
        }
        catch
        {
            StatusMessage = "Не вдалося завантажити налаштування.";
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            var settings = AppSettings.Create(SelectedDifficulty, ShowTimer, HighlightConflicts);
            await _settingsService.SaveSettingsAsync(settings);
            StatusMessage = "Налаштування збережено.";
        }
        catch
        {
            StatusMessage = "Помилка збереження налаштувань.";
        }
    }
}
