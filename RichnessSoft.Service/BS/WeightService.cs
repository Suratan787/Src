using Microsoft.EntityFrameworkCore;
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
    public interface IWeightService
    {
        ResultModel GetAll(int CorpId);
        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(Weight um);
        ResultModel Edit(Weight um);
        ResultModel Delete(Weight um);
    }
    public class WeightService : BaseService ,IWeightService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public WeightService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }
        public ResultModel Add(Weight wg)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    wg.createby = _store.CurrentUser.username;
                    wg.createatutc = DateTime.Now;
                    wg.companyid = _store.CurentCompany.id;
                    db.Add(wg);
                    db.SaveChanges();
                    AddLog<Weight>(wg);
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

        public ResultModel Delete(Weight wg)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.Weight.Where(x => x.id == wg.id).FirstOrDefault();
                    db.Weight.Remove(data);
                    DeleteLog<Weight>(data);
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

        public ResultModel Edit(Weight wg)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.Weight.Where(x => x.id == wg.id).FirstOrDefault();
                    wg.updateby = _store.CurrentUser.username;
                    wg.companyid = _store.CurentCompany.id;
                    wg.updateatutc = DateTime.Now;
                    db.Weight.Update(wg);
                    db.SaveChanges();
                    _db.Entry(wg).State = EntityState.Detached;
                    UpdateLog<Weight>(Olddata, wg);
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
            ResultModel res = new ResultModel();
            res.Data = _db.Weight.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public async Task<ResultModel> GetAllAsync(int CorpId)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Weight.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public ResultModel GetByCode(int CorpId, string Code)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Weight.Where(x => x.companyid == CorpId && x.code.Equals(Code)).FirstOrDefault();
            return res;
        }

        public ResultModel GetById(int Id)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Weight.Where(x => x.id == Id).FirstOrDefault();
            return res;
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Weight.Where(x => x.companyid == CorpId && x.code.Contains(Name)).ToList();
            return res;
        }
    }
}
