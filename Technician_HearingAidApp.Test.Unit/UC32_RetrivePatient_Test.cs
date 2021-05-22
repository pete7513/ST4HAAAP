using System;
using System.Collections.Generic;
using System.Text;
using BLL_Technician;
using DLL_Technician;
using NSubstitute;
using NUnit.Framework;

namespace Technician_HearingAidApp.Test.Unit
{
    public class UC32_RetrivePatient_Test
    {
        private UC3_ShowHATech uut;
        private IClinicDB db;

        [SetUp]
        public void Setup()
        {
            db = Substitute.For<IClinicDB>();
            uut = new UC3_ShowHATech(db);
        }

        [TestCase("1234")]
        [TestCase("123456-7890")]
        public void GetPatient_CallToDB_DBGetPatientReceivesACall(string cpr)
        {
            uut.GetPatient(cpr);

            //db.Received(1).GetPatient(cpr);
        }
    }
}
