using System;
using System.Collections.Generic;

namespace Hackaton.Models.DTO
{
    public class EventCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int BasePoints { get; set; }
        public double Difficulty { get; set; }
        public string Category { get; set; }
        public List<string> PrizeNames { get; set; }
    }
}