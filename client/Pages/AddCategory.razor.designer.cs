using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Net5Wasm.Models.Net5Wasmconn;
using Microsoft.AspNetCore.Identity;
using Net5Wasm.Models;
using Net5Wasm.Client.Pages;

namespace Net5Wasm.Pages
{
    public partial class AddCategoryComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected Net5WasmconnService Net5Wasmconn { get; set; }

        Net5Wasm.Models.Net5Wasmconn.Category _category;
        protected Net5Wasm.Models.Net5Wasmconn.Category category
        {
            get
            {
                return _category;
            }
            set
            {
                if (!object.Equals(_category, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "category", NewValue = value, OldValue = _category };
                    _category = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            category = new Net5Wasm.Models.Net5Wasmconn.Category(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Net5Wasm.Models.Net5Wasmconn.Category args)
        {
            try
            {
                var net5WasmconnCreateCategoryResult = await Net5Wasmconn.CreateCategory(category:category);
                DialogService.Close(category);
            }
            catch (System.Exception net5WasmconnCreateCategoryException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Category!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
