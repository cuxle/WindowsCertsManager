using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAppAccountManager.Components.Models
{
    public class PdfDocument
    {
        public string Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public byte[] Content { get; set; } = Array.Empty<byte>();

        public long FileSize => Content.Length;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
