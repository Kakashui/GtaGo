using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using static Whistler.Core.Weapons;

namespace Whistler.Core.CustomSync.Attachments
{
    public class AttachmentSync : Script
    {
        private const int SerializeBase = 16;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AttachmentSync));

        public static bool AddAttachment(ExtPlayer player, AttachId attachId)
        {
            if (attachId == AttachId.invalid) return false;

            uint hash = NAPI.Util.GetHashKey(attachId.ToString());
            return AddAttachment(player, hash);
        }

        public static bool AddAttachment(ExtPlayer player, uint attachHash)
        {
            if (player.Session.Attachments.Contains(attachHash)) return false;

            player.Session.Attachments.Add(attachHash);
            SafeTrigger.SetSharedData(player, "attachmentsData", JsonConvert.SerializeObject(player.Session.Attachments));
            return true;
        }

        public static bool RemoveAttachment(ExtPlayer player, AttachId attachId)
        {
            if (attachId == AttachId.invalid) return false;

            uint hash = NAPI.Util.GetHashKey(attachId.ToString());
            return RemoveAttachment(player, hash);
        }

        public static bool RemoveAttachment(ExtPlayer player, uint attachHash)
        {
            if (!player.Session.Attachments.Contains(attachHash)) return false;
            
            player.Session.Attachments.Remove(attachHash);
            SafeTrigger.SetSharedData(player, "attachmentsData", JsonConvert.SerializeObject(player.Session.Attachments));
            return true;
        }

        public static bool HasAttachment(ExtPlayer player, AttachId attachId)
        {
            return player.Session.Attachments.Contains(NAPI.Util.GetHashKey(attachId.ToString()));
        }

        [RemoteEvent("staticAttachments.Add")]
        public void HandleAddAttachmentFromClient(ExtPlayer player, string hash)
        {
            try
            {
                AddAttachment(player, (uint)Convert.ToInt64(hash));
            }
            catch (Exception e)
            {
                _logger.WriteError("Unhandled exception catched on staticAttachments.Add: " + e.ToString());
            }
        }

        [RemoteEvent("staticAttachments.Remove")]
        public void HandleRemoveAttachmentFromClient(ExtPlayer player, string hash)
        {
            try
            {
                RemoveAttachment(player, (uint)Convert.ToInt64(hash));
            }
            catch (Exception e)
            {
                _logger.WriteError("Unhandled exception catched on staticAttachments.Add: " + e.ToString());
            }
        }
    }
}
