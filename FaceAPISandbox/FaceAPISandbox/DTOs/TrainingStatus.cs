using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAPISandbox.DTOs
{
    public class TrainingStatus
    {
        public Status Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastActionDateTime { get; set; }
        public DateTime? LastSuccessfulTrainingDateTime { get; set; }
        public string Message { get; set; }
    }

    public enum Status
    {
        Succeeded = 0,
        Failed = 1,
        Running = 2
    }
}
