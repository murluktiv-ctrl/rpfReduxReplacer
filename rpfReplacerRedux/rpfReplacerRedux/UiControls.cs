using System.Drawing.Drawing2D;

namespace rpfReplacerRedux;

internal sealed class RoundedPanel : Panel
{
    public int CornerRadius { get; set; } = 24;
    public Color SurfaceColor { get; set; } = Color.White;
    public Color BorderColor { get; set; } = Color.FromArgb(220, 226, 238);
    public Color ShadowColor { get; set; } = Color.FromArgb(24, 32, 54, 18);

    public RoundedPanel()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = CreatePath(rect, CornerRadius);
        using var fillBrush = new SolidBrush(SurfaceColor);
        using var borderPen = new Pen(BorderColor, 1f);
        e.Graphics.FillPath(fillBrush, path);
        e.Graphics.DrawPath(borderPen, path);
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        if (Width > 0 && Height > 0)
        {
            using var path = CreatePath(new Rectangle(0, 0, Width, Height), CornerRadius);
            Region = new Region(path);
        }
    }

    private static GraphicsPath CreatePath(Rectangle rectangle, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class ModernButton : Button
{
    private Color baseColor = Color.FromArgb(228, 235, 245);
    private Color hoverColor = Color.FromArgb(216, 225, 240);
    private Color currentBackColor = Color.FromArgb(228, 235, 245);
    private Color currentForeColor = Color.FromArgb(28, 34, 48);

    public ModernButton()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
        BackColor = Color.Transparent;
    }

    public void ApplyPalette(Color background, Color foreground)
    {
        baseColor = background;
        hoverColor = Shift(background, -14);
        currentBackColor = background;
        currentForeColor = foreground;
        ForeColor = foreground;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        currentBackColor = hoverColor;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        currentBackColor = baseColor;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        pevent.Graphics.Clear(GetHostColor());
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = CreatePath(rect, 16);
        using var brush = new SolidBrush(currentBackColor);
        pevent.Graphics.FillPath(brush, path);
        TextRenderer.DrawText(
            pevent.Graphics,
            Text,
            Font,
            rect,
            currentForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (Width > 0 && Height > 0)
        {
            using var path = CreatePath(new Rectangle(0, 0, Width, Height), 16);
            Region = new Region(path);
        }
    }

    private static GraphicsPath CreatePath(Rectangle rectangle, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static Color Shift(Color color, int delta)
    {
        return Color.FromArgb(
            color.A,
            Math.Clamp(color.R + delta, 0, 255),
            Math.Clamp(color.G + delta, 0, 255),
            Math.Clamp(color.B + delta, 0, 255));
    }

    private Color GetHostColor()
    {
        return Parent is RoundedPanel panel ? panel.SurfaceColor : Parent?.BackColor ?? SystemColors.Control;
    }
}

internal sealed class ThemeToggle : CheckBox
{
    private Color accentColor = Color.FromArgb(20, 122, 102);
    private Color borderColor = Color.FromArgb(220, 226, 238);
    private Color surfaceColor = Color.White;

    public ThemeToggle()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);
        MinimumSize = new Size(46, 22);
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;
    }

    public void ApplyPalette(Color accent, Color border, Color surface)
    {
        accentColor = accent;
        borderColor = border;
        surfaceColor = surface;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        pevent.Graphics.Clear(GetHostColor());

        var track = new Rectangle(0, 0, Width - 1, Height - 1);
        using var trackPath = CreateTrack(track, Height - 1);
        using var trackBrush = new SolidBrush(Checked ? accentColor : surfaceColor);
        using var borderPen = new Pen(Checked ? accentColor : borderColor, 1f);
        pevent.Graphics.FillPath(trackBrush, trackPath);
        pevent.Graphics.DrawPath(borderPen, trackPath);

        var knobSize = Height - 6;
        var knobX = Checked ? Width - knobSize - 4 : 4;
        var knobRect = new Rectangle(knobX, 3, knobSize, knobSize);
        using var knobBrush = new SolidBrush(Color.White);
        pevent.Graphics.FillEllipse(knobBrush, knobRect);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (Width > 0 && Height > 0)
        {
            using var path = CreateTrack(new Rectangle(0, 0, Width, Height), Height);
            Region = new Region(path);
        }
    }

    private static GraphicsPath CreateTrack(Rectangle rectangle, int radius)
    {
        var path = new GraphicsPath();
        path.AddArc(rectangle.X, rectangle.Y, radius, radius, 90, 180);
        path.AddArc(rectangle.Right - radius, rectangle.Y, radius, radius, 270, 180);
        path.CloseFigure();
        return path;
    }

    private Color GetHostColor()
    {
        return Parent is RoundedPanel panel ? panel.SurfaceColor : Parent?.BackColor ?? SystemColors.Control;
    }
}
