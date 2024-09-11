namespace WorkTask.Models
{
    // Defines sorting options for tasks
    public class SortingOptions
    {
        // Enum to specify the field by which tasks should be sorted
        public enum SortByOptions
        {
            DueDate,
            Priority
        }

        // Enum to specify the order of sorting
        public enum SortOrder
        {
            Ascending,
            Descending
        }
    }
}
