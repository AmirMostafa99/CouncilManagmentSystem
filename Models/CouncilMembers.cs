﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class CouncilMembers
    {
        [Key, Column(Order = 0)]
        public string MemberId { get; set; }

        [Key, Column(Order = 1)]
        public int CouncilId { get; set; }

        public string? Pdf { get; set; }
        public bool IsAttending { get; set; }
        public string? ReasonNonAttendance { get; set; }

        [ForeignKey("CouncilId")]
        public Councils Council { get; set; }

        [ForeignKey("MemberId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}