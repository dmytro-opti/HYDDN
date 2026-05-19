using System.Diagnostics.Contracts;

namespace TravellerAI.Domain.Models;
//Travel with who and recommendation Connection
public class UserInfoModel
{
   // public TravelWithWhoModel TravelWithWho {get; set}
   public Guid Id { get; set; }
    public UserModel User { get; set; }
    public List<string> Interests { get; set; }
    public string TravelStyle { get; set; }
    public List<string> Points { get; set; }
    public string LookingFor { get; set; }
    public List<string> Languages { get; set; }
    public List<string> PersonalityType { get; set; } // extrovert/introv/ambi
    public int Age { get; set; }
    public List<string> Genders { get; set; }
    public string Destanation { get; set; }
    public List<string> Point { get; set; }
    public DateTime JourneyDate { get; set; }
    public List <string> ChoosenActivity {get; set;}
    public List <string> ChoosenTrip {get; set;}
    public List <string> MoneyAmount {get; set;} // Кількість грошей на подорож (мало, норм, багато)
}