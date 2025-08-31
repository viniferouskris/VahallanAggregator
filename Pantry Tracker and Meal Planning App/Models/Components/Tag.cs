using Microsoft.EntityFrameworkCore;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TagType Type { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; }
}

public enum TagType
{
    Cuisine,      // e.g., Mediterranean, Italian, Asian
    DietaryType,  // e.g., Vegetarian, Vegan, Gluten-Free
    Meal,         // e.g., Breakfast, Lunch, Dinner
    Course,       // e.g., Appetizer, Main, Dessert
    Technique,    // e.g., Grilled, Baked, Raw
    Season,       // e.g., Summer, Winter
    Occasion      // e.g., Holiday, Party
}

