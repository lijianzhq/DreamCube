using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader.Test
{
    public partial class testdb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBService.DB.SaveUploadFileRecord(new DBService.UploadFile()
            {
                SavePath = "xxxx",
                CODE = DBService.DB.GetGuid(),
                FileName = "aaaaaa",
                CreateBy = "1",
                LastUpdateBy = "1"
            });

            //using (var db = new DBService.DB())
            //{
            //    db.UPLOADFILE.Add(new DBService.UPLOADFILE()
            //    {
            //        SAVEPATH = "xxxx",
            //        CODE = DBService.DB.GetGuid(),
            //        FileName = "aaaaaa",
            //        CREATEBY = "1",
            //        LASTUPDATEBY = "1"
            //    });
            //    db.SaveChanges();
            //}
        }
    }
}