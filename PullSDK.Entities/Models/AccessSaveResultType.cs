namespace PullSDKDataListener.Entities.Models
{
    public enum AccessSaveResultType
    {
        Inserted,        // Yeni kayıt eklendi
        PersonNotFound,  // Kart/PIN ile eşleşen kişi yok
        Duplicate,       // Aynı kayıt zaten var
        Error            // DB hatası
    }
}
