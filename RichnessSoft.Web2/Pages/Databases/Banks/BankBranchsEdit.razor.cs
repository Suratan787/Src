using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Banks
{
    public partial class BankBranchsEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        BankBranch bankbranch { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/BankBranch/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = bankbranchService.GetById(Id);
                bankbranch = (BankBranch)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                bankbranch = new BankBranch();
                bankbranch.companyid = store.CurentCompany.id;
                bankbranch.active = ConstUtil.ACTIVE.YES;
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
                        results = bankbranchService.Add(bankbranch);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = bankbranchService.Edit(bankbranch);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            bankbranch = new BankBranch();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/BankBranch/{ParrentMenu}");
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
            var res = bankbranchService.GetByCode(bankbranch.companyid, bankbranch.code);
            if (res.Data != null && !string.IsNullOrEmpty(((BankBranch)res.Data)?.code))
            {
                BankBranch OldData = (BankBranch)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != bankbranch.id)
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
                bankbranch.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }

    }
}