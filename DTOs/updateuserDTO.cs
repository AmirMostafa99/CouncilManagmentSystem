﻿namespace CouncilsManagmentSystem.DTOs
{
    public class updateuserDTO
    {
        public string? academic_degree { get; set; }
        public string? phone { get; set; }
        public string? functional_characteristic { get; set; }
        public string? administrative_degree { get; set; }
        public IFormFile img { get; set; }
        public DateTime? Birthday { get; set; }

    }
}
