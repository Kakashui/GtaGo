using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.SDK
{
    public enum NotifyType
    {
        Alert,
        Error,
        Success,
        Info,
        Warning
    }
    public enum NotifyPosition
    {
        Top,
        TopLeft,
        TopCenter,
        TopRight,
        Center,
        CenterLeft,
        CenterRight,
        Bottom,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
    public static class Notify
    {
        public static void Send(ExtPlayer client, NotifyType type, NotifyPosition pos, string msg, int time)
        {
            SafeTrigger.ClientEvent(client, "notify", type, pos, msg, time);
        }

        public static void SendInfo(this ExtPlayer player, string msg, int ms = 5000)
        {
            SafeTrigger.ClientEvent(player, "notify", NotifyType.Info, NotifyPosition.Top, msg, ms);
        }
        
        public static void SendError(this ExtPlayer player, string msg, int ms = 5000)
        {
            SafeTrigger.ClientEvent(player, "notify", NotifyType.Error, NotifyPosition.Top, msg, ms);
        }
        
        public static void SendAlert(this ExtPlayer player, string msg, int ms = 5000)
        {
            SafeTrigger.ClientEvent(player, "notify", NotifyType.Alert, NotifyPosition.Top, msg, ms);
        }
        
        public static void SendSuccess(this ExtPlayer player, string msg, int ms = 5000)
        {
            SafeTrigger.ClientEvent(player, "notify", NotifyType.Success, NotifyPosition.Top, msg, ms);
        }

        public static void SendAuthNotify(this ExtPlayer player, int status, string head, string msg)
        {
            SafeTrigger.ClientEvent(player, "authNotify", status, head, msg);
        }

        /// <summary>
        /// Показать большое уведомление с тегом успешно
        /// </summary>
        /// <param name="client">Игрок </param>
        /// <param name="message">Текст сообщения (ключ локализации)</param>
        public static void Alert(ExtPlayer client, string message)
        {
            SafeTrigger.ClientEvent(client, "alert", message);
        }
    }
}
