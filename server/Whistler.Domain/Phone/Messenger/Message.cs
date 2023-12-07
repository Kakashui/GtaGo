using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("phones_msg_messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int SenderId { get; set; }
        public Account Sender { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public bool IsRead { get; set; } = false;

        public IReadOnlyList<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    public abstract class Attachment
    {
        public AttachmentType Type { get; set; }

        public Attachment(AttachmentType type)
        {
            Type = type;
        }
    }

    public class GeoPosition : Attachment
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public GeoPosition(float x, float y, float z) : base (AttachmentType.GeoPosition)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Sound : Attachment
    {
        public string Url { get; }

        public Sound(string url) : base(AttachmentType.Sound)
        {
            Url = url;
        }
    }

    public class Photo : Attachment
    {
        public string Url { get; }

        public Photo(string url) : base(AttachmentType.Photo)
        {
            Url = url;
        }
    }

    public enum AttachmentType
    {
        GeoPosition = 0,
        Sound = 1,
        Photo = 2
    }
}
