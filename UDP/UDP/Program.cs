using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static UDP.Enums;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using UDP.DTO;
using UDP.ServiceInterfaces;

namespace UDP
{
    class Program
    {
        //static DeviceClient deviceClient;


        static void Main(string[] args)
        {
            //Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(50002);

            //Creates an IPEndPoint to record the IP Address and port number of the sender.
            //The IPendPoint will allow you to read datagrams send from any source.
            //IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("10.0.10.136"), 50002);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            #region test
            //try
            //{
            //    //Blocks until a message returns on this socket from a remote host.
            //    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

            //    string returnData = Encoding.ASCII.GetString(receiveBytes);

            //    Console.WriteLine("This is the message you received " +
            //                                 returnData.ToString());
            //    Console.WriteLine("This message was sent from " +
            //                                RemoteIpEndPoint.Address.ToString() +
            //                                " on their port number " +
            //                                RemoteIpEndPoint.Port.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    Console.ReadKey();
            //}
            #endregion

            while (true)
            {
                try
                {
                    //Creates an IPEndPoint to record the IP Address and port number of the sender. 
                    //The IPEndPoint will allow you to read datagrams sent from any source.
                    Byte[] packet = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(packet);

                    int packetSize = BitConverter.ToUInt16(new byte[] { packet[4], packet[5] }, 0);
                    FlagIdEnum flag = (FlagIdEnum)packet[8];
                    TypeIdEnum type = (TypeIdEnum)packet[6];
                    var command = (Enums.AcquisitionCommandIdEnum)packet[7];

                    var x = new Program();
                    x.init(packet);
                    x.HeaderData(packet);

                    AcquisitionDataBlock adb = x.ProcessData(packet);

                    ContinuousBufferDto cb = new ContinuousBufferDto();
                    cb.WellId = adb.WellId;
                    cb.Time = adb.TimeStamp;
                    cb.DiffPressure = adb.Process2DP;
                    cb.Pressure = adb.Process4P;
                    cb.Temperature = adb.Process1Temp;

                    //Store ContinuousBufferDto to database.

                    Console.WriteLine("This is the message you received "); //+ returnData.ToString());
                    Console.WriteLine("Size : " + packetSize);
                    Console.WriteLine("Flag : " + flag);
                    Console.WriteLine("Type : " + type);

                    Console.WriteLine("Process1Temp : " + adb.Process1Temp);
                    Console.WriteLine("Process4P : " + adb.Process4P);


                    

                    Console.WriteLine("This message was sent from " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " on their port number " +

                                                RemoteIpEndPoint.Port.ToString());
                    Console.WriteLine("------------------------------------");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.ReadKey();
                }
            }
        }

        public UInt16 PredictionDataFormatMinorVersion { get; set; }
        public UInt16 PredictionDataFormatMajorVersion { get; set; }
        public UInt16 QueueDepth { get; set; }
        public UInt16 SoftwareVersion4 { get; set; }
        public UInt16 ModelId { get; set; }
        public UInt16 IniFileId { get; set; }
        public string PVTProfile { get; set; }
        public UInt16 ProjectNumber { get; set; }
        public UInt16 SerialNumber { get; set; }
        public UInt16 HWMinor { get; set; }
        public UInt16 HWMajor { get; set; }
        public UInt16 NoOfDataBlocks { get; set; }
        public UInt16 NoOfParametersPrDataBlock { get; set; }

        public Enums.TypeIdEnum PacketType { get; set; }
        //public Byte Command { get; set; }
        public Enums.FlagIdEnum Flag { get; set; }
        public byte PacketSequence { get; set; }
        public short Length { get; set; }
        public byte DataSequence { get; set; }
        public Byte[] ByteData { get; set; }

        public int CustomerId { get; set; }
        public int MeterId { get; set; }

        public float FrameRate { get; set; }

