using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDP.DTO;

namespace UDP.ServiceInterfaces
{
    public interface IContinuousDataService
    {
        void CreateContinuousData(ContinuousBufferDto cd);

        ObservableCollection<ContinuousBufferDto> GetContinuousData();

        void DeleteContinuousData(ContinuousBufferDto cd);
    }
}
