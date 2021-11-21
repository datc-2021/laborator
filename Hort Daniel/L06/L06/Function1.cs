using System;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace L06
{
    public class Function1
    {
        [FunctionName("Function1")]
        [return: Table("Students", Connection = "DatcStorage")]
        public StudentEntity Run([QueueTrigger("datc6-queue")] CloudQueueMessage message, ILogger log)
        {
            log.LogInformation($"New message: {message.AsString}");
            StudentEntity student = default;
            try { student = JsonConvert.DeserializeObject<StudentEntity>(message.AsString); }catch { }
            if (student is null)
                log.LogWarning($"Could not deserialize message into StudentEntity");
            return student;
        }
    }
}
