using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP.DTO
{
    public class ContinuousBufferDto
    {
        public Int32 MeterId { get; set; }
        public Int32 WellId { get; set; }
        public DateTime Time { get; set; }
        public Double DiffPressure { get; set; }
        public Double Pressure { get; set; }
        public Double Temperature { get; set; }

    }
}
