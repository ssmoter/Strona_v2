using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strona_v2.Shared.File
{
    internal class CommentModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string? Comment { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public DateTimeOffset Created { get; set; }



    }
}
