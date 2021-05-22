using System;
using System.Collections.Generic;
using System.Text;
using BLL_Clinician;
using CoreEFTest.Models;
using DLL_Clinician;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;


namespace Clinician_HearingAidApp.Test.Unit
{
    public class UC2_ManagePatient_UnitTest
    {

        private UC2_ManagePatient uut;
        private IClinicDatabase clinicDatabase;
        private FakePatient patient;

        [SetUp]
        public void Setup()
        {
            uut = new UC2_ManagePatient();
            clinicDatabase = Substitute.For<IClinicDatabase>();
            patient = new FakePatient();

        }

        [Test]
        public void SaveUpdates_ExpectedResult_CallDatabaseUpdate()
        {
            uut.SaveUpdates(patient);

            clinicDatabase.Received(1).UpdatePatient(patient);
        }

        [Test]
        public void SavePatientPressed_ExpectedResult_CallDatabaseUpdate()
        {
            uut.SavePatient(patient);

            clinicDatabase.Received(1).CreatePatient(patient);
        }

        [Test]
        public void CheckCPR_ExpectedResult_(){}

        [TestCase(123456-7890)]
        public void GetPatientInformation_ExpectedResult_CallGetPatient(string cpr)
        {
            uut.GetPatientInformation(Convert.ToString(cpr));

            clinicDatabase.Received(1).GetPatient(Convert.ToString(cpr));
        }

        [Test]
        public void GetPatientInformation_ExpectedResult_CallGetPatient()
        {
            uut.GetPatientInformation("1234");

            clinicDatabase.Received(1).GetPatient("1234");
        }
    }

    public class FakePatient: Patient
    {
        public string CPR { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
    }

   
}
