using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;
using RichnessSoft.Service.BS;


namespace RichnessSoft.Web2.Pages.Databases.Customers
{
    public partial class CustGroupsEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        CustGroup custgroup { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/CustGroup/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = custgroupService.GetById(Id);
                custgroup = (CustGroup)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                custgroup = new CustGroup();
                custgroup.companyid = store.CurentCompany.id;
                custgroup.Active = ConstUtil.ACTIVE.YES;
            }
        }

        async void SaveAsync()
        {
            ResultModel results = new ResultModel();
            try
            {
                _loaded = true;
                string strErrMsg = "";
                string strErrFocus = "";
                if (Validated && CheckDupCode())
                {
                    if (Mode == gbVar.ModeInsert)
                    {
                        results = custgroupService.Add(custgroup);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = custgroupService.Edit(custgroup);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            custgroup = new CustGroup();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/CustGroup/{ParrentMenu}");
                        }
                    }
                    else
                    {
                        await Dialog.ShowMessageBox("error", Lng["SAVE_MSG_FAIL"], "OK");
                        await Dialog.ShowMessageBox("info", results.Message, "OK");
                    }
                    StateHasChanged();
                }
                _loaded = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckDupCode()
        {
            bool bSucc = true;
            var res = custgroupService.GetByCode(custgroup.companyid, custgroup.code);
            if (res.Data != null && !string.IsNullOrEmpty(((CustGroup)res.Data)?.code))
            {
                CustGroup OldData = (CustGroup)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != custgroup.id)
                {
                    bSucc = false;
                }
            }
            if (bSucc == false)
            {
                _snackBar.Add(Lng["DUPLICATED_CODE"], Severity.Error);
            }
            return bSucc;
        }

        async void activeChange(IEnumerable<string> values)
        {
            var fm = values.ToArray();
            if (fm[0] == ConstUtil.ACTIVE.YES)
            {
                custgroup.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }

    }
}