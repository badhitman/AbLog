////////////////////////////////////////////////
// � https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.JSInterop;

namespace RazorLib
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

#pragma warning disable CS1591 // ����������� ����������� XML ��� ��������� �������� ���� ��� �����
    public class ExampleJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorLib/exampleJsInterop.js").AsTask());

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                IJSObjectReference module = await moduleTask.Value;
                await module.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
#pragma warning restore CS1591 // ����������� ����������� XML ��� ��������� �������� ���� ��� �����
}