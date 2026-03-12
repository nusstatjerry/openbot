using BotLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotLib.Extensions;
using Bot.Common;

namespace Bot.Automation.ChatDeskNs.Automators
{
    public class QnAccountFinder
    {
        public virtual string ChatWindowTitlePattern
        {
            get
            {
                return "千牛接待台|接待台|客服";
            }
        }

        private static QnChatWnd currenrQNChatWnd;

        static QnAccountFinder()
        {
            currenrQNChatWnd = null;
        }


        private static readonly string[] QianniuProcessNames = new[]
        {
            "AliWorkbench",
            "qianniu",
            "Qianniu"
        };

        private static HashSet<int> GetAliWorkbenchPids()
        {
            var pids = new HashSet<int>();
            foreach (var processName in QianniuProcessNames)
            {
                var aliWorkbenchPs = Process.GetProcessesByName(processName);
                foreach (var p in aliWorkbenchPs.xSafeForEach())
                {
                    pids.Add(p.Id);
                }
            }
            return pids;
        }

        public virtual (QnChatWnd, QnChatWnd) GetSingleChatWnd()
        {
            var closedChatWnd = currenrQNChatWnd;
            var pids = GetAliWorkbenchPids();
            if(pids.Any(pid=> currenrQNChatWnd!=null && pid == currenrQNChatWnd.Pid)) {
                return (currenrQNChatWnd,closedChatWnd);
            }
            foreach (var pid in pids.xSafeForEach())
            {
                int hWnd;
                var title = GetWndTitle(pid, out hWnd);
                if (!string.IsNullOrEmpty(title) && hWnd != 0)
                {
                    var value = new QnChatWnd(title, hWnd ,pid);
                    currenrQNChatWnd = value;
                    break;
                }
            }
            return (currenrQNChatWnd, closedChatWnd);
        }

		private string GetWndTitle(int pid, out int hwnd)
		{
			var t = string.Empty;
			var htmp = 0;
			try
			{
			    foreach (var className in GetCandidateWindowClassNames())
                {
                    WinApi.FindAllDesktopWindowByClassNameAndTitlePattern(className, this.ChatWindowTitlePattern, (qnHwnd, title) =>
                    {
                        if (WinApi.IsVisible(qnHwnd))
                        {
                            t = title;
                            htmp = qnHwnd;
                        }
                    }, pid);

                    if (htmp != 0)
                    {
                        break;
                    }
                }
			}
			catch (Exception e)
			{
				Log.Exception(e);
			}
			hwnd = htmp;
			return t;
		}

        private static IEnumerable<string> GetCandidateWindowClassNames()
        {
            yield return "Qt5152QWindowIcon";
            yield return "Qt5QWindowIcon";
            yield return "Qt6QWindowIcon";
        }
    }
}
