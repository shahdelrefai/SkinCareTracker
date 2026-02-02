using System.Collections.ObjectModel;

namespace SkinCareTracker.Models
{
    public class RoutineGroup : ObservableCollection<Routine>
    {
        public string Key { get; set; }

        public RoutineGroup(string key, ObservableCollection<Routine> items) : base(items)
        {
            Key = key;
        }
    }
}
