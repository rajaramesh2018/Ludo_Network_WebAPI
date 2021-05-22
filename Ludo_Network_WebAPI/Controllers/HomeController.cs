using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ludo_Network_WebAPI.DAL;
using System.Data.SqlClient;
using System.Data;

namespace Ludo_Network_WebAPI.Controllers
{
    public class HomeController : Controller
    {

        //sync flag 5 -- first step
        //sync flag - 100 --win
        //sync flag 0 --needs to sync
        //sync flag 1--synked

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public string  CreateUser(string UserName, string password, string Email, string Gender)
        {

            string cmd = "sp_CreateUser '" + UserName + "','" + password + "','" + Email + "','"+ Gender+"'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            //  return Json(new { StatusCode = ds.Tables[0].Rows[0]["UserId"].ToString(), Message = ds.Tables[0].Rows[0]["Message"].ToString() }, JsonRequestBehavior.AllowGet);
            return ds.Tables[0].Rows[0]["Message"].ToString()+"_"+ ds.Tables[0].Rows[0]["UserId"].ToString(); 
        }


        public string isVirtualUser(string UserName)
        {
            string res = "false";
            string cmd = "select * from tblUsers where username='"+ UserName + "' ";
            DataSet ds = DAL.DAL.getDtata(cmd);

            if (ds.Tables[0].Rows.Count > 0) {
                if (ds.Tables[0].Rows[0]["isVirtualUser"].ToString().Trim() == "1") {
                    res = "true";
                }
            }
            return res;
        }



        public string LoginUser(string UserName, string password)
        {

            string cmd = "sp_LoginUser '" + UserName + "','" + password +"'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            //  return Json(new { StatusCode = ds.Tables[0].Rows[0]["UserId"].ToString(), Message = ds.Tables[0].Rows[0]["Message"].ToString() }, JsonRequestBehavior.AllowGet);
            return ds.Tables[0].Rows[0]["Message"].ToString()+"_"+ ds.Tables[0].Rows[0]["UserID"].ToString();
        }

        public string getPassword(string UserName)
        {

            string cmd = "sp_getPassword '" + UserName +"'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            //  return Json(new { StatusCode = ds.Tables[0].Rows[0]["UserId"].ToString(), Message = ds.Tables[0].Rows[0]["Message"].ToString() }, JsonRequestBehavior.AllowGet);
            if (ds.Tables[0].Rows[0][0].ToString() == "******UserId Not Found*******")
            {
                return "UserId Not Found";
            }
            else {
                return "sent in mail: "+ ds.Tables[0].Rows[0][0].ToString();
            }
        }




        public JsonResult EnterLobby(string UserId, string UserName)
        {

            string cmd = "sp_EntertoLobby '" + UserId + "','" + UserName + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            return Json(new { StatusCode = ds.Tables[0].Rows[0]["UserId"].ToString(), Message = ds.Tables[0].Rows[0]["Message"].ToString() }, JsonRequestBehavior.AllowGet);
        }

        //sp_getMyGameSessionTableName

        public string getSessionTableNameOfUser(string UserId, string UserName)
        {

            string cmd = "sp_getMyGameSessionTableName '" + UserId + "','" + UserName + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);

            string UserName_backup = UserName;


            string result = "Failed"; 
            if (ds.Tables[0].Rows.Count > 0)
            {
                string[] tables = new string[] { ds.Tables[0].Rows[0]["Table1"].ToString(), ds.Tables[0].Rows[0]["Table2"].ToString() };
                // //mytable, opponentstable

                string mytable = "";
                string opponent_table = "";
                UserName = UserName.Replace('@','_');
                UserName = UserName.Replace('.', '_');
                foreach (string _table in tables)
                {

                    if (_table.Contains(UserName)  )
                    {
                        mytable = _table;
                    }
                    else {
                        opponent_table = _table;
                    }

                }
                //fetching opponent usermail id
                string gameSessionId = ds.Tables[0].Rows[0]["GameSessionID"].ToString();
                ////8:rajaramesh.mannem@gmail.com#10:sweeti@gmail.com#12112020 012434
                gameSessionId = gameSessionId.Replace(UserId+":"+ UserName_backup, "");
                gameSessionId = gameSessionId.Split(':')[1];
                gameSessionId = gameSessionId.Split('#')[0];
                string opponent_userid = gameSessionId;


                result = mytable +":"+ opponent_table+":"+ opponent_userid;
            }
            else
            {

            }
            return result;


        }

