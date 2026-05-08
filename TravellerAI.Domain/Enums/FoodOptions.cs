namespace TravellerAI.Domain.Enums;

public enum FoodOptions
{
    None = 0,   //немає харчування
    BreakfastOnly = 1,   //тільки сніданок
    HalfBoard = 2,   //сніданок + вечеря
    FullBoard = 3,    //3-х разове харчування
    AllInclusive = 4,   //все включено
    SelfCatering = 5    //кухня (готуєш сам)
}