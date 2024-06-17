namespace CouncilsManagmentSystem.DTOs
{
    public class AddDepartmntDto
    {
        // [RegularExpression(@"^[\u0600-\u06FF]+$", ErrorMessage = "رجاء ادخال حروف عربيه")]
        public string name { get; set; }

        //[RegularExpression(@"^[\u0600-\u06FF]+$", ErrorMessage = "رجاء ادخال حروف عربيه")]
        public int collage_id { get; set; }
    }
}
