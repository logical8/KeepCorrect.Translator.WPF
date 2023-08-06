// using System;
// using System.Collections.Generic;
// using System.Runtime.InteropServices;
// using System.Windows.Input;
// using System.Windows.Interop;
//
// namespace KeepCorrect.Translator
// {
//     public class GlobalHotKey : IDisposable
//     {
//         /// <summary>
//         /// Registers a global hotkey
//         /// </summary>
//         /// <param name="aKey">e.g. S</param>
//         /// <param name="aAction">Action to be called when hotkey is pressed</param>
//         /// <param name="aModifier">e.g. Alt | Shift</param>
//         /// <returns>true, if registration succeeded, otherwise false</returns>
//         public static bool RegisterHotKey(Modifiers aModifier, Key aKey, Action aAction)
//         {
//             if (aModifier == Modifiers.NoModifier)
//             {
//                 throw new ArgumentException("Modifier must not be ModifierKeys.None");
//             }
//
//             if (aAction is null)
//             {
//                 throw new ArgumentNullException(nameof(aAction));
//             }
//
//             _currentId += 1;
//
//             var aRegistered = RegisterHotKey(Window.Handle,
//                 _currentId,
//                 (uint)aModifier | ModNorepeat,
//                 (uint)aKey);
//
//             if (aRegistered)
//             {
//                 RegisteredHotKeys.Add(new HotKeyWithAction(aModifier, aKey, aAction));
//             }
//
//             return aRegistered;
//         }
//
//         public void Dispose()
//         {
//             // unregister all the registered hot keys.
//             for (var i = _currentId; i > 0; i--)
//             {
//                 UnregisterHotKey(Window.Handle, i);
//             }
//
//             // dispose the inner native window.
//             Window.Dispose();
//         }
//
//         static GlobalHotKey()
//         {
//             Window.KeyPressed += (s, e) =>
//             {
//                 RegisteredHotKeys.ForEach(x =>
//                 {
//                     if (e.Modifier == x.Modifier && e.Key == x.Key)
//                     {
//                         x.Action();
//                     }
//                 });
//             };
//         }
//
//         private static readonly InvisibleWindowForMessages Window = new InvisibleWindowForMessages();
//         private static int _currentId;
//         private const uint ModNorepeat = 0x4000;
//         private static readonly List<HotKeyWithAction> RegisteredHotKeys = new List<HotKeyWithAction>();
//
//         private class HotKeyWithAction
//         {
//             public HotKeyWithAction(Modifiers modifier, Key key, Action action)
//             {
//                 Modifier = modifier;
//                 Key = key;
//                 Action = action;
//             }
//
//             public Modifiers Modifier { get; }
//             public Key Key { get; }
//             public Action Action { get; }
//         }
//
//         // Registers a hot key with Windows.
//         [DllImport("user32.dll")]
//         private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
//
//         // Unregisters the hot key with Windows.
//         [DllImport("user32.dll")]
//         private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
//         
//         [DllImport("User32.dll")] 
//         private static extern bool SetForegroundWindow(IntPtr hWnd);
//
//         [DllImport("user32.dll", CharSet=CharSet.Auto)]
//         public static extern IntPtr GetForegroundWindow();
//
//         [DllImport("user32.dll")]
//         static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
//
//         public static void SendCtrlC(IntPtr hWnd)
//         {
//             uint KEYEVENTF_KEYUP = 2;
//             byte VK_CONTROL = 0x11;
//             SetForegroundWindow(hWnd);
//             keybd_event(VK_CONTROL,0,0,0);
//             keybd_event (0x43, 0, 0, 0 ); //Send the C key (43 is "C")
//             keybd_event (0x43, 0, KEYEVENTF_KEYUP, 0);
//             keybd_event (VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);// 'Left Control Up
//
//         }
//
//         private sealed class InvisibleWindowForMessages : NativeWindow, IDisposable
//         {
//             public InvisibleWindowForMessages()
//             {
//                 CreateHandle(new CreateParams());
//             }
//
//             private const int WmHotkey = 0x0312;
//             
//             
//
//             protected override void WndProc(ref Message m)
//             {
//                 base.WndProc(ref m);
//                 if (m.Msg != WmHotkey) return;
//                 var keys = (Keys)((int)m.LParam >> 0x10) & Keys.KeyCode;
//                 var modifier = (Modifiers)((int)m.LParam & 0xffff);
//                 KeyPressed?.Invoke(this, new HotKeyPressedEventArgs(modifier, keys));
//             }
//
//             public class HotKeyPressedEventArgs : EventArgs
//             {
//                 internal HotKeyPressedEventArgs(Modifiers modifier, Keys key)
//                 {
//                     Modifier = modifier;
//                     Key = key;
//                 }
//
//                 public Modifiers Modifier { get; }
//                 public Keys Key { get; }
//             }
//
//
//             public event EventHandler<HotKeyPressedEventArgs> KeyPressed;
//
//             #region IDisposable Members
//
//             public void Dispose()
//             {
//                 DestroyHandle();
//             }
//
//             #endregion
//         }
//     }
//
//     [Flags]
//     public enum Modifiers
//     {
//         /// <summary>Specifies that the key should be treated as is, without any modifier.
//         /// </summary>
//         NoModifier = 0x0000,
//
//         /// <summary>Specifies that the Accelerator key (ALT) is pressed with the key.
//         /// </summary>
//         Alt = 0x0001,
//
//         /// <summary>Specifies that the Control key is pressed with the key.
//         /// </summary>
//         Ctrl = 0x0002,
//
//         /// <summary>Specifies that the Shift key is pressed with the associated key.
//         /// </summary>
//         Shift = 0x0004,
//
//         /// <summary>Specifies that the Window key is pressed with the associated key.
//         /// </summary>
//         Win = 0x0008
//     }
// }