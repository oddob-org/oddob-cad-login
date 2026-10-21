#if ZWCAD
using ZwSoft.ZwCAD.ApplicationServices;
using Application = ZwSoft.ZwCAD.ApplicationServices.Application;
#else
using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
#endif
using Oddob.Service;

namespace Oddob.Acad;

internal static class DocEvent
{
    public static void SetUp()
    {
        Application.DocumentManager.DocumentActivated -= DocumentManagerOnDocumentActivated;
        Application.DocumentManager.DocumentActivated += DocumentManagerOnDocumentActivated;
    }

    private static void DocumentManagerOnDocumentActivated(object sender,
        DocumentCollectionEventArgs e)
    {
        if (e.Document is null)
            return;
        IdleAction.Add(Initialize.Refresh);
    }
}