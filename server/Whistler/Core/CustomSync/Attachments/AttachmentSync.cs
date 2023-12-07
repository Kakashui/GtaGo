using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.CustomSync.Attachments
{
    public class AttachmentSync : Script
    {
        private const int SerializeBase = 16;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AttachmentSync));

        public static bool AddAttachment(Player player, AttachId attachId)
        {
            if (attachId == AttachId.invalid) return false;
            var attachHash = NAPI.Util.GetHashKey(attachId.ToString());
            return AddAttachment(player, attachHash);
        }

        public static bool AddAttachment(Player player, uint attachHash)
        {
            var attachments = player.GetData<List<uint>>("attachments:data");

            if (attachments.Contains(attachHash))
                return false;

            attachments.Add(attachHash);

            player.SetSharedData("attachmentsData", SerializeAttachments(attachments));
            return true;
        }

        public static bool RemoveAttachment(Player player, AttachId attachId)
        {
            if (attachId == AttachId.invalid) return false;
            var attachHash = NAPI.Util.GetHashKey(attachId.ToString());
            return RemoveAttachment(player, attachHash);
        }

        public static bool RemoveAttachment(Player player, uint attachHash)
        {
            var attachments = player.GetData<List<uint>>("attachments:data");

            if (!attachments.Contains(attachHash))
                return false;

            attachments.Remove(attachHash);

            player.SetSharedData("attachmentsData", SerializeAttachments(attachments));
            return true;
        }

        public static bool HasAttachment(Player player, AttachId attachId)
        {
            var attachments = player.GetData<List<uint>>("attachments:data");
            return attachments.Contains(NAPI.Util.GetHashKey(attachId.ToString()));
        }

        private static string SerializeAttachments(List<uint> attachments)
        {
            if (attachments.Count == 0)
                return "";

            return attachments
                .Select(hash => Convert.ToString(hash, SerializeBase))
                .Aggregate((hash1, hash2) => hash1 + "|" + hash2);
        }

        [ServerEvent(Event.PlayerConnected)]
        public void HandlePlayerConnected(Player player)
        {
            player.SetData("attachments:data", new List<uint>());
        }

        [RemoteEvent("staticAttachments.Add")]
        public void HandleAddAttachmentFromClient(Player player, string attachSerialized)
        {
            try
            {
                var attachmentHash = Convert.ToUInt32(attachSerialized, SerializeBase);
                AddAttachment(player, attachmentHash);
            }
            catch (Exception e)
            {
                _logger.WriteError("Unhandled exception catched on staticAttachments.Add: " + e.ToString());
            }
        }

        [RemoteEvent("staticAttachments.Remove")]
        public void HandleRemoveAttachmentFromClient(Player player, string attachSerialized)
        {
            try
            {
                var attachmentHash = Convert.ToUInt32(attachSerialized, SerializeBase);
                RemoveAttachment(player, attachmentHash);
            }
            catch (Exception e)
            {
                _logger.WriteError("Unhandled exception catched on staticAttachments.Add: " + e.ToString());
            }
        }
    }
}
