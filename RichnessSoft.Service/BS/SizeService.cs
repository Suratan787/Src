using Microsoft.EntityFrameworkCore;
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
   public interface ISizeService
    {
        ResultModel GetAll(int CorpId)
    
        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(Sizex um);
        ResultModel Edit(Sizex um);
        ResultModel Delete(Sizex um);
    }
    public class SizeService : BaseService, ISizeService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public SizeService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }
        public ResultModel Add(Sizex sizex)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    sizex.createby = _store.CurrentUser.username;
                    sizex.createatutc = DateTime.Now;
                    sizex.companyid = _store.CurentCompany.id;
                    db.Add(sizex);
                    db.SaveChanges();
                    AddLog<Sizex>(sizex);
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

        public ResultModel Delete(Sizex sizex)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.Size.Where(x => x.id == sizex.id).FirstOrDefault();
                    db.Size.Remove(data);
                    DeleteLog<Sizex>(data);
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

        public ResultModel Edit(Sizex sizex)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.Size.Where(x => x.id == sizex.id).FirstOrDefault();
                    sizex.updateby = _store.CurrentUser.username;
                    sizex.companyid = _store.CurentCompany.id;
                    sizex.updateatutc = DateTime.Now;
                    db.Size.Update(sizex);
                    db.SaveChanges();
                    _db.Entry(sizex).State = EntityState.Detached;
                    UpdateLog<Sizex>(Olddata, sizex);
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
            res.Data = _db.Size.Where(x => x.companyid == CorpId&& (x.active.Equals(strActive)|| x.inactivedate >= DateTime.Now.Date)).ToList();
            return res;
        }

        public async Task<ResultModel> GetAllAsync(int CorpId)

        {
            ResultModel res = new ResultModel();
            res.Data = _db.Size.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public ResultModel GetByCode(int CorpId, string Code)

        {
            ResultModel res = new ResultModel();
            res.Data = _db.Size.Where(x => x.companyid == CorpId && x.code.Equals(Code)).FirstOrDefault();
            return res;
        }

        public ResultModel GetById(int Id)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Size.Where(x => x.id == Id).FirstOrDefault();
            return res;
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Size.Where(x => x.companyid == CorpId && x.code.Contains(Name)).ToList();
            return res;
        }
    }
}
