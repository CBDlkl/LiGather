using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class TargeCompanyDomain
    {
        private static readonly object Obj = new object();
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static void Add(TargeCompanyEntity model)
        {
            lock (Obj)
            {
                Db.TrCompanyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }

        /// <summary>
        /// 按照条件获取数据和
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetInt(Expression<Func<TargeCompanyEntity, bool>> where)
        {
            lock (Obj)
            {
                return Db.TrCompanyEntities.Count(where);
            }
        }

        /// <summary>
        /// 获取一个非以查询，非异常状态的企业实体，同时更新为已搜索状态
        /// </summary>
        /// <returns></returns>
        public static TargeCompanyEntity GetSingel(Expression<Func<TargeCompanyEntity, bool>> where)
        {
            lock (Obj)
            {
                var model = Db.TrCompanyEntities.Where(where).FirstOrDefault(t => t.IsAbnormal == false && t.IsSearched == false);
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
            lock (Obj)
            {
                Db.TrCompanyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }

    }
}
