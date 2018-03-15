using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureShare.Website.Models
{
    public class ResultAuth
    {
        public bool IsPerson { get; }
        public double Percentage { get; }
        public string PersonVerifyResult { get; }

        public ResultAuth(double result)
        {
            Percentage = result;
            if (Percentage > 0.5){
                PersonVerifyResult = "The face belongs to the person" + Percentage;
                IsPerson = true;
            }else {
                PersonVerifyResult = "The face not belong to the person" + Percentage;
                IsPerson = false;
            }
        }
    }

    
}