        public void HeaderData(Byte[] rawData)
        {
            this.PredictionDataFormatMinorVersion = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 10, 11));
            this.PredictionDataFormatMajorVersion = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 12, 13));

            this.QueueDepth = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 14, 15));

            this.SoftwareVersion4 = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 16, 17));
            this.ModelId = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 18, 19));
            this.IniFileId = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 20, 21));

            this.PVTProfile = Encoding.Default.GetString(GetByteArrayFromIndexes(0, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41)).Replace("\0", string.Empty);

            this.ProjectNumber = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 42, 43));
            this.SerialNumber = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 44, 45));

            this.HWMinor = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 46, 47));
            this.HWMajor = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 48, 49));
            this.NoOfDataBlocks = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 50, 51));
            this.NoOfParametersPrDataBlock = HelperMethods.ByteArrayToType<UInt16>(GetByteArrayFromIndexes(0, 52, 53));


            this.FrameRate = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(0, 54, 55, 56, 57));

            this.CustomerId = 3;
            this.MeterId = 1;
        }

        private void init(Byte[] rawData)
        {
            this.ByteData = rawData;
            PacketSequence = rawData[3];
            Length = BitConverter.ToInt16(new Byte[] { rawData[4], rawData[5] }, 0);
            PacketType = (Enums.TypeIdEnum)rawData[6];
            Flag = (Enums.FlagIdEnum)rawData[8];
            DataSequence = rawData[9];
        }

        private AcquisitionDataBlock ProcessData(Byte[] rawData)
        {

            AcquisitionDataBlock adb = new AcquisitionDataBlock();

            adb.CustomerId = CustomerId;
            adb.CustomerLocation = "Al Marsa Street , Dubai";
            //adb.MeterId = ProjectNumber;

            int offset = 0;
            int k = 24;

            adb.SecondsSinceEpoch = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 34 + k, 35 + k, 36 + k, 37 + k));

            adb.TimeStamp = TimeConverter.EpochToDateTime(adb.SecondsSinceEpoch);
            adb.TickCount = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 38 + k, 39 + k, 40 + k, 41 + k));

            adb.TimeStamp = adb.TimeStamp.AddMilliseconds(adb.TickCount * 0.1);

            adb.Status = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 42 + k, 43 + k, 44 + k, 45 + k));
            adb.ExperimentNumber = HelperMethods.ByteArrayToType<Int32>(GetByteArrayFromIndexes(offset, 46 + k, 47 + k, 48 + k, 49 + k));

            adb.Process1Temp = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 50 + k, 51 + k, 52 + k, 53 + k));
            adb.Process2DP = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 54 + k, 55 + k, 56 + k, 57 + k));
            adb.Process3DP = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 58 + k, 59 + k, 60 + k, 61 + k));
            adb.Process4P = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 62 + k, 63 + k, 64 + k, 65 + k));

            adb.PhaseFractionGas = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 66 + k, 67 + k, 68 + k, 69 + k));
            adb.PhaseFractionOil = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 70 + k, 71 + k, 72 + k, 73 + k));
            adb.PhaseFractionWater = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 74 + k, 75 + k, 76 + k, 77 + k));

            adb.VelocityGas = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 78 + k, 79 + k, 80 + k, 81 + k));
            adb.VelocityOil = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 82 + k, 83 + k, 84 + k, 85 + k));
            adb.VelocityWater = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 86 + k, 87 + k, 88 + k, 89 + k));

            adb.DensityGas = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 90 + k, 91 + k, 92 + k, 93 + k));
            adb.DensityOil = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 94 + k, 95 + k, 96 + k, 97 + k));
            adb.DensityWater = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 98 + k, 99 + k, 100 + k, 101 + k));

            adb.GasVolFlowStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 102 + k, 103 + k, 104 + k, 105 + k));
            adb.OilVolFlowStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 106 + k, 107 + k, 108 + k, 109 + k));
            adb.WaterVolFlowStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 110 + k, 111 + k, 112 + k, 113 + k));

            adb.CrossSectionArea = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 114 + k, 115 + k, 116 + k, 117 + k));

            adb.VelocityCorrelation = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 118 + k, 119 + k, 120 + k, 121 + k));
            adb.Spread = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 122 + k, 123 + k, 124 + k, 125 + k));
            //adb.SmearedDP = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 126, 127, 128, 129));

            adb.MixVelocity = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 126 + k, 127 + k, 128 + k, 129 + k));
            adb.GOR1 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 130 + k, 131 + k, 132 + k, 133 + k));
            adb.RhoGasStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 134 + k, 135 + k, 136 + k, 137 + k));
            adb.RhoOilStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 138 + k, 139 + k, 140 + k, 141 + k));
            adb.RhoWaterStd = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 142 + k, 143 + k, 144 + k, 145 + k));
            adb.MixPermittivity = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 146 + k, 147 + k, 148 + k, 149 + k));
            adb.OilPermittivity = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 150 + k, 151 + k, 152 + k, 153 + k));
            adb.MixConductivity = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 154 + k, 155 + k, 156 + k, 157 + k));
            adb.WaterConductivity = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 158 + k, 159 + k, 160 + k, 161 + k));
            adb.TempFrontEndElectonics = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 162 + k, 163 + k, 164 + k, 165 + k));
            adb.TempDpCellTransmitter = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 166 + k, 167 + k, 168 + k, 169 + k));

            adb.StatisticInfo1 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 170 + k, 171 + k, 172 + k, 173 + k));
            adb.StatisticInfo2 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 174 + k, 175 + k, 176 + k, 177 + k));
            adb.StatisticInfo3 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 178 + k, 179 + k, 180 + k, 181 + k));
            adb.StatisticInfo4 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 182 + k, 183 + k, 184 + k, 185 + k));

            adb.ExternalSensorData1 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 186 + k, 187 + k, 188 + k, 189 + k));
            adb.ExternalSensorData2 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 190 + k, 191 + k, 192 + k, 193 + k));
            adb.ExternalSensorData3 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 194 + k, 195 + k, 196 + k, 197 + k));
            adb.ExternalSensorData4 = HelperMethods.ByteArrayToType<float>(GetByteArrayFromIndexes(offset, 198 + k, 199 + k, 200 + k, 201 + k));

            adb.Alarms = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 202 + k, 203 + k, 204 + k, 205 + k));
            adb.Warnings = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 206 + k, 207 + k, 208 + k, 209 + k));

            adb.EndOfRecordCheck = HelperMethods.ByteArrayToType<UInt32>(GetByteArrayFromIndexes(offset, 210 + k, 211 + k, 212 + k, 213 + k));



            string jsonADB = JsonConvert.SerializeObject(adb);

            SendDeviceToCloudMessagesAsync(jsonADB);

            return adb;
        }

        private static async void SendDeviceToCloudMessagesAsync(string adbJson)
        {
            double minTemperature = 20;
            Random rand = new Random();

            double currentTemperature = minTemperature + rand.NextDouble() * 15;
            var messageString = adbJson;
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            //await deviceClient.SendEventAsync(message);

            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

            await Task.Delay(1000);
        }

        protected Byte[] GetByteArrayFromIndexes(int offset, params int[] indexes)
        {
            Byte[] retval = new Byte[indexes.Length];

            for (int i = 0; i < indexes.Length; i++)
            {
                retval[i] = this.ByteData[indexes[i] + offset];
            }
            return retval;
        }
    }
}
