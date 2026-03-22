#nullable enable

namespace rpfReplacerRedux;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private RoundedPanel headerPanel = null!;
    private RoundedPanel archiveCard = null!;
    private RoundedPanel targetCard = null!;
    private RoundedPanel replacementCard = null!;
    private RoundedPanel footerCard = null!;
    private RoundedPanel logCard = null!;
    private Label titleLabel = null!;
    private Label subtitleLabel = null!;
    private Label archiveLabel = null!;
    private TextBox archivePathTextBox = null!;
    private ModernButton browseArchiveButton = null!;
    private ModernButton autoDetectButton = null!;
    private Label pathHintLabel = null!;
    private Label targetLabel = null!;
    private ComboBox targetComboBox = null!;
    private Label targetHintLabel = null!;
    private Label replacementLabel = null!;
    private TextBox replacementPathTextBox = null!;
    private ModernButton browseReplacementButton = null!;
    private ModernButton replaceButton = null!;
    private Label statusCaptionLabel = null!;
    private Label statusValueLabel = null!;
    private Label themeCaptionLabel = null!;
    private ThemeToggle themeToggle = null!;
    private Label logLabel = null!;
    private TextBox logTextBox = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        headerPanel = new RoundedPanel();
        titleLabel = new Label();
        subtitleLabel = new Label();
        archiveCard = new RoundedPanel();
        archiveLabel = new Label();
        archivePathTextBox = new TextBox();
        browseArchiveButton = new ModernButton();
        autoDetectButton = new ModernButton();
        pathHintLabel = new Label();
        targetCard = new RoundedPanel();
        targetLabel = new Label();
        targetComboBox = new ComboBox();
        targetHintLabel = new Label();
        replacementCard = new RoundedPanel();
        replacementLabel = new Label();
        replacementPathTextBox = new TextBox();
        browseReplacementButton = new ModernButton();
        footerCard = new RoundedPanel();
        replaceButton = new ModernButton();
        statusCaptionLabel = new Label();
        statusValueLabel = new Label();
        themeCaptionLabel = new Label();
        themeToggle = new ThemeToggle();
        logCard = new RoundedPanel();
        logLabel = new Label();
        logTextBox = new TextBox();
        headerPanel.SuspendLayout();
        archiveCard.SuspendLayout();
        targetCard.SuspendLayout();
        replacementCard.SuspendLayout();
        footerCard.SuspendLayout();
        logCard.SuspendLayout();
        SuspendLayout();
        // 
        // headerPanel
        // 
        headerPanel.Controls.Add(titleLabel);
        headerPanel.Controls.Add(subtitleLabel);
        headerPanel.CornerRadius = 28;
        headerPanel.Location = new Point(24, 20);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(24, 20, 24, 20);
        headerPanel.Size = new Size(872, 108);
        headerPanel.TabIndex = 0;
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point, 204);
        titleLabel.Location = new Point(22, 16);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(327, 45);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "GTA V RPF Replacer";
        // 
        // subtitleLabel
        // 
        subtitleLabel.AutoSize = true;
        subtitleLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
        subtitleLabel.Location = new Point(24, 66);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(664, 19);
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "Автоматическая замена hud_reticle.gfx и minimap.gfx внутри update.rpf с проверкой результата.";
        // 
        // archiveCard
        // 
        archiveCard.Controls.Add(archiveLabel);
        archiveCard.Controls.Add(archivePathTextBox);
        archiveCard.Controls.Add(browseArchiveButton);
        archiveCard.Controls.Add(autoDetectButton);
        archiveCard.Controls.Add(pathHintLabel);
        archiveCard.CornerRadius = 24;
        archiveCard.Location = new Point(24, 146);
        archiveCard.Name = "archiveCard";
        archiveCard.Padding = new Padding(20);
        archiveCard.Size = new Size(872, 134);
        archiveCard.TabIndex = 1;
        // 
        // archiveLabel
        // 
        archiveLabel.AutoSize = true;
        archiveLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 204);
        archiveLabel.Location = new Point(22, 18);
        archiveLabel.Name = "archiveLabel";
        archiveLabel.Size = new Size(134, 20);
        archiveLabel.TabIndex = 0;
        archiveLabel.Text = "Путь к update.rpf";
        // 
        // archivePathTextBox
        // 
        archivePathTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
        archivePathTextBox.Location = new Point(24, 49);
        archivePathTextBox.Name = "archivePathTextBox";
        archivePathTextBox.PlaceholderText = "Программа запомнит этот путь для следующего запуска";
        archivePathTextBox.Size = new Size(525, 25);
        archivePathTextBox.TabIndex = 1;
        // 
        // browseArchiveButton
        // 
        browseArchiveButton.FlatAppearance.BorderSize = 0;
        browseArchiveButton.FlatStyle = FlatStyle.Flat;
        browseArchiveButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
        browseArchiveButton.Location = new Point(562, 46);
        browseArchiveButton.Name = "browseArchiveButton";
        browseArchiveButton.Size = new Size(142, 34);
        browseArchiveButton.TabIndex = 2;
        browseArchiveButton.Text = "Выбрать файл";
        browseArchiveButton.UseVisualStyleBackColor = true;
        browseArchiveButton.Click += browseArchiveButton_Click;
        // 
        // autoDetectButton
        // 
        autoDetectButton.FlatAppearance.BorderSize = 0;
        autoDetectButton.FlatStyle = FlatStyle.Flat;
        autoDetectButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
        autoDetectButton.Location = new Point(718, 46);
        autoDetectButton.Name = "autoDetectButton";
        autoDetectButton.Size = new Size(129, 34);
        autoDetectButton.TabIndex = 3;
        autoDetectButton.Text = "Автопоиск";
        autoDetectButton.UseVisualStyleBackColor = true;
        autoDetectButton.Click += autoDetectButton_Click;
        // 
        // pathHintLabel
        // 
        pathHintLabel.Location = new Point(24, 89);
        pathHintLabel.Name = "pathHintLabel";
        pathHintLabel.Size = new Size(823, 26);
        pathHintLabel.TabIndex = 4;
        pathHintLabel.Text = "Сначала используется сохраненный путь, если он валиден. Иначе программа ищет стандартный Steam-путь автоматически.";
        // 
        // targetCard
        // 
        targetCard.Controls.Add(targetLabel);
        targetCard.Controls.Add(targetComboBox);
        targetCard.Controls.Add(targetHintLabel);
        targetCard.CornerRadius = 24;
        targetCard.Location = new Point(24, 294);
        targetCard.Name = "targetCard";
        targetCard.Padding = new Padding(20);
        targetCard.Size = new Size(424, 144);
        targetCard.TabIndex = 2;
        // 
        // targetLabel
        // 
        targetLabel.AutoSize = true;
        targetLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 204);
        targetLabel.Location = new Point(22, 18);
        targetLabel.Name = "targetLabel";
        targetLabel.Size = new Size(114, 20);
        targetLabel.TabIndex = 0;
        targetLabel.Text = "Что заменить";
        // 
        // targetComboBox
        // 
        targetComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        targetComboBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
        targetComboBox.FormattingEnabled = true;
        targetComboBox.Location = new Point(24, 49);
        targetComboBox.Name = "targetComboBox";
        targetComboBox.Size = new Size(374, 25);
        targetComboBox.TabIndex = 1;
        targetComboBox.SelectedIndexChanged += targetComboBox_SelectedIndexChanged;
        // 
        // targetHintLabel
        // 
        targetHintLabel.Location = new Point(24, 88);
        targetHintLabel.Name = "targetHintLabel";
        targetHintLabel.Size = new Size(374, 36);
        targetHintLabel.TabIndex = 2;
        targetHintLabel.Text = "Описание цели замены";
        // 
        // replacementCard
        // 
        replacementCard.Controls.Add(replacementLabel);
        replacementCard.Controls.Add(replacementPathTextBox);
        replacementCard.Controls.Add(browseReplacementButton);
        replacementCard.CornerRadius = 24;
        replacementCard.Location = new Point(472, 294);
        replacementCard.Name = "replacementCard";
        replacementCard.Padding = new Padding(20);
        replacementCard.Size = new Size(424, 144);
        replacementCard.TabIndex = 3;
        // 
        // replacementLabel
        // 
        replacementLabel.AutoSize = true;
        replacementLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 204);
        replacementLabel.Location = new Point(22, 18);
        replacementLabel.Name = "replacementLabel";
        replacementLabel.Size = new Size(170, 20);
        replacementLabel.TabIndex = 0;
        replacementLabel.Text = "Файл для подстановки";
        // 
        // replacementPathTextBox
        // 
        replacementPathTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
        replacementPathTextBox.Location = new Point(24, 49);
        replacementPathTextBox.Name = "replacementPathTextBox";
        replacementPathTextBox.PlaceholderText = "Выберите .gfx файл";
        replacementPathTextBox.Size = new Size(374, 25);
        replacementPathTextBox.TabIndex = 1;
        // 
        // browseReplacementButton
        // 
        browseReplacementButton.FlatAppearance.BorderSize = 0;
        browseReplacementButton.FlatStyle = FlatStyle.Flat;
        browseReplacementButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
        browseReplacementButton.Location = new Point(24, 90);
        browseReplacementButton.Name = "browseReplacementButton";
        browseReplacementButton.Size = new Size(164, 36);
        browseReplacementButton.TabIndex = 2;
        browseReplacementButton.Text = "Выбрать замену";
        browseReplacementButton.UseVisualStyleBackColor = true;
        browseReplacementButton.Click += browseReplacementButton_Click;
        // 
        // footerCard
        // 
        footerCard.Controls.Add(replaceButton);
        footerCard.Controls.Add(statusCaptionLabel);
        footerCard.Controls.Add(statusValueLabel);
        footerCard.Controls.Add(themeCaptionLabel);
        footerCard.Controls.Add(themeToggle);
        footerCard.CornerRadius = 24;
        footerCard.Location = new Point(24, 454);
        footerCard.Name = "footerCard";
        footerCard.Padding = new Padding(20);
        footerCard.Size = new Size(872, 88);
        footerCard.TabIndex = 4;
        // 
        // replaceButton
        // 
        replaceButton.FlatAppearance.BorderSize = 0;
        replaceButton.FlatStyle = FlatStyle.Flat;
        replaceButton.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 204);
        replaceButton.Location = new Point(24, 22);
        replaceButton.Name = "replaceButton";
        replaceButton.Size = new Size(212, 42);
        replaceButton.TabIndex = 0;
        replaceButton.Text = "Заменить файл в архиве";
        replaceButton.UseVisualStyleBackColor = true;
        replaceButton.Click += replaceButton_Click;
        // 
        // statusCaptionLabel
        // 
        statusCaptionLabel.AutoSize = true;
        statusCaptionLabel.Location = new Point(270, 34);
        statusCaptionLabel.Name = "statusCaptionLabel";
        statusCaptionLabel.Size = new Size(47, 15);
        statusCaptionLabel.TabIndex = 1;
        statusCaptionLabel.Text = "Статус:";
        // 
        // statusValueLabel
        // 
        statusValueLabel.AutoSize = true;
        statusValueLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
        statusValueLabel.Location = new Point(323, 31);
        statusValueLabel.Name = "statusValueLabel";
        statusValueLabel.Size = new Size(78, 19);
        statusValueLabel.TabIndex = 2;
        statusValueLabel.Text = "Ожидание";
        // 
        // themeCaptionLabel
        // 
        themeCaptionLabel.AutoSize = true;
        themeCaptionLabel.Location = new Point(673, 34);
        themeCaptionLabel.Name = "themeCaptionLabel";
        themeCaptionLabel.Size = new Size(38, 15);
        themeCaptionLabel.TabIndex = 3;
        themeCaptionLabel.Text = "Тема:";
        // 
        // themeToggle
        // 
        themeToggle.AutoSize = true;
        themeToggle.Location = new Point(725, 30);
        themeToggle.Name = "themeToggle";
        themeToggle.Size = new Size(46, 22);
        themeToggle.TabIndex = 4;
        themeToggle.UseVisualStyleBackColor = true;
        themeToggle.CheckedChanged += themeToggle_CheckedChanged;
        // 
        // logCard
        // 
        logCard.Controls.Add(logLabel);
        logCard.Controls.Add(logTextBox);
        logCard.CornerRadius = 24;
        logCard.Location = new Point(24, 558);
        logCard.Name = "logCard";
        logCard.Padding = new Padding(20);
        logCard.Size = new Size(872, 218);
        logCard.TabIndex = 5;
        // 
        // logLabel
        // 
        logLabel.AutoSize = true;
        logLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 204);
        logLabel.Location = new Point(22, 18);
        logLabel.Name = "logLabel";
        logLabel.Size = new Size(104, 20);
        logLabel.TabIndex = 0;
        logLabel.Text = "Журнал замены";
        // 
        // logTextBox
        // 
        logTextBox.BorderStyle = BorderStyle.None;
        logTextBox.Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
        logTextBox.Location = new Point(24, 49);
        logTextBox.Multiline = true;
        logTextBox.Name = "logTextBox";
        logTextBox.ReadOnly = true;
        logTextBox.ScrollBars = ScrollBars.Vertical;
        logTextBox.Size = new Size(823, 148);
        logTextBox.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(920, 798);
        Controls.Add(logCard);
        Controls.Add(footerCard);
        Controls.Add(replacementCard);
        Controls.Add(targetCard);
        Controls.Add(archiveCard);
        Controls.Add(headerPanel);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "GTA V RPF Replacer";
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        archiveCard.ResumeLayout(false);
        archiveCard.PerformLayout();
        targetCard.ResumeLayout(false);
        targetCard.PerformLayout();
        replacementCard.ResumeLayout(false);
        replacementCard.PerformLayout();
        footerCard.ResumeLayout(false);
        footerCard.PerformLayout();
        logCard.ResumeLayout(false);
        logCard.PerformLayout();
        ResumeLayout(false);
    }
}
