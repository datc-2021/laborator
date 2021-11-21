using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class QueueTrigger1
    {
        [Function("QueueTrigger1")]
        [TableOutput("studenti")]
        public static StudentEntity Run([QueueTrigger("students-q", Connection = "azurestorageroxie_STORAGE")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("QueueTrigger1");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);
            return student;
        }
    }
}
