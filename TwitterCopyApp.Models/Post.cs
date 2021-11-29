using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCopyApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; }

        public int Likes { get; set; }

        public DateTime CreationDate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
