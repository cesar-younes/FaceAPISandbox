using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAPISandbox.DTOs
{
    public class DetectedFace
    {
        public Guid PersonId { get; set; }
        public Guid[] PersistedFaceIds { get; set; }
        public string Name { get; set; }
        public string UserData { get; set; }
    }
}
