using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L05;

public class AzureStorageAccountSettings
{
    public string AccountName => "datcstorage";
    public string ConnectionString => "DefaultEndpointsProtocol=https;AccountName=datcstorage;AccountKey=/2oP05/AHX/K6IND8XCpGMM8sjNfkSRsxpeFntOH8+D6xbTFU06hT2SCr7cmLatHvESlbCEuQHzFb8QzYZ2rhQ==;EndpointSuffix=core.windows.net";
    public string StorageAccountKey => "/2oP05/AHX/K6IND8XCpGMM8sjNfkSRsxpeFntOH8+D6xbTFU06hT2SCr7cmLatHvESlbCEuQHzFb8QzYZ2rhQ==";
    public string StorageUri => $"https://{AccountName}.table.core.windows.net/";
}
