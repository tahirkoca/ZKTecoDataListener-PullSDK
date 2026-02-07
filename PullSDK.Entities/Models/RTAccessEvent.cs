using System;

namespace PullSDKDataListener.Entities.Models
{
    public class RTAccessEvent
    {
        public DateTime Time { get; set; }
        public string CardNo { get; set; }
        public string Pin { get; set; }
        public string Mode { get; set; } // "Kart", "Şifre", "Kart+Şifre", "Bilinmiyor"
    }
}
