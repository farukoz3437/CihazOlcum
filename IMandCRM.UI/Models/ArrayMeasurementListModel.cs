using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ArrayMeasurementListModel
    {
        public string ArrayDesignIdKod { get; set; }
        public List<ArrayMeasurement> arrayMeasurements { get; set; }
    }
}
