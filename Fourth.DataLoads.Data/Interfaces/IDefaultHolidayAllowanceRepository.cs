namespace Fourth.DataLoads.Data.Repositories
{
    using Interfaces;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// An example repository that imports and exports the same model.
    /// </summary>
    /// <remarks>
    /// Note that repositories should use asynchronous methods so we can get the scaling benefit of
    /// asynchronous execution when making calls to the database.
    /// </remarks>
    public interface IDefaultHolidayAllowanceRepository:IRepository<DefaultHolidayAllowance>
    {
        /// <summary>
        /// Fetches default holiday allowance data for the supplied job group id.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <returns>A list of default holiday allowances.</returns>
        Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID);

        /// <summary>
        /// Fetches default holiday allowance data for the supplied job title id.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <param name="jobtitleID">The identifier of the job title ID.</param>
        /// <returns>A list of default holiday allowances.</returns>
        Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID, int jobtitleID);

        /// <summary>
        /// Fetches default holiday allowance data for the supplied job title id and years worked.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <param name="jobtitleID">The identifier of the job title ID.</param>
        /// <param name="yearsworked">The identifier of the years worked value.</param>
        /// <returns>A list of default holiday allowances.</returns>
        Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID, int jobtitleID, int yearsworked);

        /// <summary>
        /// Updates a submitted list of default holiday allowances.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <param name="input">The list of default holiday allawances to update.</param>
        /// <returns>An indication of whether any records were updated.</returns>
        Task<bool> SetDataAsync(int groupID, List<DefaultHolidayAllowance> input);

        /// <summary>
        /// Deletes a default holiday allowances based on supplied groupID and jobtitleID.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <param name="jobtitleID">The identifier of the job title ID.</param>
        /// <returns>An indication of whether any records were deleted.</returns>
        Task<bool> DeleteDataAsync(int groupID, int jobtitleID);
    }
}