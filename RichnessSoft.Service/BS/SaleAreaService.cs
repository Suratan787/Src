﻿using Microsoft.EntityFrameworkCore;
using RichnessSoft.Common;
using RichnessSoft.Entity.Context;
using RichnessSoft.Entity.Model;
using RichnessSoft.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Service.BS
{
   public interface ISaleAreaService
    {
        ResultModel GetAll(int CorpId);

        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(SaleArea um);
        ResultModel Edit(SaleArea um);
        ResultModel Delete(SaleArea um);
    }
    public class SaleAreaService : BaseService, ISaleAreaService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public SaleAreaService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }
        public ResultModel Add(SaleArea saleArea)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    saleArea.createby = _store.CurrentUser.username;
                    saleArea.createatutc = DateTime.Now;
                    saleArea.companyid = _store.CurentCompany.id;
                    db.Add(saleArea);
                    db.SaveChanges();
                    AddLog<SaleArea>(saleArea);
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel Delete(SaleArea saleArea)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.SaleArea.Where(x => x.id == saleArea.id).FirstOrDefault();
                    db.SaleArea.Remove(data);
                    DeleteLog<SaleArea>(data);
                    db.SaveChanges();
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel Edit(SaleArea saleArea)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.SaleArea.Where(x => x.id == saleArea.id).FirstOrDefault();
                    saleArea.updateby = _store.CurrentUser.username;
                    saleArea.companyid = _store.CurentCompany.id;
                    saleArea.updateatutc = DateTime.Now;
                    db.SaleArea.Update(saleArea);
                    db.SaveChanges();
                    _db.Entry(saleArea).State = EntityState.Detached;
                    UpdateLog<SaleArea>(Olddata, saleArea);
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel GetAll(int CorpId)
        {
            return GetAll(CorpId, ConstUtil.ACTIVE.YES);
        }
        public ResultModel GetAll(int CorpId, string strActive = ConstUtil.ACTIVE.YES)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.SaleArea.Where(x => x.companyid == CorpId && (x.active.Equals(strActive) || x.inactivedate >= DateTime.Now.Date)).ToList();
            return res;
        }

        public async Task<ResultModel> GetAllAsync(int CorpId)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.SaleArea.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public ResultModel GetByCode(int CorpId, string Code)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.SaleArea.Where(x => x.companyid == CorpId && x.code.Equals(Code)).FirstOrDefault();
            return res;
        }

        public ResultModel GetById(int Id)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.SaleArea.Where(x => x.id == Id).FirstOrDefault();
            return res;
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.SaleArea.Where(x => x.companyid == CorpId && x.code.Contains(Name)).ToList();
            return res;
        }
    }
}
