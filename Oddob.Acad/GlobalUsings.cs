// global using 指令

global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Runtime.InteropServices;
#if ZWCAD
global using ZwSoft.ZwCAD.DatabaseServices;
global using ZwSoft.ZwCAD.Runtime;
global using Acap = ZwSoft.ZwCAD.ApplicationServices.Application;
#else
global using Autodesk.AutoCAD.DatabaseServices;
global using Autodesk.AutoCAD.Runtime;
global using Acap =Autodesk.AutoCAD.ApplicationServices.Application;
#endif
global using DYH.CAD.Extensions;
global using DYH.Extensions.DependencyInjection;
global using Oddob.Service.Service.Interface;
global using Oddob.Service.View.Account;
global using Cursor = System.Windows.Forms.Cursor;
global using Exception = System.Exception;