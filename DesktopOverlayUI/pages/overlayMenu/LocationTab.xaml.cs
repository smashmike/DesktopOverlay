using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopOverlayUI.pages.overlayMenu;

/// <summary>
/// Interaction logic for LocationTab.xaml
/// </summary>
public partial class LocationTab : Page
{
    private OverlayDisplay _overlay;
    private Point _overlayPoint;
    private Point _offsetPoint;

    private System.Diagnostics.Process _targetProcess;
    private DispatcherTimer _timer;

    public LocationTab(OverlayDisplay overlay)
    {
        _overlay = overlay;
        var processes = System.Diagnostics.Process.GetProcesses();
        var processList = new List<System.Diagnostics.Process>();
        foreach (var process in processes)
            if (!string.IsNullOrEmpty(process.MainWindowTitle))
                processList.Add(process);
        _overlayPoint = new Point(0, 0);
        _offsetPoint = new Point(0, 0);
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += OnTimerTick;
        InitializeComponent();
        WindowsComboBox.ItemsSource = processList;
    }

    private void UpdateZLevel(object sender, RoutedEventArgs e)
    {
        if (TopMostButton.IsChecked != null && TopMostButton.IsChecked.Value)
        {
            _timer.Stop();
            _overlay.Topmost = true;
            _overlayPoint = new Point(0, 0);
            _overlay.Left = _offsetPoint.X;
            _overlay.Top = _offsetPoint.Y;
        }
        else if (AttachedButton.IsChecked != null && AttachedButton.IsChecked.Value)
        {
            var selectedProcess = (System.Diagnostics.Process)WindowsComboBox.SelectedItem;
            if (selectedProcess != null)
            {
                var windowRect = new RECT();
                GetWindowRect(selectedProcess.MainWindowHandle, ref windowRect);
                _targetProcess = selectedProcess;
                _overlayPoint = new Point(windowRect.Left + _offsetPoint.X, windowRect.Top + _offsetPoint.Y);
                _overlay.Left = _overlayPoint.X;
                _overlay.Top = _overlayPoint.Y;
                _timer.Start();
            }
        }
    }

    private void UpdateOffset(object sender, TextChangedEventArgs e)
    {
        if (OffsetXTextBox == null || OffsetYTextBox == null) return;
        if (!System.Text.RegularExpressions.Regex.IsMatch(OffsetXTextBox.Text, @"\A\b[0-9]+\b\Z"))
        {
            OffsetXTextBox.Text = "";
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(OffsetYTextBox.Text, @"\A\b[0-9]+\b\Z"))
        {
            OffsetYTextBox.Text = "";
        }
        if (string.IsNullOrEmpty(OffsetXTextBox.Text) || string.IsNullOrEmpty(OffsetYTextBox.Text)) return;
        _offsetPoint = new Point(int.Parse(OffsetXTextBox.Text), int.Parse(OffsetYTextBox.Text));
        _overlay.Left = _overlayPoint.X + _offsetPoint.X;
        _overlay.Top = _overlayPoint.Y + _offsetPoint.Y;
    }

    private void WindowsComboBox_DropDownOpened(object sender, EventArgs e)
    {
        var processes = System.Diagnostics.Process.GetProcesses();
        var processList = new List<System.Diagnostics.Process>();
        foreach (var process in processes)
            if (!string.IsNullOrEmpty(process.MainWindowTitle) &&
                process.MainWindowHandle != System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle)
                processList.Add(process);

        WindowsComboBox.ItemsSource = processList;
    }

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);


    private void OnTimerTick(object sender, EventArgs e)
    {
        if (_overlay.IsVisible == false)
        {
            return;
        }
        var windowRect = new RECT();
        GetWindowRect(_targetProcess.MainWindowHandle, ref windowRect);

        if (_overlay.Left != windowRect.Left || _overlay.Top != windowRect.Top)
        {
            _overlayPoint = new Point(windowRect.Left + _offsetPoint.X, windowRect.Top + _offsetPoint.Y);
            _overlay.Left = _overlayPoint.X;
            _overlay.Top = _overlayPoint.Y;
        }
    }
}

public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}