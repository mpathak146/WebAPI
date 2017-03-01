namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IPortalVerificationRepository
    {
        bool IsValidEmployee(string groupID,string employee);
        bool RollbackEmployee(string groupID, string employee);
    }
}