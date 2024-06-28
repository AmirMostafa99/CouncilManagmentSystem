namespace CouncilsManagmentSystem.DTOs
{
    public class AddCollageDTO
    {
        
        // [RegularExpression(@"^[\u0600-\u06FF]+$", ErrorMessage = "رجاء ادخال حروف عربيه")]
        public string collage_name { get; set; }
    }
}
