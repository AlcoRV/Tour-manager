using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency
{
    class DBData
    {
        static readonly string dbConfig = @"Data Source=DESKTOP-GJS201Q\SQLEXPRESS;Initial Catalog=ТурАгентство;Integrated Security=True";

        public Constants.AccountId GetVisitorType(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(dbConfig))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "ПроверкаВхода";
                    command.Parameters.Add("@Логин", System.Data.SqlDbType.VarChar, 20);
                    command.Parameters.Add("@Пароль", System.Data.SqlDbType.VarChar, 20);
                    command.Parameters.Add("@Статус", System.Data.SqlDbType.SmallInt);
                    command.Parameters.Add("@Номер", System.Data.SqlDbType.SmallInt);
                    command.Parameters["@Статус"].Direction = System.Data.ParameterDirection.Output;
                    command.Parameters["@Номер"].Direction = System.Data.ParameterDirection.Output;

                    command.Parameters["@Логин"].Value = login;
                    command.Parameters["@Пароль"].Value = password;

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();

                    var accountId = new Constants.AccountId();
                    accountId.visitorType = (Constants.AccountId.VisitorType)Enum.ToObject(typeof(Constants.AccountId.VisitorType), command.Parameters["@Статус"].Value);

                    if (!accountId.visitorType.Equals(Constants.AccountId.VisitorType.Error))
                    {
                        accountId.id = (short)command.Parameters["@Номер"].Value;
                    }

                    return accountId;
                }
            }
        }


    }
}
