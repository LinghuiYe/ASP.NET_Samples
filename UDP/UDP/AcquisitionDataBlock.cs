using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    public class AcquisitionDataBlock
    {
        public Int32 CustomerId { get; set; }
        public string CustomerLocation { get; set; }
        public Int32 WellId { get; set; }


        public UInt32 SecondsSinceEpoch { get; set; }
        public DateTime TimeStamp { get; set; }

        public UInt32 TickCount { get; set; }
        public Int32 ExperimentNumber { get; set; }
        public float Process1Temp { get; set; }                         // (Pt-100)
        public float Process2DP { get; set; }                           // (4-20mA)
        public float Process3DP { get; set; }                           // (Hart)
        public float Process4P { get; set; }                            // (Hart)
        public float PhaseFractionGas { get; set; }
        public float PhaseFractionOil { get; set; }
        public float PhaseFractionWater { get; set; }
        public float VelocityGas { get; set; }
        public float VelocityOil { get; set; }
        public float VelocityWater { get; set; }
        public float DensityGas { get; set; }                               // Density
        public float DensityOil { get; set; }                               // Density
        public float DensityWater { get; set; }                             // Density
        public float GasVolFlowStd { get; set; }                              // Standard Condition
        public float OilVolFlowStd { get; set; }                              // Standard Condition
        public float WaterVolFlowStd { get; set; }                            // Standard Condition
        public float CrossSectionArea { get; set; }
        public float VelocityCorrelation { get; set; }
        public float Spread { get; set; }
        public float SmearedDP { get; set; }
        public float PermittivityConductivityAverageChannel0 { get; set; }
        public float PermittivityConductivityAverageChannel1 { get; set; }

        public float MixVelocity { get; set; }
        public float GOR1 { get; set; }
        public float RhoGasStd { get; set; }
        public float RhoOilStd { get; set; }
        public float RhoWaterStd { get; set; }
        public float MixPermittivity { get; set; }
        public float OilPermittivity { get; set; }
        public float MixConductivity { get; set; }
        public float WaterConductivity { get; set; }
        public float TempFrontEndElectonics { get; set; }
        public float TempDpCellTransmitter { get; set; }

        //FlowProtocol 1.2
        public float StatisticInfo1 { get; set; }
        public float StatisticInfo2 { get; set; }
        public float StatisticInfo3 { get; set; }
        public float StatisticInfo4 { get; set; }

        public float ExternalSensorData1 { get; set; }
        public float ExternalSensorData2 { get; set; }
        public float ExternalSensorData3 { get; set; }
        public float ExternalSensorData4 { get; set; }

        //Add By Linghui
        public float MyOil { get; set; }
        public float MyLiq { get; set; }
        public float GasStd { get; set; }
        public float OilStd { get; set; }
        public float WaterStd { get; set; }
        public UInt32 CumulationTimeStamp { get; set; }

        public UInt32 Alarms { get; set; }
        public UInt32 Warnings { get; set; }
        public UInt32 EndOfRecordCheck { get; set; }

        // Status Register Values
        public bool MeasurementDataAvailable { get; set; }
        public Enums.MeasurementStatusEnum MeasurementStatus { get; set; }
        public String FlowModel { get; set; }
        public bool ValidCorrelation { get; set; }
        public bool VelocityIsGas { get; set; }
        public short NumberOfPermittivityConductivityData { get; set; }
        public Enums.MassContinuityDeviationEnum MassContinuityDeviation { get; set; }
        public short VelocityIndex { get; set; }
        public short DPIndex { get; set; }

        public short ModelState { get; set; }
        public short ModelModus { get; set; }

        public short Transition { get; set; }

        public List<float> AdmittanceData { get; set; } // 16

        public UInt32 Status
        {
            set
            {
                this.MeasurementDataAvailable = GetBooleanValueFromInt(value, 0);
                this.MeasurementStatus = (Enums.MeasurementStatusEnum)GetValueFromBits(value, 1, (uint)Enums.StatusRegisterMasksEnum.MeasurementStatus);

                Enums.FlowModelEnum flowModelVal = (Enums.FlowModelEnum)GetValueFromBits(value, 3, (uint)Enums.StatusRegisterMasksEnum.FlowModel);
                this.FlowModel = flowModelVal.ToString().Replace('_', ' ');
                this.ValidCorrelation = GetBooleanValueFromInt(value, 6);
                this.VelocityIsGas = GetBooleanValueFromInt(value, 7);
                this.NumberOfPermittivityConductivityData = GetValueFromBits(value, 8, (uint)Enums.StatusRegisterMasksEnum.NumberOfPermittivityConductivityData);
                this.MassContinuityDeviation = (Enums.MassContinuityDeviationEnum)GetValueFromBits(value, 14, (uint)Enums.StatusRegisterMasksEnum.MassContinuityDeviation);
                this.VelocityIndex = GetValueFromBits(value, 18, (uint)Enums.StatusRegisterMasksEnum.VelocityIndex);
                this.DPIndex = GetValueFromBits(value, 22, (uint)Enums.StatusRegisterMasksEnum.DPIndex);

                this.ModelState = GetValueFromBits(value, 26, (uint)Enums.StatusRegisterMasksEnum.ModelState);
                this.ModelModus = GetValueFromBits(value, 29, (uint)Enums.StatusRegisterMasksEnum.ModelModus);

            }
        }

        private bool GetBooleanValueFromInt(UInt32 input, int mask)
        {
            return Convert.ToBoolean((input & (1 << mask)) >> mask);
        }

        private Int16 GetValueFromBits(UInt32 bits, int offset, UInt32 mask)
        {
            UInt32 value = (bits >> (offset)) & mask;
            return Convert.ToInt16(value);
        }

        public AcquisitionDataBlock()
        {
            this.AdmittanceData = new List<float>();
        }

    }
}