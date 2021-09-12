/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/
namespace K2host.Core.Enums
{

    public enum OWindowMessages : int
    {
        WM_NULL = 0,
        WM_CREATE = 1,
        WM_DESTROY = 2,
        WM_MOVE = 3,
        WM_SIZE = 5,
        WM_ACTIVATE = 6,
        WM_SETFOCUS = 7,
        WM_KILLFOCUS = 8,
        WM_ENABLE = 10,
        WM_SETREDRAW = 11,
        WM_SETTEXT = 12,
        WM_GETTEXT = 13,
        WM_GETTEXTLENGTH = 14,
        WM_PAINT = 15,
        WM_CLOSE = 16,

        WM_COPYDATA = 0x004A,
        WM_USER = 0x400,

        WM_QUIT = 18,
        WM_ERASEBKGND = 20,
        WM_SYSCOLORCHANGE = 21,
        WM_SHOWWINDOW = 24,

        WM_ACTIVATEAPP = 28,

        WM_SETCURSOR = 32,
        WM_MOUSEACTIVATE = 33,
        WM_GETMINMAXINFO = 36,
        WM_WINDOWPOSCHANGING = 70,
        WM_WINDOWPOSCHANGED = 71,

        WM_CONTEXTMENU = 123,
        WM_STYLECHANGING = 124,
        WM_STYLECHANGED = 125,
        WM_DISPLAYCHANGE = 126,
        WM_GETICON = 127,
        WM_SETICON = 128,

        // non client area 
        WM_NCCREATE = 129,
        WM_NCDESTROY = 130,
        WM_NCCALCSIZE = 131,
        WM_NCHITTEST = 132,
        WM_NCPAINT = 133,
        WM_NCACTIVATE = 134,

        WM_GETDLGCODE = 135,

        WM_SYNCPAINT = 136,

        // non client mouse 
        WM_NCMOUSEMOVE = 160,
        WM_NCLBUTTONDOWN = 161,
        WM_NCLBUTTONUP = 162,
        WM_NCLBUTTONDBLCLK = 163,
        WM_NCRBUTTONDOWN = 164,
        WM_NCRBUTTONUP = 165,
        WM_NCRBUTTONDBLCLK = 166,
        WM_NCMBUTTONDOWN = 167,
        WM_NCMBUTTONUP = 168,
        WM_NCMBUTTONDBLCLK = 169,

        // keyboard 
        WM_KEYDOWN = 256,
        WM_KEYUP = 257,
        WM_CHAR = 258,

        WM_SYSCOMMAND = 274,

        // menu 
        WM_INITMENU = 278,
        WM_INITMENUPOPUP = 279,
        WM_MENUSELECT = 287,
        WM_MENUCHAR = 288,
        WM_ENTERIDLE = 289,
        WM_MENURBUTTONUP = 290,
        WM_MENUDRAG = 291,
        WM_MENUGETOBJECT = 292,
        WM_UNINITMENUPOPUP = 293,
        WM_MENUCOMMAND = 294,

        WM_CHANGEUISTATE = 295,
        WM_UPDATEUISTATE = 296,
        WM_QUERYUISTATE = 297,

        // mouse 
        WM_MOUSEFIRST = 512,
        WM_MOUSEMOVE = 512,
        WM_LBUTTONDOWN = 513,
        WM_LBUTTONUP = 514,
        WM_LBUTTONDBLCLK = 515,
        WM_RBUTTONDOWN = 516,
        WM_RBUTTONUP = 517,
        WM_RBUTTONDBLCLK = 518,
        WM_MBUTTONDOWN = 519,
        WM_MBUTTONUP = 520,
        WM_MBUTTONDBLCLK = 521,
        WM_MOUSEWHEEL = 522,
        WM_MOUSELAST = 525,

        WM_PARENTNOTIFY = 528,
        WM_ENTERMENULOOP = 529,
        WM_EXITMENULOOP = 530,

        WM_NEXTMENU = 531,
        WM_SIZING = 532,
        WM_CAPTURECHANGED = 533,
        WM_MOVING = 534,

        WM_ENTERSIZEMOVE = 561,
        WM_EXITSIZEMOVE = 562,

        WM_MOUSELEAVE = 675,
        WM_MOUSEHOVER = 673,
        WM_NCMOUSEHOVER = 672,
        WM_NCMOUSELEAVE = 674,

        WM_MDIACTIVATE = 546,
        WM_HSCROLL = 276,
        WM_VSCROLL = 277,

        WM_console_print = 791,
        WM_console_printCLIENT = 792,

        SB_BOTTOM = 7

    }

}
