namespace SguScheduleAPNs.NotificationServer.Entities;

public class Lesson
{
    public string Weekday { get; set; }
    public string Cabinet { get; set; }
    public string TeacherFullName { get; set; }
    public string LessonType { get; set; }
    public string TimeStart { get; set; }
    public string Title { get; set; }
    public string Subgroup { get; set; }
    public string TimeEnd { get; set; }
    public string Weektype { get; set; }
    public int LessonNumber { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Lesson other)
            return false;

        return Weekday == other.Weekday &&
               Cabinet == other.Cabinet &&
               TeacherFullName == other.TeacherFullName &&
               LessonType == other.LessonType &&
               TimeStart == other.TimeStart &&
               Title == other.Title &&
               Subgroup == other.Subgroup &&
               TimeEnd == other.TimeEnd &&
               Weektype == other.Weektype &&
               LessonNumber == other.LessonNumber;
    }

    public override int GetHashCode()
    {
        var hash1 = HashCode.Combine(
            Weekday,
            Cabinet,
            TeacherFullName,
            LessonType,
            TimeStart,
            Title,
            Subgroup);

        var hash2 = HashCode.Combine(
            TimeEnd,
            Weektype,
            LessonNumber);

        return HashCode.Combine(hash1, hash2);
    }
}