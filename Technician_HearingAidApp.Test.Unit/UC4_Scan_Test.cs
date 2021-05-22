using BLL_Technician;
using CoreEFTest.Models;
using DLL_Technician;
using NUnit.Framework;
using NSubstitute;



namespace Technician_HearingAidApp.Test.Unit
{
    public class UC4_Scan_Test
    {
        private UC4_Scan uut;
        private IClinicDB _clinicDB;
        private IScanner _scanner;

        [SetUp]
        public void Setup()
        {
            _scanner = Substitute.For<IScanner>();
            _clinicDB = Substitute.For<IClinicDB>();
            uut = new UC4_Scan(_clinicDB, _scanner);

        }

        [Test]
        public void UC4_Scan_ConnectToScanner_ScannerRecievesConnectCall()
        {
            //Act
            uut.ConnectToScanner();

            //Test
            _scanner.Received().connectTo3DScanner();

        }

        [Test]
        public void UC4_Scan_StartScanner_ScannerRecievesStartCall()
        {
            uut.StartScanning(Ear.Left);

            _scanner.Received().StartScanning(Ear.Left);
        }


        [Test]
        public void UC4_Scan_GetPatientInformations_ClinicRecievesGetCall()
        {
            uut.GetPatientInformations("1111111-0000");

            _clinicDB.Received().GetPatientInformations("1111111-0000");
        }
    }
}