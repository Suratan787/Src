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
   public interface IProjectService
    {
        ResultModel GetAll(int CorpId);
        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(Project um);
        ResultModel Edit(Project um);
        ResultModel Delete(Project um);
    }
    public class ProjectService : BaseService, IProjectService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public ProjectService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }
        public ResultModel Add(Project project)
        {

            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    project.createby = _store.CurrentUser.username;
                    project.createatutc = DateTime.Now;
                    project.companyid = _store.CurentCompany.id;
                    db.Add(project);
                    db.SaveChanges();
                    AddLog<Project>(project);
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

        public ResultModel Delete(Project project)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.Project.Where(x => x.id == project.id).FirstOrDefault();
                    db.Project.Remove(data);
                    DeleteLog<Project>(data);
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

        public ResultModel Edit(Project project)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.Project.Where(x => x.id == project.id).FirstOrDefault();
                    project.updateby = _store.CurrentUser.username;
                    project.companyid = _store.CurentCompany.id;
                    project.updateatutc = DateTime.Now;
                    db.Project.Update(project);
                    db.SaveChanges();
                    _db.Entry(project).State = EntityState.Detached;
                    UpdateLog<Project>(Olddata, project);
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
            throw new NotImplementedException();
        }

        public Task<ResultModel> GetAllAsync(int CorpId)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetByCode(int CorpId, string Code)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            throw new NotImplementedException();
        }
    }
}
