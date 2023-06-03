using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModel
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Login { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        [MinLength(4)]
        public string Password { get; set; } 

        public int Games { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Defeats { get; set; }
    }
}
