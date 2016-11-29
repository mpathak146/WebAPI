namespace Fourth.DataLoads.Data.Interfaces
{
    public interface ITableSchema
    {
        string CHARACTER_MAXIMUM_LENGTH { get; set; }
        string COLUMN_NAME { get; set; }
        string DATA_TYPE { get; set; }
        string TABLE_NAME { get; set; }
    }
}