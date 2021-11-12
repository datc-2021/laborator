namespace L04;

public class AzureStorageAccountSettings
{
    public string AccountName { get; set; }
    public string ConnectionString { get; set; }
    public string StorageAccountKey { get; set; }
    public string StorageUri => $"https://{AccountName}.table.core.windows.net/";
}
