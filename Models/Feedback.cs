using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo_Food_Main.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; } = string.Empty;

        [ForeignKey("Order")]
        public int? OrderID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

   

        // Navigation properties
        public virtual Users? User { get; set; }
        public virtual Order? Order { get; set; }
        
        // Link feedback to a menu item (optional)
        public int? MenuItemId { get; set; }
        public virtual MenuItem? MenuItem { get; set; }
    }
}