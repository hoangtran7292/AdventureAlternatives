using System;
using static AdventureAlternatives.Models.CommonZohoModules;

namespace AdventureAlternatives.Models
{
    public class CampResponse
    {
        public CampDetails[] data { get; set; }
    }

    public class CampDetails
    {
        public string Camp_Web_Page { get; set; }
        public Owner Owner { get; set; }
        public string Camp_Start_Date { get; set; }
        public string Camp_Due_Date { get; set; }
        // public object Camp_Dates { get; set; }
        public string Camp_Teacher_Web_Page { get; set; }
        public string Name { get; set; }
        public string Camp_Location { get; set; }
        public DateTime? Last_Activity_Time { get; set; }
        public Modified_By Modified_By { get; set; }
        public object Unsubscribed_Mode { get; set; }
        public string Stage { get; set; }
        public string Year_Level { get; set; }
        public string id { get; set; }
        public int? Number_of_Participants { get; set; }
        public DateTime? Modified_Time { get; set; }
        public Camp_Styles[] Camp_Styles { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Unsubscribed_Time { get; set; }
        public object Deal { get; set; }
        public int? Number_of_Groups { get; set; }
        public float? Camp_Cost { get; set; }
        public Related_Camp_Activities[] Related_Camp_Activities { get; set; }
        public object[] Tag { get; set; }
        public Created_By Created_By { get; set; }
        public string Camp_Medical_Dietary_Reponses_Link { get; set; }
    }



    public class Camp_Styles
    {
        public bool? in_merge { get; set; }
        public object field_states { get; set; }
        public DateTime? Created_Time { get; set; }
        public string Camp_Style { get; set; }
        public Parent_Id Parent_Id { get; set; }
        public int? Nights { get; set; }
        public string id { get; set; }
        public string Day_or_Night { get; set; }
    }

    public class Related_Camp_Activities
    {
        public float? Cost_per_Student { get; set; }
        public DateTime Created_Time { get; set; }
        public Activity Activity { get; set; }
        public string id { get; set; }
        public string Date { get; set; }
    }

    public class Activity
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
