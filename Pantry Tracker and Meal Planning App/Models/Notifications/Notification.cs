namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Notifications
{
    public class Notification
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string RelatedItemId { get; set; }
        public bool IsRead => ReadAt.HasValue;

    }
}
