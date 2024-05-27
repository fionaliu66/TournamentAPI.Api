using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate
        {
            get { return StartDate.AddMonths(3); }
        }
    }
}
