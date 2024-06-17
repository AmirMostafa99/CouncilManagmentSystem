namespace CouncilsManagmentSystem.Contants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.AddCouncil",
                $"Permissions.{module}.EditCouncil",
                $"Permissions.{module}.CreateCouncil",
                $"Permissions.{module}.DeleteCouncil",
                $"Permissions.{module}.EndCouncil",
                $"Permissions.{module}.AddMember",
                $"Permissions.{module}.AddUser",
                $"Permissions.{module}.DeactivateUser",
                $"Permissions.{module}.AddTopic",
                $"Permissions.{module}.ArrangeTopic",
                $"Permissions.{module}.AddResult",

            };
        }
    }
}
