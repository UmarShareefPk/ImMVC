using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace ImBlazorWebAssembly.Helper
{
    public class NewCommentModel
    {
        [Required]
        public string CommentText { get; set; }
        public string IncidentId { get; set; }
        public string UserId { get; set; }

        public IBrowserFile files { get; set; }
    }
}
