﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeepCorrect.Translator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        [DllImport("User32.dll")] 
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint VK_T = 0x54;
            const uint MOD_CTRL = 0x0002;
            if(!RegisterHotKey(helper.Handle, HOTKEY_ID, (uint)(Modifiers.Alt | Modifiers.Shift), VK_T))
            {
                // handle error
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch(msg)
            {
                case WM_HOTKEY:
                    switch(wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private async void OnHotKeyPressed()
        {
            await ShowTranslate();
        }

        private async Task ShowTranslate()
        {
            var ptr = GetForegroundWindow();
            Thread.Sleep(200);
            SendCtrlC(ptr);
            Thread.Sleep(100);
            // programatically copy selected text into clipboard
            //await System.Threading.Tasks.Task.Factory.StartNew(fetchSelectionToClipboard);

            // access clipboard which now contains selected text in foreground window (active application)
            var text = await Task.Factory.StartNew(getClipBoardValue);

            //TODO: if (it is not text) return;
            if (ItIsText(text))
            {
                ShowTranslateOfText(text, await GetTranslate(text));
            }
            else
            {
                if (text.Length > 100) return;
                var result = await Search.GetSearchResult(text);
                //ShowTranslates();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            
            //SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
            Activate();
        }
        
        private async Task<string> GetTranslate(string text)
        {
            try
            {
                var @string = await "https://translate.googleapis.com"
                    .AppendPathSegments("translate_a", "single")
                    .SetQueryParam("client", "gtx")
                    .SetQueryParam("sl", "en")
                    .SetQueryParam("tl", "ru")
                    .SetQueryParam("dt", "t")
                    .SetQueryParam("q", text)
                    .GetStringAsync();
                var jsonObj = (JArray)JsonConvert.DeserializeObject(@string);
                return string.Join("", jsonObj[0].Select(t=>t[0]));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        
        private static bool ItIsText(string text)
        {
            return text.Trim().Any(ch => ch == ' ');
        }
        
        private void ShowTranslateOfText(string text, string translate)
        {
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            
            
            var textBox = new TextBox
            {
                Padding = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                Text = text
            };
            textBox.MouseMove += TextBox_MouseMove;
            stackPanel.Children.Add(textBox);
            
            var translateBox = new TextBox
            {
                Padding = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                Text = translate
            };
            translateBox.MouseMove += TextBox_MouseMove;
            stackPanel.Children.Add(translateBox);
            
            myGrid.Children.Add(stackPanel);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Point mousePoint = Mouse.GetPosition(textBox);
            int charPosition = textBox.GetCharacterIndexFromPoint(mousePoint, true);
            if (charPosition > 0)
            {
                textBox.Focus();
                int index = 0;
                int i = 0;
                string[] strings = textBox.Text.Split(' ');
                while (index + strings[i].Length < charPosition && i < strings.Length)
                {
                    index += strings[i++].Length + 1;
                }

                textBox.Select(index, strings[i].Length);
            }
        }

        public static void SendCtrlC(IntPtr hWnd)
        {
            uint KEYEVENTF_KEYUP = 2;
            byte VK_CONTROL = 0x11;
            SetForegroundWindow(hWnd);
            keybd_event(VK_CONTROL,0,0,0);
            keybd_event (0x43, 0, 0, 0 ); //Send the C key (43 is "C")
            keybd_event (0x43, 0, KEYEVENTF_KEYUP, 0);
            keybd_event (VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);// 'Left Control Up

        }
        
        // depends on the type of your app, you sometimes need to access clipboard from a Single Thread Appartment model..therefore I'm creating a new thread here
        static string getClipBoardValue()
        {
            var text = "";
            var staThread = new Thread(
                delegate()
                {
                    try
                    {
                        text = Clipboard.GetText();
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return text;
        }

        [Flags]
        public enum Modifiers
        {
            /// <summary>Specifies that the key should be treated as is, without any modifier.
            /// </summary>
            NoModifier = 0x0000,

            /// <summary>Specifies that the Accelerator key (ALT) is pressed with the key.
            /// </summary>
            Alt = 0x0001,

            /// <summary>Specifies that the Control key is pressed with the key.
            /// </summary>
            Ctrl = 0x0002,

            /// <summary>Specifies that the Shift key is pressed with the associated key.
            /// </summary>
            Shift = 0x0004,

            /// <summary>Specifies that the Window key is pressed with the associated key.
            /// </summary>
            Win = 0x0008
        }
    }
}