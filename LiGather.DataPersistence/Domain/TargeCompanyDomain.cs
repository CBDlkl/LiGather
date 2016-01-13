using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using LiGather.Model;

namespace LiGather.DataPersistence.Domain
{
    public class TargeCompanyDomain
    {
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static void Add(TargeCompanyEntity model)
        {
            lock (Db)
            {
                Db.TrCompanyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取一个非以查询，非异常状态的企业实体
        /// </summary>
        /// <returns></returns>
        public static TargeCompanyEntity GetSingel()
        {
            lock (Db)
            {
                var model = Db.TrCompanyEntities.FirstOrDefault(t => t.IsAbnormal == false && t.IsSearched == false);
                if (model != null)
                {
                    model.IsSearched = true;
                    Update(model);
                }
                return model;
            }
        }

        public static void Update(TargeCompanyEntity model)
        {
            lock (Db)
            {
                Db.TrCompanyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }

    }
}
