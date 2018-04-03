using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureShare.Website.Models
{
    public class ResultAuth
    {
        public bool IsPerson { get; }
        public string PersonVerifyResult { get; }

        public ResultAuth(double result)
        {
            if (result > 0.7){
                PersonVerifyResult = "The face belongs to the person. " +
                                     "We know that for " + result * 100 + "% sure.";
                IsPerson = true;
            }else {
                PersonVerifyResult = "The face not belong to the person. " +
                                     "We know that for " + (100 -(result * 100)) + "% sure.";
                IsPerson = false;
            }
        }
    }
}
