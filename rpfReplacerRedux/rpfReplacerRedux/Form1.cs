using CodeWalker.GameFiles;
using System.Security.Cryptography;
using System.Text.Json;

namespace rpfReplacerRedux;

public partial class Form1 : Form
{
    private static readonly ReplacementTarget[] Targets =
    [
        new(
            "Прицел HUD",
            "Заменяет hud_reticle.gfx внутри scaleform_generic.rpf",
            "x64/data/cdimages/scaleform_generic.rpf",
            "hud_reticle.gfx"),
        new(
            "Миникарта",
            "Заменяет minimap.gfx внутри scaleform_minimap.rpf",
            "x64/patch/data/cdimages/scaleform_minimap.rpf",
            "minimap.gfx")
    ];

    private static readonly string DefaultArchivePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
        "Steam",
        "steamapps",
        "common",
        "Grand Theft Auto V",
        "update",
        "update.rpf");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string settingsPath;
    private readonly System.Windows.Forms.Timer fadeInTimer;
    private readonly Dictionary<string, int> warningCounters = [];
    private AppSettings settings = new();
    private ThemePalette currentTheme = ThemePalette.Light;
    private int totalWarningCount;

    public Form1()
    {
        InitializeComponent();

        settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "rpfReplacerRedux",
            "settings.json");

        fadeInTimer = new System.Windows.Forms.Timer { Interval = 18 };
        fadeInTimer.Tick += FadeInTimer_Tick;

        targetComboBox.DataSource = Targets;
        targetComboBox.DisplayMember = nameof(ReplacementTarget.DisplayName);
        targetComboBox.SelectedIndex = 0;

        LoadSettings();
        ResolveArchivePath();

        if (targetComboBox.SelectedItem is ReplacementTarget target)
        {
            targetHintLabel.Text = $"{target.Description}. Внутренний путь: {target.ArchivePath}/{target.FileName}";
        }

        Opacity = 0;
        ApplyTheme(currentTheme);
        fadeInTimer.Start();
    }

    private void browseArchiveButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "RPF archive (*.rpf)|*.rpf",
            Title = "Выберите update.rpf",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        archivePathTextBox.Text = dialog.FileName;
        settings.LastArchivePath = dialog.FileName;
        SaveSettings();
        SetStatus("Путь обновлен");
        AppendLog($"Выбран архив: {dialog.FileName}");
    }

    private void autoDetectButton_Click(object? sender, EventArgs e)
    {
        ResolveArchivePath(forceDetect: true);
    }

    private void browseReplacementButton_Click(object? sender, EventArgs e)
    {
        var selectedTarget = GetSelectedTarget();
        using var dialog = new OpenFileDialog
        {
            Filter = "GFX file (*.gfx)|*.gfx|All files (*.*)|*.*",
            Title = $"Выберите файл для замены: {selectedTarget.FileName}",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        replacementPathTextBox.Text = dialog.FileName;
        SetStatus("Файл замены выбран");
    }

    private async void replaceButton_Click(object? sender, EventArgs e)
    {
        try
        {
            replaceButton.Enabled = false;
            totalWarningCount = 0;
            warningCounters.Clear();
            logTextBox.Clear();
            SetStatus("Выполняется замена");

            ValidateInputs();

            settings.LastArchivePath = archivePathTextBox.Text;
            SaveSettings();

            var selectedTarget = GetSelectedTarget();
            await Task.Run(() => ReplaceFile(archivePathTextBox.Text, replacementPathTextBox.Text, selectedTarget));

            FlushWarningSummary();
            SetStatus("Готово");
            MessageBox.Show(
                this,
                $"Файл {selectedTarget.FileName} успешно заменен.",
                "Успех",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка");
            AppendLog($"Ошибка: {ex.Message}");
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            replaceButton.Enabled = true;
        }
    }

    private void targetComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (targetComboBox.SelectedItem is not ReplacementTarget selectedTarget)
        {
            return;
        }

        targetHintLabel.Text = $"{selectedTarget.Description}. Внутренний путь: {selectedTarget.ArchivePath}/{selectedTarget.FileName}";
    }

    private void themeToggle_CheckedChanged(object? sender, EventArgs e)
    {
        settings.UseDarkTheme = themeToggle.Checked;
        SaveSettings();
        currentTheme = themeToggle.Checked ? ThemePalette.Dark : ThemePalette.Light;
        ApplyTheme(currentTheme);
    }

    private void ResolveArchivePath(bool forceDetect = false)
    {
        var candidates = new List<string>();

        if (!forceDetect && !string.IsNullOrWhiteSpace(settings.LastArchivePath))
        {
            candidates.Add(settings.LastArchivePath);
        }

        candidates.Add(DefaultArchivePath);

        var detectedPath = candidates.FirstOrDefault(File.Exists);
        if (string.IsNullOrWhiteSpace(detectedPath))
        {
            SetStatus("Путь не найден");
            if (forceDetect)
            {
                AppendLog("Автопоиск не нашел update.rpf по стандартному пути Steam.");
            }

            return;
        }

        archivePathTextBox.Text = detectedPath;
        settings.LastArchivePath = detectedPath;
        SaveSettings();

        AppendLog(forceDetect
            ? $"Автопоиск нашел update.rpf: {detectedPath}"
            : $"Подготовлен путь к игре: {detectedPath}");
    }

    private ReplacementTarget GetSelectedTarget()
    {
        return targetComboBox.SelectedItem as ReplacementTarget
            ?? throw new InvalidOperationException("Не удалось определить выбранный тип замены.");
    }

    private void ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(archivePathTextBox.Text) || !File.Exists(archivePathTextBox.Text))
        {
            throw new FileNotFoundException("Укажите существующий файл update.rpf.");
        }

        if (!string.Equals(Path.GetExtension(archivePathTextBox.Text), ".rpf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Нужно выбрать именно .rpf архив.");
        }

        if (!string.Equals(Path.GetFileName(archivePathTextBox.Text), "update.rpf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Для этой программы нужен файл update.rpf из GTA V.");
        }

        if (string.IsNullOrWhiteSpace(replacementPathTextBox.Text) || !File.Exists(replacementPathTextBox.Text))
        {
            throw new FileNotFoundException("Укажите существующий файл для замены.");
        }
    }

    private void ReplaceFile(string updateRpfPath, string replacementFilePath, ReplacementTarget target)
    {
        AppendLog($"Открываю архив: {updateRpfPath}");
        CreateBackupIfNeeded(updateRpfPath);

        var replacementBytes = File.ReadAllBytes(replacementFilePath);
        AppendLog($"Загружен файл замены: {replacementFilePath} ({replacementBytes.Length:N0} байт)");

        var updateRpf = new RpfFile(updateRpfPath, "update.rpf");
        updateRpf.ScanStructure(
            message => AppendLog($"Чтение: {message}"),
            error => AppendArchiveWarning(error, "чтение"));

        var nestedArchiveEntry = FindEntry<RpfFileEntry>(updateRpf, target.ArchivePath)
            ?? throw new InvalidOperationException($"Не найден вложенный архив: {target.ArchivePath}");

        AppendLog($"Найден вложенный архив: {nestedArchiveEntry.Path}");

        var nestedArchive = updateRpf.FindChildArchive(nestedArchiveEntry)
            ?? throw new InvalidOperationException($"Не удалось открыть вложенный архив: {target.ArchivePath}");

        var targetFileEntry = FindEntry<RpfFileEntry>(nestedArchive, target.FileName)
            ?? throw new InvalidOperationException($"Не найден файл {target.FileName} внутри {target.ArchivePath}");

        var parentDirectory = targetFileEntry.Parent
            ?? throw new InvalidOperationException($"Не найдена папка-родитель для {target.FileName}.");

        AppendLog($"Старый файл в архиве: {targetFileEntry.Path}");
        AppendLog($"Удаляю старый файл: {targetFileEntry.Path}");
        RpfFile.DeleteEntry(targetFileEntry);

        AppendLog($"Записываю новый файл: {target.FileName}");
        var createdEntry = RpfFile.CreateFile(parentDirectory, target.FileName, replacementBytes, false);

        AppendLog($"Новый файл создан: {createdEntry.Path}");
        VerifyReplacementSaved(updateRpfPath, replacementBytes, target);
        AppendLog("Замена завершена.");
    }

    private void VerifyReplacementSaved(string updateRpfPath, byte[] replacementBytes, ReplacementTarget target)
    {
        AppendLog("Проверяю сохранение на диске...");

        var verificationRpf = new RpfFile(updateRpfPath, "update.rpf");
        verificationRpf.ScanStructure(
            _ => { },
            error => AppendArchiveWarning(error, "проверка"));

        var nestedArchiveEntry = FindEntry<RpfFileEntry>(verificationRpf, target.ArchivePath)
            ?? throw new InvalidOperationException($"После записи не найден архив {target.ArchivePath}");

        var nestedArchive = verificationRpf.FindChildArchive(nestedArchiveEntry)
            ?? throw new InvalidOperationException($"После записи не удалось открыть архив {target.ArchivePath}");

        var savedEntry = FindEntry<RpfFileEntry>(nestedArchive, target.FileName)
            ?? throw new InvalidOperationException($"После записи не найден файл {target.FileName}");

        var savedBytes = nestedArchive.ExtractFile(savedEntry)
            ?? throw new InvalidOperationException($"Не удалось прочитать {target.FileName} после записи.");

        var expectedHash = ComputeSha256(replacementBytes);
        var actualHash = ComputeSha256(savedBytes);

        AppendLog($"Размер нового файла: {savedBytes.Length:N0} байт");
        AppendLog($"SHA-256 ожидаемый: {expectedHash}");
        AppendLog($"SHA-256 в архиве: {actualHash}");

        if (!string.Equals(expectedHash, actualHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Файл в архиве не совпадает с выбранным файлом после записи. Замена не была подтверждена проверкой.");
        }
    }

    private void AppendArchiveWarning(string error, string stage)
    {
        totalWarningCount++;

        var firstLine = error
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()?
            .Trim();

        var key = string.IsNullOrWhiteSpace(firstLine) ? $"unknown:{stage}" : $"{stage}:{firstLine}";
        warningCounters.TryGetValue(key, out var count);
        warningCounters[key] = count + 1;

        if (count < 2)
        {
            AppendLog($"Предупреждение ({stage}): {firstLine}");
        }
    }

    private void FlushWarningSummary()
    {
        if (totalWarningCount <= 0)
        {
            return;
        }

        AppendLog($"Служебных предупреждений CodeWalker: {totalWarningCount}");
        AppendLog("Они не мешают результату, если финальная SHA-256 проверка совпала.");
    }

    private static string ComputeSha256(byte[] data)
    {
        return Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
    }

    private static TEntry? FindEntry<TEntry>(RpfFile archive, string normalizedPath)
        where TEntry : RpfEntry
    {
        var expectedPath = normalizedPath.Replace('\\', '/').Trim('/').ToLowerInvariant();

        return archive.AllEntries
            .OfType<TEntry>()
            .FirstOrDefault(entry =>
                string.Equals(NormalizePath(entry.Path), expectedPath, StringComparison.OrdinalIgnoreCase)
                || NormalizePath(entry.Path).EndsWith("/" + expectedPath, StringComparison.OrdinalIgnoreCase)
                || string.Equals(entry.Name, expectedPath, StringComparison.OrdinalIgnoreCase));
    }

    private static string NormalizePath(string path)
    {
        return path.Replace('\\', '/').Trim('/').ToLowerInvariant();
    }

    private void CreateBackupIfNeeded(string updateRpfPath)
    {
        var backupPath = $"{updateRpfPath}.bak";
        if (File.Exists(backupPath))
        {
            AppendLog($"Резервная копия уже существует: {backupPath}");
            return;
        }

        File.Copy(updateRpfPath, backupPath);
        AppendLog($"Создана резервная копия: {backupPath}");
    }

    private void AppendLog(string message)
    {
        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(() => AppendLog(message));
            return;
        }

        logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        logTextBox.SelectionStart = logTextBox.TextLength;
        logTextBox.ScrollToCaret();
    }

    private void SetStatus(string message)
    {
        if (statusValueLabel.InvokeRequired)
        {
            statusValueLabel.Invoke(() => SetStatus(message));
            return;
        }

        statusValueLabel.Text = message;
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(settingsPath))
            {
                var json = File.ReadAllText(settingsPath);
                settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch
        {
            settings = new AppSettings();
        }

        themeToggle.Checked = settings.UseDarkTheme;
        currentTheme = settings.UseDarkTheme ? ThemePalette.Dark : ThemePalette.Light;
    }

    private void SaveSettings()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
        File.WriteAllText(settingsPath, JsonSerializer.Serialize(settings, JsonOptions));
    }

    private void FadeInTimer_Tick(object? sender, EventArgs e)
    {
        Opacity += 0.08;
        if (Opacity >= 1)
        {
            Opacity = 1;
            fadeInTimer.Stop();
        }
    }

    private void ApplyTheme(ThemePalette palette)
    {
        BackColor = palette.Window;

        headerPanel.SurfaceColor = palette.Header;
        archiveCard.SurfaceColor = palette.Surface;
        targetCard.SurfaceColor = palette.Surface;
        replacementCard.SurfaceColor = palette.Surface;
        footerCard.SurfaceColor = palette.Surface;
        logCard.SurfaceColor = palette.Surface;

        foreach (var panel in new[] { headerPanel, archiveCard, targetCard, replacementCard, footerCard, logCard })
        {
            panel.BorderColor = palette.Border;
            panel.ShadowColor = palette.Shadow;
            panel.Invalidate();
        }

        titleLabel.ForeColor = palette.Text;
        subtitleLabel.ForeColor = palette.SubtleText;
        archiveLabel.ForeColor = palette.SubtleText;
        targetLabel.ForeColor = palette.SubtleText;
        replacementLabel.ForeColor = palette.SubtleText;
        pathHintLabel.ForeColor = palette.SubtleText;
        targetHintLabel.ForeColor = palette.SubtleText;
        statusCaptionLabel.ForeColor = palette.SubtleText;
        themeCaptionLabel.ForeColor = palette.SubtleText;
        logLabel.ForeColor = palette.SubtleText;

        archivePathTextBox.BackColor = palette.Input;
        archivePathTextBox.ForeColor = palette.Text;
        replacementPathTextBox.BackColor = palette.Input;
        replacementPathTextBox.ForeColor = palette.Text;
        targetComboBox.BackColor = palette.Input;
        targetComboBox.ForeColor = palette.Text;
        logTextBox.BackColor = palette.Input;
        logTextBox.ForeColor = palette.Text;

        browseArchiveButton.ApplyPalette(palette.SecondaryButton, palette.Text);
        browseReplacementButton.ApplyPalette(palette.SecondaryButton, palette.Text);
        autoDetectButton.ApplyPalette(palette.AccentSoft, palette.Text);
        replaceButton.ApplyPalette(palette.AccentStrong, Color.White);
        themeToggle.ApplyPalette(palette.AccentStrong, palette.Border, palette.SecondaryButton);
        statusValueLabel.ForeColor = palette.AccentStrong;
    }

    private sealed record ReplacementTarget(
        string DisplayName,
        string Description,
        string ArchivePath,
        string FileName);

    private sealed class AppSettings
    {
        public string LastArchivePath { get; set; } = string.Empty;
        public bool UseDarkTheme { get; set; }
    }

    private sealed record ThemePalette(
        Color Window,
        Color Header,
        Color Surface,
        Color Input,
        Color Border,
        Color Shadow,
        Color Text,
        Color SubtleText,
        Color SecondaryButton,
        Color AccentSoft,
        Color AccentStrong)
    {
        public static ThemePalette Light { get; } = new(
            Color.FromArgb(240, 244, 252),
            Color.FromArgb(250, 252, 255),
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(246, 248, 252),
            Color.FromArgb(220, 226, 238),
            Color.FromArgb(24, 32, 54, 18),
            Color.FromArgb(28, 34, 48),
            Color.FromArgb(95, 104, 120),
            Color.FromArgb(228, 235, 245),
            Color.FromArgb(214, 240, 233),
            Color.FromArgb(20, 122, 102));

        public static ThemePalette Dark { get; } = new(
            Color.FromArgb(17, 22, 32),
            Color.FromArgb(23, 29, 42),
            Color.FromArgb(26, 34, 48),
            Color.FromArgb(31, 41, 59),
            Color.FromArgb(53, 66, 91),
            Color.FromArgb(0, 0, 0, 48),
            Color.FromArgb(239, 244, 255),
            Color.FromArgb(161, 172, 192),
            Color.FromArgb(43, 54, 77),
            Color.FromArgb(34, 74, 63),
            Color.FromArgb(67, 200, 160));

    }
}
