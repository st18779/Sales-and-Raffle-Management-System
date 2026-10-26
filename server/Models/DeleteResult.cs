namespace server.Models
{
    // תוצאת מחיקה: נמחק / לא נמצא / בשימוש (יש נתונים שתלויים בו)
    public enum DeleteResult { Deleted, NotFound, InUse }
}