        public string deleteUserFromLobbyByUserid(string UserId)
        {
            string cmd = "sp_delFromLobby_byUserId " + UserId ;
            DataSet ds = DAL.DAL.getDtata(cmd);
            return "";
        }

        public string FirstStepConfirmation(string tableName) {

            string result = "";
            string cmd = "select * from  " + tableName+ " where StepDescription='null' and SyncFlag=5";
            DataSet ds = DAL.DAL.getDtata(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = "yes";
                string delCmd = "Delete from "+ tableName;
                DataSet ds1 = DAL.DAL.getDtata(delCmd);
            }
            else
            {
                result = "no";
            }

            return result;
        }


        public string getUpdateCoinsCount(string UserID, string CoinsCount)
        {
            string result = "";
            string cmd = "sp_updateCoinsCount "+ UserID+", "+ CoinsCount;
            DataSet ds = DAL.DAL.getDtata(cmd);

            if (ds.Tables[0].Rows.Count > 0) {
                result = ds.Tables[0].Rows[0]["Message"].ToString();
            }

            return result;
        }

        public string getUserCoins(string UserID)
        {
            string result = "0";
            string cmd = "select * from tblusercoins where userid= " + UserID;
            DataSet ds = DAL.DAL.getDtata(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["Coins"].ToString();
            }

            return result;
        }



        public string InsertDiceNumberDetails(string tableName, string dicenum)
        {
            string result = "";
            string cmd = "sp_InsertDiceNumberDetails '" + tableName + "' , '" + dicenum + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            result = "success";

            return result;
        }

        public string getDiceNumberDetails(string tableName)
        {
            string result = "NoStepStillNow";
            string cmd = "sp_getDiceNumberDetails '" + tableName + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);

            if (ds.Tables[0].Rows.Count > 0) {
                result = ds.Tables[0].Rows[0]["StepDescription"].ToString();
            }

           

            return result;
        }




        public string InsertStepDetails(string tableName, string stepInfo) {
            string result = "";
            string cmd = "sp_InsertStepDetails '"+ tableName + "' , '"+ stepInfo + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);
              result = "success";

            return result;
        }
        //[sp_InsertWinStepDetails] 'table_one1_TEMP' , ''
        public string InsertWINStepDetails(string tableName, string stepInfo)
        {
            string result = "";
            string cmd = "sp_InsertWinStepDetails '" + tableName + "' , '" + stepInfo + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            result = "success";

            return result;
        }

        //[sp_getTableInfotoEvalWin] @tableName
        public string getWINInfo(string tableName)
        {
            string result = "";
            string cmd = "sp_getTableInfotoEvalWin '" + tableName + "'";
            DataSet ds = DAL.DAL.getDtata(cmd);
            int score = 0;
            foreach (DataRow dr in ds.Tables[0].Rows) {
                int sc =int.Parse( dr["StepDescription"].ToString().Split(':')[0]);
                score += sc;
            }

            if (score >= 25) { result = "win"; }
            else { result = "not win"; }

            return result;
        }



        public string ReadOppStepDetails(string tableName) {
            string result = "";

            //if sync 100 is there means opponents win
            //sp_ReadOppWINDetails

            string cmd_win_check = "sp_ReadOppWINDetails '" + tableName + "'";
            DataSet ds_win = DAL.DAL.getDtata(cmd_win_check);

            if (ds_win.Tables[0].Rows.Count == 0)
            {
                string cmd = "sp_ReadOppStepDetails '" + tableName + "'";
                DataSet ds = DAL.DAL.getDtata(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["StepDescription"].ToString();
                    //step reading done, sync will be done by client, set sync flag here
                    string cmd_sync = "update " + tableName + " set syncflag=1 where syncflag=0";
                    DataSet ds1 = DAL.DAL.getDtata(cmd_sync);
                }
                else
                {
                    result = "no step till now";
                }
            }
            else {
                result = "opponent win";
            }

            return result;
        }

    }
}
