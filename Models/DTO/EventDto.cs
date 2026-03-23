using System;
using System.Collections.Generic;

namespace Hackaton.Models.DTO
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int BasePoints { get; set; }
        public double Difficulty { get; set; }
        public string Category { get; set; }
        public string OrganizerName { get; set; }
        public List<string> Prizes { get; set; }
        public int ParticipantsCount { get; set; }
    }
}