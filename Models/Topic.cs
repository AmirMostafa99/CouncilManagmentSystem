using CouncilsManagmentSystem.Attributes;
using CouncilsManagmentSystem.Settings;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        [DefaultValue(false)]
        public bool ?IsDiscussed { get; set; }
        public string? Notes { get; set; }
        [DefaultValue("Not yet")]
        public string? Result { get; set; } 
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("CouncilId")]
        public int CouncilId { get; set; }
        public Councils Council { get; set; }
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        public string? Attachment { get; set; } 
    }
}
