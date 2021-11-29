using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCopyApp.Models.ViewModels
{
    public class UserPostViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Post> UserPosts { get; set; }

        public IEnumerable<Like> Likes { get; set; }
    }
}
