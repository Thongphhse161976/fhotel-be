using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Repository.SMS
{
    public interface ISpeedSMSAPI
    {
        public string SendOTP(string phoneNumber, string otpCode);
    }
}
