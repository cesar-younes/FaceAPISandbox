using FaceAPISandbox.DTOs;
using FaceAPISandbox.Interfaces;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAPISandbox.Services
{
    public class CognitiveFaceService : IFaceService
    {
        static readonly string FACEKEY = Environment.GetEnvironmentVariable("FaceKey");
        static readonly string FACEENDPOINT = "northeurope";
        private static FaceServiceClient _faceServiceClient = new FaceServiceClient(FACEKEY, FACEENDPOINT);

        public async Task<CreatePersonResult> AddEmptyPersonToGroupAsync(string personName)
        {
            try
            {
                var personGroups = await _faceServiceClient.ListPersonGroupsAsync();
                var personGroup = personGroups[0];
                var result =  await _faceServiceClient.CreatePersonInLargePersonGroupAsync(
                    personGroup.PersonGroupId,
                    personName
                );

                return new CreatePersonResult()
                {
                    PersonId = result.PersonId
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task AddFaceToPersonAsync(Guid personId, string imageUrl)
        {
            try
            {
                var personGroups = await _faceServiceClient.ListLargePersonGroupsAsync();
                var personGroup = personGroups[0];
                await _faceServiceClient.AddPersonFaceInLargePersonGroupAsync(personGroup.LargePersonGroupId, personId, imageUrl);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task CreatePersonGroupIfNotExistsAsync(string personGroupId, string personGroupName)
        {
            try
            {
                var listOfPersonGroups = await _faceServiceClient.ListPersonGroupsAsync();

                if (listOfPersonGroups.Length == 0)
                {
                    await _faceServiceClient.CreateLargePersonGroupAsync(personGroupId, personGroupName);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<DetectedFace> DetectFacesAsync(string url)
        {
            var faces = await _faceServiceClient.DetectAsync(url);
            var faceIds = faces.Select(face => face.FaceId).ToArray();
            var personGroupList = await _faceServiceClient.ListLargePersonGroupsAsync();
            for (int x = 0; x <= personGroupList.Length; x++)
            {
                var results = await _faceServiceClient.IdentifyAsync(personGroupList[x].LargePersonGroupId, faceIds);
                foreach (var identifyResult in results)
                {
                    //Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var Person = await _faceServiceClient.GetPersonAsync(personGroupList[x].LargePersonGroupId, candidateId);
                        return new DetectedFace()
                        {
                            Name = Person.Name,
                            PersonId = Person.PersonId,
                            PersistedFaceIds = Person.PersistedFaceIds,
                            UserData = Person.UserData
                        };
                    }
                }
            }
            return null;
        }

        public async Task<TrainingStatus> GetTrainingStatus()
        {
            var personGroups = await _faceServiceClient.ListLargePersonGroupsAsync();
            var personGroup = personGroups[0];
            var trainingStatus =  await _faceServiceClient.GetLargePersonGroupTrainingStatusAsync(personGroup.LargePersonGroupId);
            Status status = (Status)Enum.Parse(typeof(TrainingStatus), trainingStatus.Status.ToString(), true);
            return new TrainingStatus()
            {
                Status = status,
                CreatedDateTime = trainingStatus.CreatedDateTime,
                LastActionDateTime = trainingStatus.LastActionDateTime,
                LastSuccessfulTrainingDateTime = trainingStatus.LastSuccessfulTrainingDateTime,
                Message = trainingStatus.Message
            };
        }

        public async Task<RecognizedFace> RecognizeFacesAsync(Guid[] faceIds, string url)
        {
            throw new NotImplementedException();
        }

        public async Task TrainPersonGroupAsync()
        {
            try
            {
                var personGroups = await _faceServiceClient.ListLargePersonGroupsAsync();
                var personGroup = personGroups[0];
                await _faceServiceClient.TrainLargePersonGroupAsync(personGroup.LargePersonGroupId);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
