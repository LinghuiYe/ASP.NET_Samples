using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    public static class Enums
    {
        public enum MeasurementStatusEnum
        {
            NotConclusive = 0,
            Capacitive = 1,
            Conductive = 2
        };

        public enum MassContinuityDeviationEnum
        {
            NotInUse = 0,
            ZeroToOne = 1,
            OneToThree = 2,
            ThreeToFive = 3,
            FiveToTen = 4,
            TenToTwenty = 5,
            TwentyToFifty = 6,
            FiftyPlus = 7
        };

        public enum AcquisitionCommandIdEnum
        {
            GetMeasurementData = 33,
            GetTraceData = 34,
            GetAvgPredData = 35
        };

        public enum TypeIdEnum
        {
            Control = 33,
            MeasureSmall = 34,
            MeasureLarge = 35,
            FileTransfer = 36,
            Console = 37
        };

        public enum FlagIdEnum
        {
            ACK = 1,
            NAK = 2,
            INCOMPLETE = 4,
            NOTUSED = 8,
            MEASWARNING = 16,
            SYSTEMMESSAGE = 32,
            SYSTEMERROR = 64,
            REQUEST = 128
        };


        public enum FlowModelEnum
        {
            Single_Phase = 0,
            Universal_Model = 1,
            Added_Model_1 = 2,
            Added_Model_2 = 3,
            Added_Model_3 = 4,
            Added_Model_4 = 5,
            Transition_Model = 6,
            Homogeneous_Model = 7
        };

        public enum StatusRegisterMasksEnum : uint
        {
            PredictionDataAvailable = 1,
            MeasurementStatus = 3,
            FlowModel = 7,
            ValidCorrelation = 1,
            VelocityIsGas = 1,
            NumberOfPermittivityConductivityData = 63,
            MassContinuityDeviation = 15,
            VelocityIndex = 15,
            DPIndex = 15,
            ModelState = 7,
            ModelModus = 3
        };
    }
}
