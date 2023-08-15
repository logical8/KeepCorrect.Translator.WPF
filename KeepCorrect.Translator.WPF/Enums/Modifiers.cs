using System;

namespace KeepCorrect.Translator.WPF.Enums;

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