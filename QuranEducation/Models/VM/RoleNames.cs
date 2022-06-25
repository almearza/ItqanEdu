

namespace QuranEducation.Models.VM
{
    public static class RoleNames
    {
        public const string AdminLevel = "AdminLevel";
        public const string InstructorLevel = "InstructorLevel";
        public const string StudentLevel = "StudentLevel";
        public const string AllLevels = AdminLevel + "," + "," + InstructorLevel+","+ StudentLevel;
    }
}