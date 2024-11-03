using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Repository.SMS
{
    public class InMemoryOtpStore
    {
        private readonly ConcurrentDictionary<string, (string otp, DateTime expirationTime)> _otpStore;

        public InMemoryOtpStore()
        {
            _otpStore = new ConcurrentDictionary<string, (string otp, DateTime expirationTime)>();
        }

        public void StoreOTP(string phoneNumber, string otpCode, TimeSpan expiration)
        {
            var expirationTime = DateTime.UtcNow.Add(expiration);
            _otpStore[phoneNumber] = (otpCode, expirationTime);
        }

        public bool ValidateOTP(string phoneNumber, string otpCode)
        {
            if (_otpStore.TryGetValue(phoneNumber, out var storedOtp))
            {
                if (DateTime.UtcNow <= storedOtp.expirationTime)
                {
                    return storedOtp.otp == otpCode;
                }
                else
                {
                    
                    _otpStore.TryRemove(phoneNumber, out _);
                }
            }
            return false;
        }
    }
}
