﻿namespace ESTMS.API.Host.Models
{
    public class DailyPlayerScoreResponse
    {
        public int TotalKills { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
        public DateTime Date { get; set; }
    }
}
