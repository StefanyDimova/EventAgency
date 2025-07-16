using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Web.ViewModels.Event
{
    public class DeleteEventViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
