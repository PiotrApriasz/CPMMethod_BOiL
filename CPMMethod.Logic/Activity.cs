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
        
        public Activity[] Successors { get; set; } 
        public Activity[] Preccessors { get; set; }
    }
}