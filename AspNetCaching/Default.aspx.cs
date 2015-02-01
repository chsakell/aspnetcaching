using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetCaching
{
    public partial class Default : System.Web.UI.Page
    {
        public static Object thisLock = new Object();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime timeStart = DateTime.Now;
            if (!IsPostBack)
            {
                ReloadAndCacheCustomers();
                DateTime timeLoaded = DateTime.Now;
                TimeSpan timeElapsed = timeLoaded.Subtract(timeStart);
                lblDataRetrievedFrom.Text = " database..";
                lblNumberOfCustomers.Text = GridView1.Rows.Count.ToString();
                lblTimeElapsed.Text = timeElapsed.TotalMilliseconds.ToString() + " milliseconds" +
                    " (" + timeElapsed.Seconds.ToString() + " sec)";
            }

        }

        private void ReloadAndCacheCustomers()
        {
            // Read connection string from web.config file
            string CS = ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;

            lock (thisLock)
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlDataAdapter da = new SqlDataAdapter("spGetCustomers", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    CacheItemRemovedCallback onCacheItemRemoved = new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod);

                    // Build SqlCacheDependency object using the database and table names
                    SqlCacheDependency sqlDependency = new SqlCacheDependency("Store", "Customers");

                    // Pass SqlCacheDependency object, when caching data
                    Cache.Insert("CustomersData", ds, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration,
                    CacheItemPriority.Default, onCacheItemRemoved);

                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                }
            }
        }

        public void CacheItemRemovedCallbackMethod(string key, object value, CacheItemRemovedReason reason)
        {
            string CS = ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;
            lock (thisLock)
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlDataAdapter da = new SqlDataAdapter("spGetCustomers", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds);


                    CacheItemRemovedCallback onCacheItemRemoved = new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod);

                    SqlCacheDependency sqlDependency = new SqlCacheDependency("Store", "Customers");
                    Cache.Insert("CustomersData", ds, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration,
                        CacheItemPriority.Default, onCacheItemRemoved);
                    return;
                }
            }
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            DateTime timeStart = DateTime.Now;

            // Check if the DataSet is present in cache
            if (Cache["CustomersData"] != null)
            {
                // If data available in cache, retrieve and bind it to gridview control
                GridView1.DataSource = (DataSet)Cache["CustomersData"];
                GridView1.DataBind();
                DateTime timeLoaded = DateTime.Now;
                TimeSpan timeElapsed = timeLoaded.Subtract(timeStart);
                lblDataRetrievedFrom.Text = " cache..";
                lblNumberOfCustomers.Text = GridView1.Rows.Count.ToString();
                lblTimeElapsed.Text = timeElapsed.TotalMilliseconds.ToString() + " milliseconds" +
                    " (" + timeElapsed.Seconds.ToString() + " sec)";

            }
            else
            {

                ReloadAndCacheCustomers();
                DateTime timeLoaded = DateTime.Now;
                TimeSpan timeElapsed = timeLoaded.Subtract(timeStart);
                lblDataRetrievedFrom.Text = " database..";
                lblNumberOfCustomers.Text = GridView1.Rows.Count.ToString();
                lblTimeElapsed.Text = timeElapsed.TotalMilliseconds.ToString() + " milliseconds" +
                    " (" + timeElapsed.Seconds.ToString() + " sec)";
            }
        }
    }
}