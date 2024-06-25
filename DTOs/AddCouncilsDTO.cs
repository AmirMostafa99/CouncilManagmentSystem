using CouncilsManagmentSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.DTOs
{
    public class AddCouncilsDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
       
 
    }
}
