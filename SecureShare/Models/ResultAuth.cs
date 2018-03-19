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
            if (result > 0.5){
                PersonVerifyResult = "The face belongs to the person.";
                IsPerson = true;
            }else {
                PersonVerifyResult = "The face not belong to the person";
                IsPerson = false;
            }
        }
    }

    
}
