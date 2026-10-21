using Panuon.WPF.UI;
using MessageBoxIcon = Panuon.WPF.UI.MessageBoxIcon;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace Oddob.Service.Model.Models;

public record AccountMessage(AccountPageType AccountPageType);

public record AccountCloseMessage;

public record VerificationMessage;

public sealed class ToastMessage
{
    public static void Send(string message, MessageBoxIcon icon = MessageBoxIcon.None)
    {
        var toastMessage = new ToastMessage(message) { Icon = icon };
        WeakReferenceMessenger.Default.Send(toastMessage);
    }

    public ToastMessage(string message)
    {
        Message = message;
    }

    public string Message { get; }
    public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.None;
    public ToastPosition ToastPosition { get; set; } = ToastPosition.Right;

}