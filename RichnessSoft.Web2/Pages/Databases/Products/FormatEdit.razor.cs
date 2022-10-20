using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class FormatEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        Format format { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Format/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = formatService.GetById(Id);
                format = (Format)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                format = new Format();
                format.companyid = store.CurentCompany.id;
                format.active = ConstUtil.ACTIVE.YES;
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
                        results = formatService.Add(format);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = formatService.Edit(format);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            format = new Format();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/Format/{ParrentMenu}");
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
            var res = formatService.GetByCode(format.companyid, format.code);
            if (res.Data != null && !string.IsNullOrEmpty(((Format)res.Data)?.code))
            {
                Format OldData = (Format)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != format.id)
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
                format.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }

    }
}