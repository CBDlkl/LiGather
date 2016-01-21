using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class TargeCompanyDomain
    {
        private readonly LiGatherContext _db = new LiGatherContext();
        private readonly object _obj = new object();

        public List<TargeCompanyEntity> Get(Expression<Func<TargeCompanyEntity, bool>> expression)
        {
            lock (_obj)
            {
                return _db.TrCompanyEntities.Where(expression)?.ToList();
            }
        }

        public void Add(TargeCompanyEntity model)
        {
            lock (_obj)
            {
                _db.TrCompanyEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 按照条件获取数据和
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetInt(Expression<Func<TargeCompanyEntity, bool>> where)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                return _db.TrCompanyEntities.Count(where);
            }
        }

        /// <summary>
        /// 获取一个非以查询，非异常状态的企业实体，同时更新为已搜索状态
        /// </summary>
        /// <returns></returns>
        public TargeCompanyEntity GetSingel(Expression<Func<TargeCompanyEntity, bool>> where)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                var model = _db.TrCompanyEntities.Where(where).FirstOrDefault(t => t.IsSearched == false);
                if (model != null)
                {
                    model.IsSearched = true;
                    Update(model);
                }
                return model;
            }
        }

        public void Update(TargeCompanyEntity model)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                _db.TrCompanyEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }

    }
}
