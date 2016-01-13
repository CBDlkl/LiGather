﻿using System.Data.Entity.Migrations;
using LiGather.Model.Log;

namespace LiGather.DataPersistence.Domain
{
    public class LogDomain
    {
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static void Add(LogEntity model)
        {
            lock (Db)
            {
                Db.LogEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }
    }
}
