using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;
using RichnessSoft.Web2.Pages.Databases.Products;

namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class WeightsEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        Weight weight { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Weight/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = sizeService.GetById(Id);
                weight = (Weight)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                weight = new Weight();
                weight.companyid = store.CurentCompany.id;
                weight.active = ConstUtil.ACTIVE.YES;
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
                        results = weightService.Add(weight);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = weightService.Edit(weight);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            weight = new Weight();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/Weight/{ParrentMenu}");
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
            var res = weightService.GetByCode(weight.companyid, weight.code);
            if (res.Data != null && !string.IsNullOrEmpty(((Sizex)res.Data)?.code))
            {
                Weight OldData = (Weight)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != weight.id)
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
            var sss = values.ToArray();
            if (sss[0] == ConstUtil.ACTIVE.YES)
            {
                weight.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }

    }
}