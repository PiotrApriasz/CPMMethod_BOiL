using System.ComponentModel.DataAnnotations;
using Boil.Cpm.Logic.Validators;

namespace Boil.Cpm.Logic
{
    public class Activity
    {
        [Display(Name = "Id Aktywności")]
        [Required(ErrorMessage = "Id Aktywności jest wymagane")]
        [CustomValidationActivityId]
        public int Id { get; set; }

        [Display(Name = "Opis")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Czas Trwania")]
        public int Duration { get; set; }

        [Display(Name = "Aktywność Poprzedzająca")]
        [CustomValidationPredecessors]
        public string PreActivities { get; set; } = String.Empty;

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

        public List<Activity> Successors { get; set; } = new List<Activity>();
        public List<Activity> Preccessors { get; set; } = new List<Activity>();
    }
}