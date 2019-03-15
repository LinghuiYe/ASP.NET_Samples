using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDP.DTO;
using UDP.RepositoryInterfaces;
using UDP.ServiceInterfaces;

namespace UDP.Services
{
    public class ContinuousDataService : IContinuousDataService
    {
        private IContinuousDataRepository ContinuousDataRepository;
        public ContinuousDataService(IContinuousDataRepository continuousDataRepository)
        {
            this.ContinuousDataRepository = continuousDataRepository;
        }

        public void CreateContinuousData(ContinuousBufferDto cd)
        {
            this.ContinuousDataRepository.CreateContinuousData(cd);
        }

        public void DeleteContinuousData(ContinuousBufferDto cd)
        {
            this.ContinuousDataRepository.DeleteContinuousData(cd);
        }

        public ObservableCollection<ContinuousBufferDto> GetContinuousData()
        {
            return this.ContinuousDataRepository.GetContinuousData();
        }
    }
}
