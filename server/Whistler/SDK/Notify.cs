using GTANetworkAPI;

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
        public static void Send(Player client, NotifyType type, NotifyPosition pos, string msg, int time)
        {
            Trigger.ClientEvent(client, "notify", type, pos, msg, time);
        }

        public static void SendInfo(this Player player, string msg)
        {
            Trigger.ClientEvent(player, "notify", NotifyType.Info, NotifyPosition.Top, msg, 3000);
        }
        
        public static void SendError(this Player player, string msg)
        {
            Trigger.ClientEvent(player, "notify", NotifyType.Error, NotifyPosition.Top, msg, 3000);
        }
        
        public static void SendAlert(this Player player, string msg)
        {
            Trigger.ClientEvent(player, "notify", NotifyType.Alert, NotifyPosition.Top, msg, 3000);
        }
        
        public static void SendSuccess(this Player player, string msg)
        {
            Trigger.ClientEvent(player, "notify", NotifyType.Success, NotifyPosition.Top, msg, 3000);
        }

        public static void SendAuthNotify(this Player player, int status, string head, string msg)
        {
            Trigger.ClientEvent(player, "authNotify", status, head, msg);
        }

        /// <summary>
        /// Показать большое уведомление с тегом успешно
        /// </summary>
        /// <param name="client">Игрок </param>
        /// <param name="message">Текст сообщения (ключ локализации)</param>
        public static void Alert(Player client, string message)
        {
            Trigger.ClientEvent(client, "alert", message);
        }
    }
}
