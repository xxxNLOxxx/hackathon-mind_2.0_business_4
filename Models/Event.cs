using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hackaton.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int BasePoints { get; set; }          // базовые баллы

        public double Difficulty { get; set; }        // коэффициент сложности (1.0 – 3.0)

        [MaxLength(50)]
        public string Category { get; set; }          // направление (IT, Социальное проектирование, Медиа)

        public int OrganizerId { get; set; }          // внешний ключ на User

        [ForeignKey(nameof(OrganizerId))]
        public UserTable Organizer { get; set; }           // навигационное свойство

        public ICollection<Prize> Prizes { get; set; } = new List<Prize>();
        public ICollection<Participation> Participations { get; set; } = new List<Participation>();
    }
}