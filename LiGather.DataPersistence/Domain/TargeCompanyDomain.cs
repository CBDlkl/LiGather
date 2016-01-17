﻿using System;
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
        private readonly LiGatherContext _db = new LiGatherContext();

        public void Add(TargeCompanyEntity model)
        {
            lock (Obj)
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
            lock (Obj)
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
            lock (Obj)
            {
                var model = _db.TrCompanyEntities.Where(where).FirstOrDefault(t => t.IsAbnormal == false && t.IsSearched == false);
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
            lock (Obj)
            {
                _db.TrCompanyEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }

    }
}
