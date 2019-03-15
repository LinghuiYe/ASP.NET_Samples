using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDP.DTO;
using UDP.RepositoryInterfaces;

namespace UDP.Repositories
{
    public class ContinuousDataRepository : IContinuousDataRepository
    {
        public void CreateContinuousData(ContinuousBufferDto continuousData)
        {
            ContinuousBufferDto cd = new ContinuousBufferDto();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLServer"].ToString();
            if (connectionString != string.Empty)
            {
                var ValueExist = false;
                try
                {
                    SqlConnection checkConnection = new SqlConnection(connectionString);
                    checkConnection.Open();
                    SqlCommand cmdCheck = new SqlCommand("Select * From [Abbon.UDP.Database].dbo.ContinuousData Where WellId = " + continuousData.WellId
                        + " and Time = " + continuousData.Time
                        + " and DiffPressure = " + continuousData.DiffPressure
                        + " and Pressure = " + continuousData.Pressure
                        + " and Temperature = " + continuousData.Temperature);
                    var valueFound = cmdCheck.ExecuteScalar();
                    checkConnection.Close();

                    if (valueFound == null)
                    {
                        ValueExist = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Verbose("Exception raised in SQL Command", ex.Message);
                }

                if (ValueExist)
                {
                    try
                    {
                        SqlConnection sc = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "INSERT [Abbon.UDP.Database].dbo.ContinuousData(WellId, Time,DiffPressure,Pressure,Temperature)" +
                            "VALUES(@WellId, @Time,@DiffPressure,@Pressure,@Temperature)";
                        cmd.Parameters.AddWithValue("(@WellId", continuousData.WellId);
                        cmd.Parameters.AddWithValue("(@Time", continuousData.Time);
                        cmd.Parameters.AddWithValue("(@DiffPressure", continuousData.DiffPressure);
                        cmd.Parameters.AddWithValue("(@Pressure", continuousData.Pressure);
                        cmd.Parameters.AddWithValue("(@Temperature", continuousData.Temperature);
                        cmd.Connection = sc;
                        sc.Open();
                        cmd.ExecuteNonQuery();
                        sc.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Verbose("Exception raised in SQL Command", ex.Message);
                    }
                }
            }
            else
            {
                Log.Fatal("Could not connect to SQL Server.  Connection String with name SQLServer Missing!");
            }
        }

        public void DeleteContinuousData(ContinuousBufferDto continuousData)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ContinuousBufferDto> GetContinuousData()
        {
            using (Entities entity = new Entities())
            {
                ObservableCollection<ContinuousBufferDto> retval = new ObservableCollection<ContinuousBufferDto>();
                foreach (ContinuousData cd in entity.ContinuousDatas.OrderBy(x => x.Id))
                {
                    ContinuousBufferDto cbd = new ContinuousBufferDto();
                    cbd.WellId = cd.WellId;
                    cbd.Time = cd.Time;
                    cbd.DiffPressure = cd.DiffPressure;
                    cbd.Pressure = cd.Pressure;
                    cbd.Temperature = cd.Temperature;

                    retval.Add(cbd);
                }
                return retval;
            }
        }
    }
}
