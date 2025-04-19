using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Models
{
    public class Attemp
    {
        public int Id { get; set; }
        public int AnsweredBy { get; set; }
        public int QuizzId { get; set; }
        public int? CorrectNumber { get; set; }
        public TimeSpan Time { get; set; }
        public bool Complete { get; set; }
    }
}
