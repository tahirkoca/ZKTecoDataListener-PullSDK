using PullSDKDataListener.Entities.Models;
using System;
using System.Data.SqlClient;

namespace PullSDKDataListener.DAL.Repositories
{
    public class AccessEventRepository
    {
        private readonly string _connectionString;
        private readonly int _defaultFirmaId;
        private readonly int _cihazId;

        public AccessEventRepository(string connectionString, int defaultFirmaId, int cihazId)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _defaultFirmaId = defaultFirmaId;
            _cihazId = cihazId;
        }

        public AccessSaveResult SaveAccessEvent(RTAccessEvent ev)
        {
            var result = new AccessSaveResult
            {
                ResultType = AccessSaveResultType.Error,
                ErrorMessage = null
            };

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    int? personelId = null;
                    int? firmaIdFromPerson = null;

                    // 1) Kişiyi KartNo veya Sicil (PIN) ile bul
                    using (var cmdFind = conn.CreateCommand())
                    {
                        cmdFind.CommandText = @"
SELECT TOP 1 PersonelId, FirmaId
FROM Kisiler
WHERE 
    (@KartNo <> '' AND KartNo = @KartNo)
    OR
    (@Pin <> '' AND PersonelId = @Pin);";

                        cmdFind.Parameters.AddWithValue("@KartNo", (object)(ev.CardNo ?? "") ?? "");
                        cmdFind.Parameters.AddWithValue("@Pin", (object)(ev.Pin ?? "") ?? "");

                        using (var dr = cmdFind.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                personelId = Convert.ToInt32(dr["PersonelId"]);
                                if (!dr.IsDBNull(dr.GetOrdinal("FirmaId")))
                                    firmaIdFromPerson = Convert.ToInt32(dr["FirmaId"]);
                            }
                        }
                    }

                    if (personelId == null)
                    {
                        result.ResultType = AccessSaveResultType.PersonNotFound;
                        return result;
                    }

                    int firmaIdToUse = firmaIdFromPerson ?? _defaultFirmaId;
                    int cihazId = _cihazId;
                    string tip = "Giris";
                    DateTime tarih = ev.Time;

                    // 2) Aynı kayıt zaten var mı?
                    using (var cmdCheck = conn.CreateCommand())
                    {
                        cmdCheck.CommandText = @"
IF EXISTS (
    SELECT 1 FROM KisiHareketler
    WHERE FirmaId    = @FirmaId
      AND CihazId    = @CihazId
      AND PersonelId = @PersonelId
      AND Tarih      = @Tarih
      AND Tip        = @Tip
)
    SELECT 1
ELSE
    SELECT 0;";

                        cmdCheck.Parameters.AddWithValue("@FirmaId", firmaIdToUse);
                        cmdCheck.Parameters.AddWithValue("@CihazId", cihazId);
                        cmdCheck.Parameters.AddWithValue("@PersonelId", personelId.Value);
                        cmdCheck.Parameters.AddWithValue("@Tarih", tarih);
                        cmdCheck.Parameters.AddWithValue("@Tip", tip);

                        int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (exists == 1)
                        {
                            result.ResultType = AccessSaveResultType.Duplicate;
                            return result;
                        }
                    }

                    // 3) Yeni hareket kaydını ekle
                    using (var cmdInsert = conn.CreateCommand())
                    {
                        cmdInsert.CommandText = @"
INSERT INTO KisiHareketler
    (FirmaId, CihazId, PersonelId, Tarih, Tip, KayitZamani, AktifMi)
VALUES
    (@FirmaId, @CihazId, @PersonelId, @Tarih, @Tip, @KayitZamani, @AktifMi);";

                        cmdInsert.Parameters.AddWithValue("@FirmaId", firmaIdToUse);
                        cmdInsert.Parameters.AddWithValue("@CihazId", cihazId);
                        cmdInsert.Parameters.AddWithValue("@PersonelId", personelId.Value);
                        cmdInsert.Parameters.AddWithValue("@Tarih", tarih);
                        cmdInsert.Parameters.AddWithValue("@Tip", tip);
                        cmdInsert.Parameters.AddWithValue("@KayitZamani", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@AktifMi", 1);

                        cmdInsert.ExecuteNonQuery();
                    }
                }

                result.ResultType = AccessSaveResultType.Inserted;
                return result;
            }
            catch (SqlException ex)
            {
                result.ResultType = AccessSaveResultType.Error;
                result.ErrorMessage = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                result.ResultType = AccessSaveResultType.Error;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }
}
