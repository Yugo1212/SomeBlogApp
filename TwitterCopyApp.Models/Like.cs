using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCopyApp.Models
{
    public class Like
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public int? PostId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }

        public int? CommentId { get; set; }

        public bool IsLiked { get; set; }
    }
}
