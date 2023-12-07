using Whistler.Domain.Phone.Messenger;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class MessageDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Time { get; set; }

        public bool IsRead { get; set; }

        public int SenderId { get; set; }

        public Attachment Attachment { get; set; }
    }

    internal class PostDto : MessageDto
    {
        public string Title { get; set; }

        public string ImgUrl { get; set; }
    }
}
