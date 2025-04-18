using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoiceApplication.Models
{
    public class Quizz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRandom { get; set; }
        public int AttempNumber { get; set; }
        public int CreatedBy { get; set; }
    }
}
