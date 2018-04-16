using System;
using System.Runtime.InteropServices;
// add'inau Forms ir Drawing i references
using System.Drawing;
using System.Windows.Forms;

public static class SafeNativeMethods
{
    private const int MF_BYCOMMAND = 0x00000000;
    private const int SC_CLOSE = 0xF060;
    private const int SC_MINIMIZE = 0xF020;
    private const int SC_MAXIMIZE = 0xF030;
    private const int SC_SIZE = 0xF000;
    private const uint ENABLE_QUICK_EDIT = 0x0040;
    // STD_INPUT_HANDLE (DWORD): -10 is the standard input device.
    private const int STD_INPUT_HANDLE = -10;

    internal static void CenterConsole()
    {
        IntPtr hWin = GetConsoleWindow();
        GetWindowRect(hWin, out RECT rc);
        Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));
        int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
        int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
        MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, false);
    }

    internal static void DisableConsoleResize()
    {
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);

        IntPtr sysMenu = GetSystemMenu(GetConsoleWindow(), false);
        DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
    }

    internal static bool DisableConsoleMouseClicks()
    {
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

        // get current console mode
        uint consoleMode;
        if (!GetConsoleMode(consoleHandle, out consoleMode))
        {
            // ERROR: Unable to get console mode.
            return false;
        }

        // Clear the quick edit bit in the mode flags
        consoleMode &= ~ENABLE_QUICK_EDIT;

        // set the new mode
        if (!SetConsoleMode(consoleHandle, consoleMode))
        {
            // ERROR: Unable to set console mode
            return false;
        }
        return true;
    }

    // P/Invoke declarations
    private struct RECT { public int left, top, right, bottom; }
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetConsoleWindow();
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
    [DllImport("user32.dll")]
    private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
}
