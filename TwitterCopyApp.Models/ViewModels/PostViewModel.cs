using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCopyApp.Models.ViewModels
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts;
        public IEnumerable<Like> Likes { get; set; }
    }
}
