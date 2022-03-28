using System.ComponentModel.DataAnnotations;
using CPMMethod.BlazorWasmClient.Validators;

namespace CPMMethod.Logic
{
    public class Activity
    {
        [Display(Name = "Id Aktywności")]
        [Required(ErrorMessage = "Id Aktywności jest wymagane")]
        [CustomValidationActivityId]
        public string Id { get; set; } = String.Empty;
        
        [Display(Name = "Opis")]
        public string Description { get; set; } = String.Empty;
        
        [Display(Name = "Czas Trwania")]
        public uint Duration { get; set; }
        
        [Display(Name = "Aktywność Poprzedzająca")]
        [Required(ErrorMessage = "Aktywność poprzedzająca jest wymagana")]
        public string PreActivities { get; set; }
        
        [Display(Name = "Wczesny Start")]
        public double EarlyStart { get; set; }
        [Display(Name = "Późny Start")]
        public double LateStart { get; set; }
        [Display(Name = "Wczesne Zakończenie")]
        public double EarlyFinish { get; set; }
        [Display(Name = "Późne Zakończnie")]
        public double LateFinish { get; set; }

        [Display(Name = "Rezerwa")]
        public double Reserve { get; set; }

        [Display(Name = "Czynność krytyczna")]
        public bool Critical { get; set; }

        public List<Activity> Successors { get; set; } 
        public List<Activity> Preccessors { get; set; }
    }
}