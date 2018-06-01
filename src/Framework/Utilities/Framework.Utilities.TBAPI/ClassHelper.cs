using System;
using System.Collections.Generic;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 类目帮助类
    /// </summary>
    public static class ClassHelper
    {
        /// <summary>
        /// 获取所有淘宝类目数据
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static List<ItemCat> GetTBWAllLM(String appKey, String appSecret)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            ItemcatsGetRequest req = new ItemcatsGetRequest();
            req.Fields = "cid,parent_cid,name,is_parent,status,sort_order,features";
            req.ParentCid = 0;
            ItemcatsGetResponse response = client.Execute(req);
            List<ItemCat> allItemCats = new List<ItemCat>();
            Queue<ItemCat> helperQueue = new Queue<ItemCat>(); //辅助的堆栈对象
            if (!response.IsError && response.ItemCats.Count>0)
            {
                allItemCats.AddRange(response.ItemCats);
                //把顶级类目入栈
                for(var i = 0;i<allItemCats.Count;i++)
                {
                    if (allItemCats[i].IsParent) helperQueue.Enqueue(allItemCats[i]);
                }
                while (helperQueue.Count > 0)
                {
                    ItemCat tempItemCat = helperQueue.Dequeue();
                    if (tempItemCat.IsParent)
                    {
                        response = GetSubClass(appKey, appSecret, tempItemCat.Cid);
                        if (!response.IsError && response.ItemCats.Count > 0)
                        {
                            //收集所有的类目数据
                            allItemCats.AddRange(response.ItemCats);
                            //把类目入栈
                            for (var i = 0; i < response.ItemCats.Count; i++)
                            {
                                if (response.ItemCats[i].IsParent) helperQueue.Enqueue(response.ItemCats[i]);
                            }
                        }
                    }
                }
            }
            return allItemCats;
        }

        /// <summary>
        /// 传入叶子类目获取类目的参数
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static ItemcatsGetResponse GetClass(String appKey, String appSecret,String cid)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            ItemcatsGetRequest req = new ItemcatsGetRequest();
            req.Fields = "cid,parent_cid,name,is_parent,status,sort_order,features";
            req.Cids = cid;
            return client.Execute(req);
        }

        /// <summary>
        /// 传入父类目获取所有的子类目
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="parent_cid"></param>
        /// <returns></returns>
        public static ItemcatsGetResponse GetSubClass(String appKey, String appSecret, long parent_cid)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            ItemcatsGetRequest req = new ItemcatsGetRequest();
            req.Fields = "cid,parent_cid,name,is_parent,status,sort_order,features";
            req.ParentCid = parent_cid;
            return client.Execute(req);
        }

        /// <summary>
        /// 根据类目的子ID获取类目的完整名称
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static String GetClassFullNameByID(String appKey, String appSecret, String cid)
        {
            ItemcatsGetResponse response = GetClass(appKey, appSecret, cid);
            if (!response.IsError && response.ItemCats.Count > 0)
            {
                String name = response.ItemCats[0].Name;
                if (response.ItemCats[0].ParentCid != 0)
                {
                    name = GetClassFullNameByID(appKey, appSecret, response.ItemCats[0].ParentCid.ToString()) + "/" + name;
                }
                return name;
            }
            return String.Empty;
        }

        /// <summary>
        /// 根据类目的子ID获取类目的名称
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static String GetClassNameByID(String appKey, String appSecret, String cid)
        {
            ItemcatsGetResponse response = GetClass(appKey, appSecret, cid);
            if (!response.IsError && response.ItemCats.Count > 0)
            {
                return response.ItemCats[0].Name;
            }
            return String.Empty;
        }
    }
}
