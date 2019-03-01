using System.Data;
using System.Data.SqlClient;

namespace DAL.Laipinche
{
    class DBHelper
    {
        static string conStr = "Data Source=.;Initial Catalog=Laipingche;Persist Security Info=True;User ID=sa;Password=123456";
        static SqlConnection conn;


        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        private static void InitConnection()
        {
            if (conn == null || conn.State != ConnectionState.Open)
                conn = new SqlConnection(conStr);
            conn.Open();
        }
        /// <summary>
        /// 执行数据库语句
        /// </summary>
        /// <param name="cmd">数据库语句</param>
        /// <returns>受影响行数</returns>
        public static int Exec(string cmd)
        {
            InitConnection();
            SqlCommand sqlCmd = new SqlCommand(cmd, conn);
            return sqlCmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="cmd">数据库语句</param>
        /// <returns>表数据</returns>
        public static DataTable GetTable(string cmd)
        {
            InitConnection();
            SqlDataAdapter sda = new SqlDataAdapter(cmd, conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
    }
}
