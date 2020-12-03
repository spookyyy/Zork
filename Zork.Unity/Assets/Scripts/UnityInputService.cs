﻿using System;
using UnityEngine;
using Zork;
using TMPro;

public class UnityInputService : MonoBehaviour, IInputService
{
    public TMP_InputField InputField;
#pragma warning disable CS0067
    public event EventHandler<string> InputReceived;
#pragma warning restore CS0067

    public void ProcessInput()
    {
        Assert.IsNotNull(InputField);
        Assert.IsFalse(string.IsNullOrEmpty(InputField.text));

        InputReceived?.Invoke(this, InputField.text);

        InputField.text = string.Empty;
    }
}
