using System;
using Microsoft.AspNetCore.Components;

public class ToastService
{
    public event Action<string, string, ToastType> OnShow;
    
    public void ShowInfo(string message, string title = "Información")
    {
        OnShow?.Invoke(title, message, ToastType.Info);
    }
    
    public void ShowSuccess(string message, string title = "Éxito")
    {
        OnShow?.Invoke(title, message, ToastType.Success);
    }
    
    public void ShowWarning(string message, string title = "Advertencia")
    {
        OnShow?.Invoke(title, message, ToastType.Warning);
    }
    
    public void ShowError(string message, string title = "Error")
    {
        OnShow?.Invoke(title, message, ToastType.Error);
    }
}

public enum ToastType
{
    Info,
    Success,
    Warning,
    Error
}