using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreEFTest.Context;
using CoreEFTest.Models;
using DTO;

namespace ShowProcess
{
   public class DLLShowProcess
    {
        private readonly ClinicDBContext _dbContext;

        public DLLShowProcess(ClinicDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Henter Process informationerne til det give CPR. Metoden vil altid hente de nyeste informationer. 
        /// </summary>
        /// <param name="CPR"></param>
        /// <returns></returns>
        public List<ProcesSpec> GetProcesInfo(string CPR)
        {

            List<ProcesSpec> procesSpecs = new List<ProcesSpec>();
            ProcesSpec procesSpecL = new ProcesSpec();
            ProcesSpec procesSpecR = new ProcesSpec();

            try
            {
                try
                {
                    //Venstre
                    //Henter generalspec
                    GeneralSpec generalSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.CPR == CPR && x.EarSide == Ear.Left);
                    if (generalSpecL.CreateDate != null)
                    {
                        procesSpecL.GeneralSpecCreateDateTime = generalSpecL.CreateDate;
                        procesSpecL.ClinicianId = generalSpecL.StaffID;
                        TecnicalSpec TechspecL = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.HAGenerelSpecID == generalSpecL.HAGeneralSpecID);

                        //Henter techspec
                        if (TechspecL.CreateDate != null)
                        {
                            procesSpecL.TechSpecCreateDateTime = TechspecL.CreateDate;
                            procesSpecL.TechnicalId = TechspecL.StaffID;
                            procesSpecL.Printed = procesSpecL.Printed;

                            //Henter EarScan
                            TechspecL.RawEarScan = _dbContext.RawEarScans.OrderBy(x => x.ScanDate).LastOrDefault(x => x.HATechnicalSpecID == TechspecL.HATechinalSpecID);
                            if (TechspecL.RawEarScan != null)
                            {
                                procesSpecL.scanTechId = TechspecL.StaffID;
                                procesSpecL.scanDateTime = TechspecL.CreateDate;
                            }

                            //Henter Earprint
                            if (procesSpecL.Printed)
                            {
                                RawEarPrint rawEarPrint = _dbContext.RawEarPrints.OrderBy(x => x.PrintDate).LastOrDefault(x => x.HATechnicalSpecID == TechspecL.HATechinalSpecID && x.EarSide == Ear.Left);
                                ;
                                procesSpecL.PrintDateTime = rawEarPrint.PrintDate;
                                procesSpecL.PrintTechId = rawEarPrint.StaffID;
                            }
                        }
                    }

                    procesSpecs.Add(procesSpecL);
                }
                catch
                {
                    procesSpecs.Add(procesSpecL);
                }

                try
                {
                    //Højre
                    //Henter GeneralSpec
                    GeneralSpec generalSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.CPR == CPR && x.EarSide == Ear.Right);
                    if (generalSpecR.CreateDate != null)
                    {
                        procesSpecR.GeneralSpecCreateDateTime = generalSpecR.CreateDate;
                        procesSpecR.ClinicianId = generalSpecR.StaffID;
                        TecnicalSpec TechspecR = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.HAGenerelSpecID == generalSpecR.HAGeneralSpecID);

                        //Henter techspec
                        if (TechspecR.CreateDate != null)
                        {
                            procesSpecR.TechSpecCreateDateTime = TechspecR.CreateDate;
                            procesSpecR.TechnicalId = TechspecR.StaffID;
                            procesSpecR.Printed = procesSpecR.Printed;

                            //Henter EarScan
                            TechspecR.RawEarScan = _dbContext.RawEarScans.OrderBy(x => x.ScanDate).LastOrDefault(x => x.HATechnicalSpecID == TechspecR.HATechinalSpecID);
                            if (TechspecR.RawEarScan != null)
                            {
                                procesSpecR.scanTechId = TechspecR.StaffID;
                                procesSpecR.scanDateTime = TechspecR.CreateDate;
                            }

                            //Henter Earprint
                            if (procesSpecR.Printed)
                            {
                                RawEarPrint rawEarPrint = _dbContext.RawEarPrints.OrderBy(x => x.PrintDate).LastOrDefault(x => x.HATechnicalSpecID == TechspecR.HATechinalSpecID && x.EarSide == Ear.Right);
                                procesSpecR.PrintDateTime = rawEarPrint.PrintDate;
                                procesSpecR.PrintTechId = rawEarPrint.StaffID;
                            }
                        }
                    }

                    procesSpecs.Add(procesSpecR);
                }
                catch
                {
                    procesSpecs.Add(procesSpecR);
                }

                return procesSpecs;
            }
            catch
            {
                return null;
            }
        }
    }
}
