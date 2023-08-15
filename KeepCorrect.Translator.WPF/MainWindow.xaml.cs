using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Flurl;
using Flurl.Http;
using KeepCorrect.Translator.WPF.AppSettings;
using KeepCorrect.Translator.WPF.Enums;
using KeepCorrect.Translator.WPF.Extensions;
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
        

        public bool ShowSourceTextIsChecked
        {
            get => AppSettingsManager.ShowSourceText;
            set => AppSettingsManager.Set(AppSettingKeyEnum.ShowSourceText, value);
        }
        
        [DllImport("User32.dll")] 
        private static extern bool SetForegroundWindow(nint hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
            [In] nint hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] nint hWnd,
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

        private nint HwndHook(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
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
            return nint.Zero;
        }

        private async void OnHotKeyPressed()
        {
            MyStackPanel.Children.Clear();
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
            var text = await Task.Factory.StartNew(GetClipBoardValue);

            if (text.IsText())
            {
                ShowTranslateOfText(text, await GetTranslate(text));
            }
            else
            {
                if (text.Length > 100) return;
                var result = await Search.GetSearchResult(text);
                ShowTranslates(result);
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            
            //SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
            Activate();
        }

        private void ShowTranslates(SearchResult searchResult)
        {
            var count = 0;
            var height = 250;
            
            var partsOfSpeech = searchResult.Word.PartsOfSpeech.GetType()
                .GetProperties()
                .Where(p => p.GetValue(searchResult.Word.PartsOfSpeech, null) != null)
                .Select(p => (Adjective)p.GetValue(searchResult.Word.PartsOfSpeech, null)) ?? new List<Adjective>();
            foreach (var partOfSpeech in partsOfSpeech)
            {
                var translateBox = new TextBox
                {
                    Padding = new Thickness(10, 10, 10, 10),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Normal,
                    Text = partOfSpeech.Word
                };
                translateBox.PreviewMouseLeftButtonUp += async (s,e) => await TranslateBoxOnMouseLeftButtonUp(s, e);
                MyStackPanel.Children.Add(translateBox);
                
                var translateRuBox = new TextBox
                {
                    Padding = new Thickness(10, 10, 10, 10),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    FontSize = 15,
                    FontWeight = FontWeights.Normal,
                    FontStyle = FontStyles.Italic,
                    Text = partOfSpeech.PartOfSpeechRu
                };
                MyStackPanel.Children.Add(translateRuBox);
                
                var translationsTextBox = GetTranslateTextBox(partOfSpeech.Values.Select(v => v.ValueValue));
                MyStackPanel.Children.Add(translationsTextBox);
                
            }
        }
        
        private Control GetTranslateTextBox(IEnumerable<string> translates)
        {
            var translateBox = new TextBox
            {
                Padding = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                FontSize = 15,
                FontWeight = FontWeights.Normal,
                FontStyle = FontStyles.Normal
            };
            foreach (var translate in translates)
            {
                translateBox.AppendText($"– {translate}");
                translateBox.AppendText(Environment.NewLine);
            }

            return translateBox;
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
        
        private void ShowTranslateOfText(string text, string translate)
        {
            var translateBox = new TextBox
            {
                Padding = new Thickness(10, 10, 10, 10),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                Text = translate
            };
            
            MyStackPanel.Children.Add(translateBox);
            
            if (AppSettingsManager.ShowSourceText)
            {
                var textBox = new TextBox
                {
                    Padding = new Thickness(10, 10, 10, 10),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    FontSize = 15,
                    FontWeight = FontWeights.Normal,
                    FontStyle = FontStyles.Normal,
                    Text = text
                };
                textBox.PreviewMouseLeftButtonUp += async (s,e) => await TranslateBoxOnMouseLeftButtonUp(s, e);
                MyStackPanel.Children.Add(textBox);
            }
        }

        private async Task TranslateBoxOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && textBox.SelectedText.IsTextOrWord())
            {
                await TryShowBubble(textBox.SelectedText.GetText(), e);
            }
        }

        private async Task TryShowBubble(string text, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (text.IsWord())
            {
                var searchResult = await Search.GetSearchResult(text);
                ShowBubble(searchResult.Html, mouseButtonEventArgs);
            }
        }

        private void ShowBubble(string html, MouseButtonEventArgs mouseButtonEventArgs)
        {
            throw new NotImplementedException();
        }

        private static void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBox;
            var mousePoint = Mouse.GetPosition(textBox);
            var charPosition = textBox.GetCharacterIndexFromPoint(mousePoint, true);
            if (charPosition > 0)
            {
                textBox.Focus();
                var index = 0;
                var i = 0;
                var strings = textBox.Text.Split(' ');
                while (index + strings[i].Length < charPosition && i < strings.Length)
                {
                    index += strings[i++].Length + 1;
                }

                textBox.Select(index, strings[i].Where(char.IsLetter).Count());
            }
        }

        private static void SendCtrlC(nint hWnd)
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
        private static string GetClipBoardValue()
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

        public void OnAutoStart()
        {
            if (WindowState == WindowState.Minimized)
            {
                //Must have this line to prevent the window start locatioon not being in center.
                WindowState = WindowState.Normal;
                Hide();
                //Show your tray icon code below
            }
            else
            {
                Show();
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}