#pragma checksum "c:\Users\User\Documents\Developer\edu-croaker\Components\Croak.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1ffbc58389d0b7a3efbe5d071764613f8db53ddc"
// <auto-generated/>
#pragma warning disable 1591
namespace Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using edu_croaker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "c:\Users\User\Documents\Developer\edu-croaker\_Imports.razor"
using edu_croaker.Shared;

#line default
#line hidden
#nullable disable
    public class Croak : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "card");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "card-body");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.OpenElement(6, "h5");
            __builder.AddAttribute(7, "class", "card-title");
            __builder.AddContent(8, 
#nullable restore
#line 5 "c:\Users\User\Documents\Developer\edu-croaker\Components\Croak.razor"
                                CroakData.Title

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(9, "\r\n        ");
            __builder.OpenElement(10, "h6");
            __builder.AddAttribute(11, "class", "card-subtitle mb-2 text-muted");
            __builder.AddContent(12, 
#nullable restore
#line 6 "c:\Users\User\Documents\Developer\edu-croaker\Components\Croak.razor"
                                                   CroakData.Author

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n        ");
            __builder.OpenElement(14, "p");
            __builder.AddAttribute(15, "class", "card-text");
            __builder.AddContent(16, 
#nullable restore
#line 7 "c:\Users\User\Documents\Developer\edu-croaker\Components\Croak.razor"
                              CroakData.Content

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(18, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "c:\Users\User\Documents\Developer\edu-croaker\Components\Croak.razor"
       
    [Parameter]
    public edu_croaker.Data.Croak CroakData { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591