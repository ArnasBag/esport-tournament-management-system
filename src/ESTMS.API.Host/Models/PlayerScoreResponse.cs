﻿namespace ESTMS.API.Host.Models
{
    public class PlayerScoreResponse
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}