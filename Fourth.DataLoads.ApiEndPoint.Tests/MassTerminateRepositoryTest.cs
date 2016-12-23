using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fourth.DataLoads.Data.Entities;
using System.Collections.Generic;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using Moq;
using AutoMapper;
using Fourth.DataLoads.ApiEndPoint.Controllers;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.ApiEndPoint;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class MassTerminateRepositoryTest
    {
        private List<MassTerminationModelSerialized> Serializedmodels
           = new List<MassTerminationModelSerialized>();
        private List<MassTerminationModel> models
            = new List<MassTerminationModel>();

        public MassTerminateRepositoryTest()
        {
            SetupModelObjects();
        }

        private void SetupModelObjects()
        {

            // ARRANGE
            models.Clear();

            IMappingFactory mapper = new MappingFactory();

            models.Add(new MassTerminationModel
            {
                EmployeeNumber = "121",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Training required"
            });

            models.Add(new MassTerminationModel
            {
                EmployeeNumber = "122",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Testing termination if it works"
            });

            models.Add(new MassTerminationModel
            {
                EmployeeNumber = "123",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Over qualified for the job"
            });

            Serializedmodels = mapper.Mapper.Map<List<MassTerminationModelSerialized>>(models);
        }
    }
}
