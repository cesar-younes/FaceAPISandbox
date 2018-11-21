using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceAPISandbox.DTOs;

namespace FaceAPISandbox.Interfaces
{
    public interface IFaceService
    {
        Task CreatePersonGroupIfNotExistsAsync(string personGroupId, string personGroupName);
        Task<CreatePersonResult> AddEmptyPersonToGroupAsync(string personName);
        Task AddFaceToPersonAsync(Guid personId, string imageUrl);
        Task TrainPersonGroupAsync();
        Task<TrainingStatus> GetTrainingStatus();
        Task<DetectedFace> DetectFacesAsync(string url);
        Task<RecognizedFace> RecognizeFacesAsync(Guid[] faceIds, string url);
    }
}
