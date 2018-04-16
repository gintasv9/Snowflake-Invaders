using System;
using System.Runtime.InteropServices;
// add'inau Forms ir Drawing i references
using System.Drawing;
using System.Windows.Forms;

public static class SafeNativeMethods
{
    private const int MF_BYCOMMAND = 0x00000000;
    public const int SC_CLOSE = 0xF060;
    public const int SC_MINIMIZE = 0xF020;
    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;

    public static void CenterConsole()
    {
        IntPtr hWin = GetConsoleWindow();
        GetWindowRect(hWin, out RECT rc);
        Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));
        int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
        int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
        MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, false);
    }

    public static void DisableConsoleResize()
    {
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);

        IntPtr sysMenu = GetSystemMenu(GetConsoleWindow(), false);
        DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
    }

    // P/Invoke declarations
    private struct RECT { public int left, top, right, bottom; }
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
    [DllImport("user32.dll")]
    private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
}