using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DesktopOverlay.pages.overlayMenu;

/// <summary>
///     Interaction logic for LocationTab.xaml
/// </summary>
public partial class LocationTab : Page
{
    private readonly OverlayDriver _overlayDriver;
    private readonly DispatcherTimer _timer;
    private Point _offsetPoint;
    private Point _overlayPoint;

    private Process? _targetProcess;

    public LocationTab(OverlayDriver driver)
    {
        _overlayDriver = driver;
        var processes = Process.GetProcesses();
        var processList = new List<Process>();
        foreach (var process in processes)
            if (!string.IsNullOrEmpty(process.MainWindowTitle))
                processList.Add(process);
        _overlayPoint = new Point(0, 0);
        _offsetPoint = new Point(0, 0);
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        //_timer.Tick += OnTimerTick;
        InitializeComponent();
        WindowsComboBox.ItemsSource = processList;
    }

    private void UpdateZLevel(object sender, RoutedEventArgs e)
    {
        if (TopMostButton.IsChecked != null && TopMostButton.IsChecked.Value)
        {
            _timer.Stop();
            _overlayPoint = new Point(0, 0);
            _overlayDriver.SetAttach(false);
        }
        else if (AttachedButton.IsChecked != null && AttachedButton.IsChecked.Value)
        {
            var selectedProcess = (Process)WindowsComboBox.SelectedItem;
            if (selectedProcess != null)
            {
                var windowRect = new Rect();
                GetWindowRect(selectedProcess.MainWindowHandle, ref windowRect);
                _targetProcess = selectedProcess;
                _overlayDriver.SetTarget(_targetProcess);
                _overlayPoint = new Point(windowRect.Left + _offsetPoint.X, windowRect.Top + _offsetPoint.Y);
                _timer.Start();
            }
        }
    }

    private void UpdateOffset(object sender, TextChangedEventArgs e)
    {
        if (OffsetXTextBox == null || OffsetYTextBox == null) return;
        if (!Regex.IsMatch(OffsetXTextBox.Text, @"\A\b[0-9]+\b\Z"))
            OffsetXTextBox.Text = "";

        if (!Regex.IsMatch(OffsetYTextBox.Text, @"\A\b[0-9]+\b\Z"))
            OffsetYTextBox.Text = "";
        if (string.IsNullOrEmpty(OffsetXTextBox.Text) || string.IsNullOrEmpty(OffsetYTextBox.Text)) return;
        _offsetPoint = new Point(int.Parse(OffsetXTextBox.Text), int.Parse(OffsetYTextBox.Text));
        _overlayDriver.SetOffset((int)_offsetPoint.X, (int)_offsetPoint.Y);
    }

    private void WindowsComboBox_DropDownOpened(object sender, EventArgs e)
    {
        var processes = Process.GetProcesses();
        var processList = new List<Process>();
        foreach (var process in processes)
            if (!string.IsNullOrEmpty(process.MainWindowTitle) &&
                process.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle)
                processList.Add(process);

        WindowsComboBox.ItemsSource = processList;
    }

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rect);


    private void OnTimerTick(object? sender, EventArgs e)
    {
        var windowRect = new Rect();
        if (_targetProcess != null) GetWindowRect(_targetProcess.MainWindowHandle, ref windowRect);

        //if (Math.Abs(_base.Left - windowRect.Left) > 0.1 || Math.Abs(_base.Top - windowRect.Top) > 0.1)
        if (windowRect.Left >= 0 && windowRect.Top >= 0)
            _overlayPoint = new Point(windowRect.Left + _offsetPoint.X, windowRect.Top + _offsetPoint.Y);
        if (windowRect.Left < 0) _overlayPoint.X = 0;
        if (windowRect.Top < 0) _overlayPoint.Y = 0;
    }

    private void UpdateWindowHeight(object sender, RoutedEventArgs e)
    {
        if (ZLevelBox == null) return;
        if (ZLevelBox.Value == null) return;
        if (!Regex.IsMatch(ZLevelBox.Value.ToString()!, @"\A\b[0-9]+\b\Z") || (int)ZLevelBox.Value > 10)
            ZLevelBox.Text = "";

        ZLevelBox.Value = (int)ZLevelBox.Value;
        _overlayDriver.SetZLevel((int)ZLevelBox.Value);
    }
}

public struct Rect(int left, int top)
{
    public readonly int Left = left;
    public readonly int Top = top;
    public int Right;
    public int Bottom;
}