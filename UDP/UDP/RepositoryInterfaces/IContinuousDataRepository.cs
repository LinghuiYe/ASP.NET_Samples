using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDP.DTO;

namespace UDP.RepositoryInterfaces
{
    public interface IContinuousDataRepository
    {
        void CreateContinuousData(ContinuousBufferDto continuousData);

        ObservableCollection<ContinuousBufferDto> GetContinuousData();

        void DeleteContinuousData(ContinuousBufferDto continuousData);
    }
}
