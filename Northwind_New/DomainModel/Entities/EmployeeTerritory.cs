namespace DomainModel.Entities
{
    public class EmployeeTerritory
    {
        public int EmployeeId { get; private set; }
        public string TerritoryId { get; private set; }

        public Employee Employee { get; private set; }
        public Territory Territory { get; private set; }
    }
}
