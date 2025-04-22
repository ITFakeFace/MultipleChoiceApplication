using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Models
{
    public class AttemptInfo
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public int CorrectNumber { get; set; }
        public TimeSpan Time { get; set; }
        public bool Complete { get; set; }
    }

}
